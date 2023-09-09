namespace Codend.Shared.ShouldUpdate;

public interface IShouldUpdate
{
    public bool ShouldUpdate { get; init; }
}

public interface IShouldUpdate<out T> : IShouldUpdate
{
    public T? Value { get; }
}