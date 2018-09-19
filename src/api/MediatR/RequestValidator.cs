using FluentValidation;

namespace DDDEastAnglia.Api.MediatR {

    public abstract class RequestValidator<TRequest> : AbstractValidator<TRequest> { }

}
