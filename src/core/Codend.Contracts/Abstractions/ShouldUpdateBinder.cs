namespace Codend.Contracts.Abstractions;

/// <summary>
/// <see cref="IShouldUpdate{T}"/> implementation used for binding properties in request.
/// </summary>
public record ShouldUpdateBinder<T>
(
    bool ShouldUpdate,
    T? Value
) : IShouldUpdate<T>
{
    /// <summary>
    /// Converts class wrapped with <see cref="ShouldUpdateBinder{T}"/> to another class.
    /// </summary>
    /// <param name="conversion">Conversion from <see cref="TIn"/> to <see cref="TOut"/>/</param>
    /// <returns><see cref="ShouldUpdateBinder{T}"/> with <see cref="TOut"/> class wrapped.</returns>
    public ShouldUpdateBinder<TOut> HasConversion<TOut>(Func<T?, TOut> conversion)
    {
        return new ShouldUpdateBinder<TOut>(ShouldUpdate, conversion(Value));
    }
}

/// <summary>
/// <see cref="ShouldUpdateBinder{T}"/> extensions
/// </summary>
public static class ShouldUpdateBinderExtensions
{
    /// <summary>
    /// Converts <see cref="ShouldUpdateBinder{T}"/> from null to ShouldUpdate = false.
    /// </summary>
    /// <typeparam name="T">Class wrapped with <see cref="ShouldUpdateBinder{T}"/></typeparam>
    /// <returns><see cref="ShouldUpdateBinder{T}"/> with correct ShouldUpdate.</returns>
    public static ShouldUpdateBinder<T> HandleNull<T>(this ShouldUpdateBinder<T>? binder)
    {
        if (binder is null)
        {
            return new ShouldUpdateBinder<T>(false, default);
        }

        return binder;
    }
}