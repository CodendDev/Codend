using Codend.Domain.Core.Errors;

namespace Codend.Application.Core.Errors;

/// <summary>
/// Validation error static class.
/// </summary>
public static partial class ValidationErrors
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

    /// <summary>
    /// Common, frequently used validation errors.
    /// </summary>
    public static class Common
    {
        /// <inheritdoc />
        public class PropertyNullOrEmpty : ValidationError
        {
            /// <inheritdoc />
            public PropertyNullOrEmpty(string stringName) : base("Common.StringPropertyNullOrEmpty",
                $"Field {stringName} cannot be null or empty.")
            {
            }
        }

        /// <inheritdoc />
        public class StringPropertyTooLong : ValidationError
        {
            /// <inheritdoc />
            public StringPropertyTooLong(string stringName, int maxLength) : base("Common.StringPropertyTooLong",
                $"Field {stringName} is longer than maximum {maxLength}")
            {
            }
        }
        
        /// <inheritdoc />
        public class DateIsInThePast : ValidationError
        {
            /// <inheritdoc />
            public DateIsInThePast(string fieldName) : base("Common.DateIsInThePast",
                $"Given date field {fieldName} is the past.")
            {
            }
        }
    }
}