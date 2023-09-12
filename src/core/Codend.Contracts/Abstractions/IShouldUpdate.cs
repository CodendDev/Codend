namespace Codend.Contracts.Abstractions;

/// <summary>
/// Interface used for updating entities' properties.
/// Allows updating properties which are nullable.
/// </summary>
public interface IShouldUpdate<out T>
{
    public T? Value { get; }
    public bool ShouldUpdate { get; }
}