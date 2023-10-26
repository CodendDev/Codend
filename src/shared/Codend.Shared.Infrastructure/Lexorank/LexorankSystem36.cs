namespace Codend.Shared.Infrastructure.Lexorank;

public class LexorankSystem36 : ILexorankSystem
{
    private readonly char[] _alphabet = "0123456789abcdefghijklmnopqrstuvwxyz".ToCharArray();

    public LexorankSystem36()
    {
    }

    public int GetBase() => 36;
    public char[] GetAlphabet() => _alphabet;
    public char GetMinChar() => '0';
    public char GetMidChar() => 'i';
    public char GetMaxChar() => 'z';
    public char StartOfAlphabet() => '/';
    public char EndOfAlphabet() => '|';

    public int DiffBetweenChars(char ch1, char ch2) => ToDigit(ch2) - ToDigit(ch1);

    /// <inheritdoc />
    public int ToDigit(char ch)
    {
        if (ch == StartOfAlphabet()) ch = GetMinChar();
        if (ch == EndOfAlphabet()) ch = GetMaxChar();
        return ch switch
        {
            >= '0' and <= '9' => ch - 48,
            >= 'a' and <= 'z' => ch - 97 + 10,
            _ => throw new LexorankException("Not valid digit: " + ch)
        };
    }

    /// <inheritdoc />
    public char ToChar(int digit)
    {
        if (digit is < 0 or >= 36)
        {
            throw new LexorankException($"Alphabet digit out of range: digit:{digit}, range: 0-{GetBase()}");
        }

        return _alphabet[digit];
    }
}