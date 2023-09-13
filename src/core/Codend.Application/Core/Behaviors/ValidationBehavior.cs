using Codend.Application.Exceptions;
using FluentValidation;
using MediatR;
using ValidationException = Codend.Application.Exceptions.ValidationException;

namespace Codend.Application.Core.Behaviors;

/// <summary>
/// Request validation pipeline implementation. Validates current request with every validator implemented for it.
/// Throws an exception if there are any validation errors.
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

    /// <inheritdoc />
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationFailures = _validators
            .Select(x => x.ValidateAsync(context, cancellationToken))
            .SelectMany(x => x.Result.Errors)
            .Where(x => x != null)
            .ToList();

        // NotFound validation errors are marked as Warning
        var notFoundFailures = validationFailures
            .Where(x => x.Severity == Severity.Warning)
            .ToList();

        // If there are more 'all' errors than notFound errors it means that there is validation error
        // therefor validation exception is thrown.
        if (validationFailures.Any() && validationFailures.Count > notFoundFailures.Count)
        {
            throw new ValidationException(validationFailures);
        }
        // If there are only notFound errors, notFoundValidationException is thrown.
        if (notFoundFailures.Any())
        {
            throw new NotFoundValidationException(notFoundFailures);
        }
        
        return await next();
    }
}