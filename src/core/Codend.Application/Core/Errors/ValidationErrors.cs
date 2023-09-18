using Codend.Application.Core.Abstractions.Authentication;
using Codend.Domain.Core.Enums;
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
            public PropertyNullOrEmpty(string stringName) : base("Validation.Common.StringPropertyNullOrEmpty",
                $"Field {stringName} cannot be null or empty.")
            {
            }
        }

        /// <inheritdoc />
        public class StringPropertyTooLong : ValidationError
        {
            /// <inheritdoc />
            public StringPropertyTooLong(string stringName, int maxLength) : base(
                "Validation.Common.StringPropertyTooLong",
                $"Field {stringName} is longer than maximum allowed length {maxLength}.")
            {
            }
        }

        /// <inheritdoc />
        public class DateIsInThePast : ValidationError
        {
            /// <inheritdoc />
            public DateIsInThePast(string fieldName) : base("Validation.Common.DateIsInThePast",
                $"Given date field {fieldName} is in the past.")
            {
            }
        }
    }

    /// <summary>
    /// Project task validation error static class.
    /// </summary>
    public static class ProjectTask
    {
        /// <inheritdoc />
        public class PriorityNotDefined : ValidationError
        {
            /// <inheritdoc />
            public PriorityNotDefined() : base("Validation.ProjectTask.PriorityNotDefined",
                $"Given priority is not defined. Valid priorities: '{string.Join(", ", ProjectTaskPriority.DefaultList())}'.")
            {
            }
        }
    }

    /// <summary>
    /// Email address validation error static class.
    /// </summary>
    public static class EmailAddress
    {
        /// <inheritdoc />
        public class NotValid : ValidationError
        {
            /// <inheritdoc />
            public NotValid() : base("Validation.EmailAddress.NotValid",
                "Provided email is not valid email address.")
            {
            }
        }

        /// <inheritdoc />
        public class TooLong : ValidationError
        {
            /// <inheritdoc />
            public TooLong() : base("Validation.EmailAddress.TooLong",
                $"Provided email is longer than allowed {IAuthService.MaxEmailLength}.")
            {
            }
        }
    }

    /// <summary>
    /// Password validation error static class.
    /// </summary>
    public static class Password
    {
        /// <inheritdoc />
        public class TooLong : ValidationError
        {
            /// <inheritdoc />
            public TooLong() : base("Validation.Password.TooLong",
                $"Provided password is longer than allowed {IAuthService.MaxPasswordLength}.")
            {
            }
        }

        /// <inheritdoc />
        public class TooShort : ValidationError
        {
            /// <inheritdoc />
            public TooShort() : base("Validation.Password.TooShort",
                $"Provided password is shorter than allowed {IAuthService.MinPasswordLength}.")
            {
            }
        }
        
        /// <inheritdoc />
        public class NotContainLowercaseLetter : ValidationError
        {
            /// <inheritdoc />
            public NotContainLowercaseLetter() : base("Validation.Password.NotContainLowercaseLetter",
                "Password must contain at least 1 lowercase letter.")
            {
            }
        }
        
        /// <inheritdoc />
        public class NotContainUppercaseLetter : ValidationError
        {
            /// <inheritdoc />
            public NotContainUppercaseLetter() : base("Validation.Password.NotContainUppercaseLetter",
                "Password must contain at least 1 uppercase letter.")
            {
            }
        }
        
        /// <inheritdoc />
        public class NotContainDigit : ValidationError
        {
            /// <inheritdoc />
            public NotContainDigit() : base("Validation.Password.NotContainDigit",
                "Password must contain at least 1 digit.")
            {
            }
        }
        
        /// <inheritdoc />
        public class NotContainCustomChar : ValidationError
        {
            /// <inheritdoc />
            public NotContainCustomChar() : base("Validation.Password.NotContainCustomChar",
                "Password must contain at least 1 of this custom chars: @$#!%*?&.")
            {
            }
        }
    }
}