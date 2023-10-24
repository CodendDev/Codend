namespace Codend.Infrastructure.Lexorank;

public class LexorankException : Exception
{
    public LexorankException(string message) : base(message)
    {
    }
    
    public LexorankException(string message, Exception innerException) : base(message, innerException)
    {
    }
}