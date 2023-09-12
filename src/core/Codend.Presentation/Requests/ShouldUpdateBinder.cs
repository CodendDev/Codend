using Codend.Contracts.Abstractions;

namespace Codend.Presentation.Requests;

/// <summary>
/// <see cref="IShouldUpdate{T}"/> implementation used for binding properties in request.
/// </summary>
public record ShouldUpdateBinder<T>
(
    bool ShouldUpdate,
    T? Value
) : IShouldUpdate<T>;