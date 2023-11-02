namespace Codend.Shared.Infrastructure.Lexorank;

public interface ILexorankSystem
{
    /// <summary>
    /// Represents the System ordered alphabet.
    /// </summary>
    /// <value>Sorted char list containing all alphabet members.</value>
    public char[] Alphabet { get; }

    /// <summary>
    /// Number of chars in alphabet.
    /// </summary>
    public int Base { get; }

    /// <summary>
    /// First element of the alphabet.
    /// </summary>
    public char MinChar { get; }

    /// <summary>
    /// Middle element of the alphabet.
    /// </summary>
    public char MidChar { get; }

    /// <summary>
    /// Last element of the alphabet.
    /// </summary>
    public char MaxChar { get; }

    /// <summary>
    /// Char not included in alphabet, which will 'mark' the start of the alphabet.
    /// </summary>
    public char StartOfAlphabet { get; }

    /// <summary>
    /// Char not included in alphabet, which will 'mark' the end of the alphabet.
    /// </summary>
    public char EndOfAlphabet { get; }

    /// <summary>
    /// Checks the distance between <paramref name="ch1"/> and <paramref name="ch2"/> in the alphabet.
    /// </summary>
    /// <param name="ch1">'From' char position.</param>
    /// <param name="ch2">'To' char position.</param>
    /// <remarks>
    /// Less than 0 - ch1 is this many places after ch2 |
    /// 0 - it's the same char |
    /// More than 0 - ch1 is this many places before ch1.
    /// </remarks>
    /// <returns><see cref="int"/> number of indexes splitting char 1 from char 2.</returns>
    public int DiffBetweenChars(char ch1, char ch2);

    /// <summary>
    /// Changes 'char' int(ascii) value to its alphabet numeric value, which is its position in alphabet char list.
    /// </summary>
    /// <returns>Int number being char position in alphabet char array.</returns>
    public int ToDigit(char ch);

    /// <summary>
    /// Converts int digit to its alphabet char representation.
    /// </summary>
    /// <returns>Char from alphabet on position with index equal to <paramref name="digit"/></returns>
    public char ToChar(int digit);
}