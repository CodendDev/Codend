namespace Codend.Shared.ShouldUpdate;

public class ShouldUpdateProperty<T> : IShouldUpdate<T>
{
    private T? _value;

    public T? Value
    {
        get => ShouldUpdate ? _value : default;
        set => _value = value;
    }

    public bool ShouldUpdate { get; init; }

    public ShouldUpdateProperty()
    {
    }

    public ShouldUpdateProperty(bool update)
    {
        ShouldUpdate = update;
    }
}