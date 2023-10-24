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