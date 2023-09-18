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

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationBehavior{TRequest, TResponse}"/> class.
    /// </summary>
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
        
        if (validationFailures.Any())
        {
            throw new ValidationException(validationFailures);
        }
        
        return await next();
    }
}