namespace Codend.Infrastructure.Lexorank;

public class Lexorank : IComparable<Lexorank>, IComparable, IEquatable<Lexorank>
{
    private static readonly ILexorankSystem System = new LexorankSystem36();
    private static readonly LexorankString MinStringValue = LexorankString.FromString("000000");
    private static readonly LexorankString MidStringValue = LexorankString.FromString("iiiiii");
    private static readonly LexorankString MaxStringValue = LexorankString.FromString("zzzzzz");

    public string Value { get; }

    public LexorankBucket Bucket { get; }

    public LexorankString RankString { get; }

    private Lexorank(string value)
    {
        Value = value;
        var lexorankParts = Value.Split('|');
        if (lexorankParts.Length != 2)
            throw new LexorankException($"Invalid string: {value}. It must look like '0|abcd1'");
        Bucket = LexorankBucket.FromString(lexorankParts[0]);
        RankString = LexorankString.FromString(lexorankParts[1]);
    }


    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (ReferenceEquals(this, obj)) return 0;
        return obj is Lexorank other
            ? CompareTo(other)
            : throw new ArgumentException($"Object must be of type {nameof(Lexorank)}");
    }

    public int CompareTo(Lexorank? other)
    {
        if (other is null) return 1;
        if (ReferenceEquals(this, other) || Equals(other)) return 1;
        return string.CompareOrdinal(Value, other.Value);
    }

    public bool Equals(Lexorank? other)
    {
        return other is not null && Value.Equals(other.Value);
    }
}