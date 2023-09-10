namespace Codend.Shared;

public interface IShouldUpdate
{
    public bool ShouldUpdate { get; }
}

public interface IShouldUpdate<out T> : IShouldUpdate
{
    public T? Value { get; }
}