using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Validators;

namespace DDDEastAnglia.Api.MediatR {
    public class PipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> {
        private readonly IServiceProvider _services;

        public PipelineBehavior(
            IServiceProvider services
            ) {
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

        private Task Authorise(TRequest request, CancellationToken cancellationToken) {

            throw new NotImplementedException();

        }

        private async Task Validate(TRequest request, CancellationToken cancellationToken) {

            var validators = _services.GetServices<RequestValidator<TRequest>>();

            foreach (var validator in validators)
                await validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);
                

        }
    }
}
