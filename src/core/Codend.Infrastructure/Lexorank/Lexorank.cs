namespace Codend.Infrastructure.Lexorank;

public class Lexorank : IComparable<Lexorank>, IComparable, IEquatable<Lexorank>
{
    protected static readonly ILexorankSystem LexorankSystem = new LexorankSystem36();

    public string Value { get; }

    private Lexorank(string value) => Value = value;

    public static Lexorank FromString(string value) => new(value);

    /// <summary>
    /// Calculates and returns middle position between two ranks.
    /// </summary>
    /// <param name="prev">First position.</param>
    /// <param name="next">Second position.</param>
    /// <returns><see cref="Lexorank"/> instance that is between two given lexoranks.</returns>
    public static Lexorank GetMiddle(Lexorank? prev = null, Lexorank? next = null)
    {
        // Handle edge cases.
        if (prev is null && next is null) return new Lexorank(LexorankSystem.GetMidChar().ToString());
        if (prev is null) prev = new Lexorank(LexorankSystem.GetMinChar().ToString());
        else if (next is null) next = new Lexorank(LexorankSystem.GetMaxChar().ToString());

        var cmp = prev.CompareTo(next);

        // Check if positions are the same.
        if (cmp == 0) throw new LexorankException($"Prev and next parameters cannot be same position! Pos: {prev}");

        // Check if positions are in proper order and correct if necessary.
        if (cmp > 0) (prev, next) = (next, prev);

        return new Lexorank(CalculateMiddle(prev!.Value, next!.Value, LexorankSystem));
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

    private static string CalculateMiddle(string prevString, string nextString, ILexorankSystem lexorankSystem)
    {
        char prevChar = '_', nextChar = '_';
        int currPos;
        string resultStr;

        var startChar = lexorankSystem.StartOfAlphabet();
        var endChar = lexorankSystem.EndOfAlphabet();

        // Find leftmost non-matching character, or non-alphabetic char if string ended.
        for (currPos = 0; prevChar == nextChar; currPos++)
        {
            prevChar = currPos < prevString.Length ? prevString[currPos] : startChar;
            nextChar = currPos < nextString.Length ? nextString[currPos] : endChar;
        }

        resultStr = prevString.Substring(0, currPos - 1); // Copy identical part of strings.
        var minChar = lexorankSystem.GetMinChar();
        var maxChar = lexorankSystem.GetMaxChar();

        if (prevChar == startChar) // When prev string is shorter, equalize length with minChar eg. '0' or 'a'.
        {
            while (nextChar == minChar) // Equalize with minChar.
            {
                nextChar = currPos < nextString.Length ? nextString[currPos++] : endChar;
                resultStr += lexorankSystem.ToChar(minChar);
            }

            if (nextChar == lexorankSystem.ToChar(1)) // In case of 'second minChar ('b' or '1')', insert minChar.
            {
                resultStr += minChar;
                nextChar = endChar;
            }
        }
        else if (lexorankSystem.DiffBetweenChars(prevChar, nextChar) == 1) // found consecutive chars eg. 'c' && 'd'
        {
            resultStr += prevChar;
            nextChar = endChar;
            while ((prevChar = currPos < prevString.Length ? prevString[currPos++] : minChar) == maxChar)
            {
                // Equalize maxChars if needed.
                resultStr += maxChar;
            }
        }

        int prevDigit = lexorankSystem.ToDigit(prevChar), nextDigit = lexorankSystem.ToDigit(nextChar);
        return resultStr + lexorankSystem.ToChar((int)Math.Ceiling((prevDigit + nextDigit) / 2D));
    }
}