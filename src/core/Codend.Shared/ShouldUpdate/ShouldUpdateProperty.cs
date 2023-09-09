namespace Codend.Shared.ShouldUpdate;

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

public class ShouldUpdateProperty<T> : IShouldUpdate<T>
{
    private readonly T? _value;

    public T? Value => ShouldUpdate ? _value : default;

    public bool ShouldUpdate { get; }

    public ShouldUpdateProperty()
    {
    }

    internal ShouldUpdateProperty(bool update, T? value)
    {
        ShouldUpdate = update;
        _value = value;
    }
}