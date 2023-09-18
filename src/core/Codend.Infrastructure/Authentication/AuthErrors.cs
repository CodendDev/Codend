using FluentResults;

namespace Codend.Infrastructure.Authentication;

/// <summary>
/// Contains the authentication errors.
/// </summary>
public static class AuthErrors
{
    /// <summary>
    /// Auth error base class.
    /// </summary>
    public abstract class AuthError : Error
    {
        public string ErrorCode { get; }

        protected AuthError(string errorCode, string message)
        {
            Metadata.Add("ErrorCode", errorCode);
            Metadata.Add("Message", message);
            ErrorCode = errorCode;
            Message = message;
        }
    }

    public static class General
    {
        public class InternalAuthError : AuthError
        {
            public InternalAuthError() : base("General.InternalAuthError", "Internal authentication service error.")
            {
            }
        }

        public class UnspecifiedAuthError : AuthError
        {
            public UnspecifiedAuthError() : base("General.UnspecifiedAuthError", "Unspecified authentication error.")
            {
            }
        }
    }
    
    public static class Login
    {
        public class InvalidEmailOrPassword : AuthError
        {
            public InvalidEmailOrPassword() : base("Login.InvalidEmailOrPassword",
                "No user exists with given email or password is incorrect.")
            {
            }
        }
    }

    public static class Register
    {
        public class EmailAlreadyExists : AuthError
        {
            public EmailAlreadyExists() : base("Register.EmailAlreadyExists", "User with given email already exists.")
            {
            }
        }
        
        public class PasswordNotValid : AuthError
        {
            public PasswordNotValid() : base("Register.PasswordNotValid", "Provided password is not valid.")
            {
            }
        }
    }
}