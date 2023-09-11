using Codend.Shared;

namespace Codend.Application.Core;

/// <summary>
/// <see cref="ShouldUpdateProperty{T}"/> helper for easier creation.
/// </summary>
public static class ShouldUpdateProperty
{
    public static ShouldUpdateProperty<T> DontUpdate<T>()
    {
        return new ShouldUpdateProperty<T>(false, default);
    }

    public static ShouldUpdateProperty<T> Update<T>(T value)
    {
        return new ShouldUpdateProperty<T>(true, value);
    }
}

/// <summary>
/// <see cref="IShouldUpdate{T}"/> implementation used for updating domain aggregates.
/// </summary>
public class ShouldUpdateProperty<T> : IShouldUpdate<T>
{
    private readonly T? _value;

    public T? Value =>
        ShouldUpdate
            ? _value
            : throw new InvalidOperationException("You can't get value of property which should not be updated");

    public bool ShouldUpdate { get; }

    internal ShouldUpdateProperty(bool update, T? value)
    {
        ShouldUpdate = update;
        _value = value;
    }
}