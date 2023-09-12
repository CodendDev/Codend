using Codend.Domain.Core.Errors;

namespace Codend.Application.Core.Errors;

/// <summary>
/// Validation error static class.
/// </summary>
public static class ValidationErrors
{
    /// <summary>
    /// Validation error base class.
    /// </summary>
    public class ValidationError : ApiError
    {
        /// <inheritdoc />
        protected ValidationError(string errorCode, string message) : base(errorCode, message)
        {
        }
    }

    public static class Common
    {
        public class StringPropertyNullOrEmpty : ValidationError
        {
            public StringPropertyNullOrEmpty(string stringName) : base("Common.StringPropertyNullOrEmpty",
                $"Field {stringName} cannot be null or empty.")
            {
            }
        }

        public class StringPropertyTooLong : ValidationError
        {
            public StringPropertyTooLong(string stringName, int maxLength) : base("Common.StringPropertyTooLong",
                $"Field {stringName} is longer than maximum {maxLength}")
            {
            }
        }
    }
}