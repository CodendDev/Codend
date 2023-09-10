using Codend.Shared;

namespace Codend.Presentation.Requests;

/// <summary>
/// <see cref="IShouldUpdate{T}"/> implementation used for binding properties in request.
/// </summary>
public class ShouldUpdateBinder<T> : IShouldUpdate<T>
{
    public bool ShouldUpdate { get; set; }
    public T? Value { get; set; }
}