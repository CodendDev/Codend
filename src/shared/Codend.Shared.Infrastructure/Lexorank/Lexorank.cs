using System.Text;

namespace Codend.Shared.Infrastructure.Lexorank;

public class Lexorank : IComparable, IComparable<Lexorank>, IEquatable<Lexorank>
{
    public static readonly ILexorankSystem LexorankSystem = new LexorankSystem36();

    public string Value { get; }

    private Lexorank()
    {
    }

    public Lexorank(string value) => Value = value;

    public static Lexorank FromString(string value) => new(value);

    /// <summary>
    /// Calculates and returns middle position between two lexoranks.
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

    /// <summary>
    /// Calculates and returns list of somewhat evenly spaced values between 2 given lexoranks.
    /// </summary>
    /// <param name="amount">Amount of values to calculate.</param>
    /// <param name="from">From lexorank, first alphabet char if null.</param>
    /// <param name="to">To lexorank, last alphabet char if null.</param>
    /// <returns>List of evenly spaced <see cref="Lexorank"/> objects.</returns>
    public static List<Lexorank> GetSpacedOutValuesBetween(int amount, Lexorank? from = null, Lexorank? to = null)
    {
        var spacedValues = new List<Lexorank>();

        from ??= new Lexorank(LexorankSystem.GetMinChar().ToString());
        to ??= new Lexorank(LexorankSystem.GetMaxChar().ToString());

        var cmp = from.CompareTo(to);
        if (cmp == 0) throw new LexorankException($"From and to parameters cannot be same position! Pos: {from}");
        if (cmp > 0) (from, to) = (to, from);

        if (amount < 1) throw new LexorankException("Amount cannot be less than 1");
        if (amount == 1)
        {
            spacedValues.Add(GetMiddle(from, to));
            return spacedValues;
        }

        var (neededChars, step) = CalculateNeededCharsAndStep(amount);
        var middleLexorank = GetMiddle(from, to);
        var prevLexorank = new Lexorank(middleLexorank.Value + new string(LexorankSystem.GetMinChar(), neededChars));
        var expectedLength = prevLexorank.Value.Length;

        for (var i = 0; i < amount; i++)
        {
            prevLexorank = CalculateNextLexorank(prevLexorank, step, expectedLength);
            spacedValues.Add(prevLexorank);
        }

        return spacedValues;
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

    public override string ToString() => Value;

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
                resultStr += minChar;
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

    private static (int, int) CalculateNeededCharsAndStep(int amount)
    {
        var neededChars = 1;
        var freeSpaces = LexorankSystem.GetBase();
        var pow = 2;
        amount += 1; // To have same distance between start and end 
        while (amount > freeSpaces)
        {
            freeSpaces = (int)Math.Pow(LexorankSystem.GetBase(), pow++);
            neededChars++;
        }

        return (neededChars, freeSpaces / amount);
    }

    private static Lexorank CalculateNextLexorank(Lexorank prev, int step, int expectedLength)
    {
        var newStrBuilder = new StringBuilder(prev.Value);
        if (newStrBuilder.Length > expectedLength) // Used to avoid added midChars in borderline cases.
        {
            newStrBuilder.Remove(newStrBuilder.Length - 1, 1);
        }

        var systemBase = LexorankSystem.GetBase();

        var levelChars = new List<int>(); // List containing char values for each 'level'
        var level = 1; // Level is string deepness. 1 - last char of string, 2 - second last etc.
        var currLevelCharDigitWithStep = LexorankSystem.ToDigit(newStrBuilder[^level]) + step;
        levelChars.Add(currLevelCharDigitWithStep % systemBase);
        while (currLevelCharDigitWithStep >= systemBase) // Loop used for moving between levels (b->c) -> (abz -> ace)
        {
            level++;
            currLevelCharDigitWithStep /= systemBase;
            currLevelCharDigitWithStep = LexorankSystem.ToDigit(newStrBuilder[^level]) + currLevelCharDigitWithStep;
            levelChars.Add(currLevelCharDigitWithStep % systemBase);
        }

        for (var i = levelChars.Count - 1; i >= 0; i--) // Overwriting previous lexorank values with new ones
        {
            newStrBuilder[^(i + 1)] = LexorankSystem.ToChar(levelChars[i]);
        }

        // In borderline cases (minChar, and maxChar as last char) midChar have to be added.
        if (newStrBuilder[^1] == LexorankSystem.GetMinChar() || newStrBuilder[^1] == LexorankSystem.GetMaxChar())
        {
            newStrBuilder.Append(LexorankSystem.GetMidChar());
        }

        return new Lexorank(newStrBuilder.ToString());
    }
}