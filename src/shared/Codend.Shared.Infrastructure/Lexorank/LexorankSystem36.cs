namespace Codend.Shared.Infrastructure.Lexorank;

public class LexorankSystem36 : ILexorankSystem
{
    private readonly char[] _alphabet = "0123456789abcdefghijklmnopqrstuvwxyz".ToCharArray();

    public LexorankSystem36()
    {
    }

    public int Base => 36;

    public char[] Alphabet => _alphabet;

    public char MinChar => '0';

    public char MidChar => 'i';

    public char MaxChar => 'z';

    public char StartOfAlphabet => '/';

    public char EndOfAlphabet => '|';

    /// <inheritdoc />
    public int DistanceBetweenCharacters(char ch1, char ch2) => ToDigit(ch2) - ToDigit(ch1);

    /// <inheritdoc />
    public int ToDigit(char ch)
    {
        if (ch == StartOfAlphabet) ch = MinChar;
        if (ch == EndOfAlphabet) ch = MaxChar;
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
            throw new LexorankException($"Alphabet digit out of range: digit:{digit}, range: 0-{Base}");
        }

        return _alphabet[digit];
    }
}