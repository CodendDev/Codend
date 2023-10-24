namespace Codend.Infrastructure.Lexorank;

public class LexorankString : IComparable<LexorankString>, IComparable, IEquatable<LexorankString>
{
    public string Value { get; }

    private LexorankString(string value) => Value = value;

    /// <summary>
    /// Call to get new instance of <see cref="LexorankString"/> with <paramref name="value"/> as Value.
    /// </summary>
    /// <param name="value">LexorankString Value.</param>
    /// <returns>New instance of <see cref="LexorankString"/>.</returns>
    public static LexorankString FromString(string value) => new LexorankString(value);

    /// <inheritdoc />
    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (ReferenceEquals(this, obj)) return 0;
        return obj is LexorankString other
            ? CompareTo(other)
            : throw new ArgumentException($"Object must be of type {nameof(LexorankString)}");
    }

    /// <inheritdoc />
    public int CompareTo(LexorankString? other)
    {
        if (other is null) return 1;
        if (ReferenceEquals(this, other) || Equals(other)) return 0;
        return string.CompareOrdinal(Value, other.Value);
    }

    /// <inheritdoc />
    public bool Equals(LexorankString? other)
    {
        return other is not null && Value.Equals(other.Value);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}