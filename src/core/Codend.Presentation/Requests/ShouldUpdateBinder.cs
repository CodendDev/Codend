using Codend.Shared;

namespace Codend.Presentation.Requests;

public class ShouldUpdateBinder<T> : IShouldUpdate<T>
{
    public ShouldUpdateBinder(bool shouldUpdate)
    {
        ShouldUpdate = shouldUpdate;
    }

    public bool ShouldUpdate { get; set; }
    public T? Value { get; set; }

    public ShouldUpdateBinder()
    {
    }

    public ShouldUpdateBinder(bool shouldUpdate, T? value)
    {
        ShouldUpdate = shouldUpdate;
        Value = value;
    }
}