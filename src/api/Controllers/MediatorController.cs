using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace DDDEastAnglia.Api.Controllers {
    [Route("mediator")]
    [ApiController]
    public class MediatorController : ControllerBase {
        private readonly ILogger<MediatorController> _logger;
        private readonly IMediator _mediator;
        private readonly MvcJsonOptions _mvcJsonOptions;

        public MediatorController(
            ILogger<MediatorController> logger,
            IMediator mediator,
            IOptions<MvcJsonOptions> mvcJsonOptionsAccessor
        ) {
            _logger = logger;
            _mediator = mediator;
            _mvcJsonOptions= mvcJsonOptionsAccessor.Value;
        }

        [HttpPost("{name}")]
        public async Task<ActionResult> Handle(string name) {


            try {

                // Resolve the request and response type
                var (requestType, responseType) = ResolveType(name);

                // if we're unable to resolve request or response types, throw a 404
                if (requestType == null || responseType == null) return NotFound("Request not found");

                // deserialize the request object
                var request = GetRequest(requestType);

                // use reflection to call mediator with the correct generics
                var response = await Send(request, responseType);

                return Ok(response);

            } catch (ValidationException e) {
                return StatusCode(400, new {
                    ValidationErrors = e.Errors.Select(error => new {
                        Message = error.ErrorMessage,
                        Property = error.PropertyName,
                        Value = error.AttemptedValue
                    })
                });
            } catch (Exception e) {
                return StatusCode(500, new {
                    Message = e.Message
                });
            }

        }

        private (Type, Type) ResolveType(string name) {

            // Highly opinionated approach to resolving the full type name of the request
            var fullName = $"DDDEastAnglia.Api.MediatR.Requests.{name}Request";

            var requestType = GetType()
                .Assembly
                .DefinedTypes
                .SingleOrDefault(x => x.FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase))
                ;

            // Not found, so return null
            if (requestType == null) return (null, null);

            // figure out what the response type is from the request type
            var responseType = requestType
                .GetTypeInfo()
                .ImplementedInterfaces
                .FirstOrDefault(x => x.IsGenericType)?
                .GenericTypeArguments
                .FirstOrDefault()
                ;

            // return a typle of the request/response types
            return (requestType, responseType);

        }


        private IBaseRequest GetRequest(Type type) {

            IBaseRequest request;
            var serializer = new JsonSerializer();
            using (var reader = new StreamReader(Request.Body))
            using (var jsonReader = new JsonTextReader(reader))
                request = (IBaseRequest)serializer.Deserialize(jsonReader, type);

            // either return the deserialized request a new instance of the request type
            return request ?? (IBaseRequest)Activator.CreateInstance(type);

        }

        private async Task<object> Send(IBaseRequest request, Type responseType) {

            var sendMethod = _mediator.GetType().GetMethods().SingleOrDefault(x
                => x.Name == "Send"
                && x.IsGenericMethod
                );

            var genericSendMethod = sendMethod.MakeGenericMethod(responseType);

            var task = (Task)genericSendMethod.Invoke(_mediator, new object[] { request, default(CancellationToken) });

            await task;

            var resultProperty = task.GetType().GetProperty("Result");
            return resultProperty.GetValue(task);

        }

        private async Task WriteResponse<T>(int status, T content = default) where T : class {

            Response.StatusCode = status;

            if (content != default(T)) {
                var json = JsonConvert.SerializeObject(content, _mvcJsonOptions.SerializerSettings);
                await Response.WriteAsync(json);
            }

        }
    }
}
