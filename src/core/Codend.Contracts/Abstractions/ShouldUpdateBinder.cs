namespace Codend.Contracts.Abstractions;

/// <summary>
/// Class used for defining if properties should be updated in requests and commands.
/// </summary>
public sealed record ShouldUpdateBinder<T>
(
    bool ShouldUpdate,
    T? Value
)
{
    /// <summary>
    /// Converts class wrapped with <see cref="ShouldUpdateBinder{T}"/> to another class.
    /// </summary>
    /// <param name="conversion">Lambda expression for conversion from current wrapped class <see cref="T"/> to <see cref="TOut"/> class.</param>
    /// <returns><see cref="ShouldUpdateBinder{T}"/> with <see cref="TOut"/> class wrapped.</returns>
    public ShouldUpdateBinder<TOut> Convert<TOut>(Func<T?, TOut> conversion)
        => new ShouldUpdateBinder<TOut>(ShouldUpdate, conversion(Value));
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