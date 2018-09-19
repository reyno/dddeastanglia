using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using DDDEastAnglia.Api.Exceptions;

namespace DDDEastAnglia.Api.MediatR {
    public class PipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> {
        private readonly HttpContext _httpContext;
        private readonly IServiceProvider _services;

        public PipelineBehavior(
            IHttpContextAccessor httpContextAccessor,
            IServiceProvider services
            ) {
            _httpContext = httpContextAccessor.HttpContext;
            _services = services;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next
            ) {


            await Authorise(request, cancellationToken);

            await Validate(request, cancellationToken);

            return await next();

        }

        private async Task Authorise(TRequest request, CancellationToken cancellationToken) {

            // Get required services
            var schemeProvider = _httpContext.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();
            var policyEvaluator = _httpContext.RequestServices.GetRequiredService<IPolicyEvaluator>();
            var policyProvider = _httpContext.RequestServices.GetRequiredService<IAuthorizationPolicyProvider>();


            // get the default policy and use it to authenticate
            var defaultPolicy = await policyProvider.GetDefaultPolicyAsync();
            var authenticateResult = await policyEvaluator.AuthenticateAsync(defaultPolicy, _httpContext);


            // get the authorize attributes from the request and combine
            var requestPolicies = typeof(TRequest).GetCustomAttributes<AuthorizeAttribute>(inherit: true);
            var authorizePolicy = await AuthorizationPolicy.CombineAsync(policyProvider, requestPolicies) ?? defaultPolicy;


            // now do the authorization
            var authorizeResult = await policyEvaluator.AuthorizeAsync(authorizePolicy, authenticateResult, _httpContext, resource: null);


            // throw forbidden if not successful
            if (!authorizeResult.Succeeded) throw new ForbiddenException();

        }

        private async Task Validate(TRequest request, CancellationToken cancellationToken) {

            var validators = _services.GetServices<RequestValidator<TRequest>>();

            foreach (var validator in validators)
                await validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);
                

        }
    }
}
