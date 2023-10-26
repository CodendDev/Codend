namespace Codend.Infrastructure.Lexorank;

public interface ILexorankSystem
{
    public int GetBase();

    /// <summary>
    /// Represents the System ordered alphabet.
    /// </summary>
    /// <returns>Sorted char list containing all alphabet members.</returns>
    public char[] GetAlphabet();

    public char GetMinChar();

    public char GetMidChar();

    public char GetMaxChar();

    /// <summary>
    /// Char not included in alphabet, which will 'mark' the start of the alphabet.
    /// </summary>
    public char StartOfAlphabet();

    /// <summary>
    /// Char not included in alphabet, which will 'mark' the end of the alphabet.
    /// </summary>
    public char EndOfAlphabet();

    /// <summary>
    /// Checks how far apart <paramref name="ch1"/> is from <paramref name="ch2"/> in an alphabet.
    /// </summary>
    /// <param name="ch1">'From' char position.</param>
    /// <param name="ch2">'To' char position.</param>
    /// <remarks>
    /// Less than 0 - ch1 is this many places after ch2 |
    /// 0 - it's the same char |
    /// More than 1 - ch1 is this many places before ch1.
    /// </remarks>
    /// <returns><see cref="int"/> number of indexes splitting char 1 from char 2.</returns>
    public int DiffBetweenChars(char ch1, char ch2);

    /// <summary>
    /// Changes 'char' int(ascii) value to it's alphabet numeric value, which is its position in alphabet char list.
    /// </summary>
    /// <returns>Int number being char position in alphabet char array.</returns>
    public int ToDigit(char ch);

    /// <summary>
    /// Converts int digit to it's alphabet char representation.
    /// </summary>
    /// <returns></returns>
    public char ToChar(int digit);
}