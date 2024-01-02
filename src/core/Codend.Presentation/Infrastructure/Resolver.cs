using Codend.Application.Exceptions;

namespace Codend.Presentation.Infrastructure;

/// <summary>
/// Class for resolving query/commands. Can authorize and return appropriate ActionResult based on request result.
/// </summary>
/// <typeparam name="TRequest">Command/Query to be resolved.</typeparam>
internal sealed class Resolver<TRequest>
{
    private Resolver()
    {
    }

    private Resolver(TRequest request) => Request = request;

    public TRequest Request { get; private set; } = default!;

    public Resolver<TRequest> ResolverFor(TRequest request)
    {
        Request = request;
        return this;
    }

    public static Resolver<TRequest> IfRequestNotNull(object? request) =>
        request is null ? throw new InvalidRequestException() : new Resolver<TRequest>();

    public static Resolver<TRequest> For(TRequest request) => new(request);
}