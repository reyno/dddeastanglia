using DDDEastAnglia.Api.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace DDDEastAnglia.Api.MediatR {
    public class MediatorMiddleware {
        private readonly RequestDelegate _next;
        private readonly ILogger<MediatorMiddleware> _logger;
        private readonly IMediatorRequestResolver _requestResolver;
        private readonly MediatorOptions _mediatorOptions;
        private MediatorJsonOptions _mediatorJsonOptions;

        public MediatorMiddleware(
            RequestDelegate next,
            ILogger<MediatorMiddleware> logger,
            IMediatorRequestResolver requestResolver,
            IOptions<MediatorOptions> mediatorOptionsAccessor,
            IOptions<MediatorJsonOptions> mediatorJsonOptionsAccessor
            ) {
            _next = next;
            _logger = logger;
            _requestResolver = requestResolver;
            _mediatorOptions = mediatorOptionsAccessor.Value;
            _mediatorJsonOptions = mediatorJsonOptionsAccessor.Value;
        }

        public async Task Invoke(HttpContext httpContext) {

            var prefix = $"/{_mediatorOptions.RoutePrefix.TrimStart('/')}";

            if (httpContext.Request.Path.StartsWithSegments(prefix))
                await HandleMediator(httpContext);
            else
                await _next(httpContext);

        }

        private async Task HandleMediator(HttpContext httpContext) {

            var mediator = httpContext.RequestServices.GetService<IMediator>();

            try {

                var commandName = GetCommandName(httpContext.Request);


                // Resolve the request and response type
                var requestType = _requestResolver.Resolve(commandName);
                var responseType = ResolveResponseType(requestType);

                // if we're unable to resolve request or response types, throw a 404
                if (requestType == null
                    || responseType == null
                    ) throw new InvalidOperationException("Command not found");

                // deserialize the request object
                var request = GetRequest(httpContext.Request, requestType);

                // use reflection to call mediator with the correct generics
                var response = await Send(mediator, request, responseType);

                // pass the result back to the client
                await SendResponse(httpContext.Response, StatusCodes.Status200OK, response);

            } catch (ForbiddenException e) {
                await SendResponse(httpContext.Response, StatusCodes.Status403Forbidden, e);
            } catch (ValidationException e) {
                _logger.LogError(e, null);
                await SendResponse(httpContext.Response, StatusCodes.Status400BadRequest, new {
                    e.Message,
                    ValidationErrors = e.Errors.Select(x => new {
                        x.ErrorMessage,
                        x.PropertyName,
                        x.AttemptedValue
                    })
                });
            } catch (InvalidOperationException e) {
                _logger.LogError(e, null);
                await SendResponse(httpContext.Response, StatusCodes.Status400BadRequest, e);
            } catch (Exception e) {
                _logger.LogError(e, null);
                await SendResponse(httpContext.Response, StatusCodes.Status500InternalServerError, e);
            }

        }



        private string GetCommandName(HttpRequest request) {

            var pathParts = request.Path.Value.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            if (pathParts.Length < 2) throw new InvalidOperationException("Missing command name from path");

            var commandName = pathParts[1];

            if (string.IsNullOrEmpty(commandName)) throw new InvalidOperationException("Command name empty");

            return commandName;

        }

        private Type ResolveResponseType(Type requestType) {

            var responseType = requestType
                .GetTypeInfo()
                .ImplementedInterfaces
                .FirstOrDefault(x => x.IsGenericType)?
                .GenericTypeArguments
                .FirstOrDefault()
                ;

            return responseType;

        }


        private IBaseRequest GetRequest(HttpRequest request, Type type) {

            IBaseRequest data;
            var serializer = new JsonSerializer();
            using (var reader = new StreamReader(request.Body))
            using (var jsonReader = new JsonTextReader(reader))
                data = (IBaseRequest)serializer.Deserialize(jsonReader, type);

            // either return the deserialized request a new instance of the request type
            return data ?? (IBaseRequest)Activator.CreateInstance(type);

        }

        private async Task<object> Send(IMediator mediator, IBaseRequest request, Type responseType) {

            var sendMethod = mediator.GetType().GetMethods().SingleOrDefault(x
                => x.Name == "Send"
                && x.IsGenericMethod
                );

            var genericSendMethod = sendMethod.MakeGenericMethod(responseType);

            var task = (Task)genericSendMethod.Invoke(mediator, new object[] { request, default(CancellationToken) });

            await task;

            var resultProperty = task.GetType().GetProperty("Result");
            return resultProperty.GetValue(task);

        }

        private void SendResponse(HttpResponse response, int statusCode) {
            response.StatusCode = statusCode;
        }

        private async Task SendResponse(HttpResponse response, int statusCode, Exception e) {

            await SendResponse(response, statusCode, new {
                e.Message,
                Stack = e.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
            });

        }


        private async Task SendResponse(HttpResponse response, int statusCode, object data) {

            response.StatusCode = statusCode;
            await response.WriteAsync(JsonConvert.SerializeObject(data, _mediatorJsonOptions.SerializerSettings));

        }

    }

}
