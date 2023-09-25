using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Querying;
using Codend.Domain.Core.Enums;
using Codend.Domain.Core.Errors;

namespace Codend.Application.Core.Errors;

/// <summary>
/// Validation error static class.
/// </summary>
internal static class ValidationErrors
{
    /// <summary>
    /// Validation error base class.
    /// </summary>
    internal abstract class ValidationError : ApiError
    {
        /// <inheritdoc />
        protected ValidationError(string errorCode, string message) : base(errorCode, message)
        {
        }
    }

    /// <summary>
    /// Common, frequently used validation errors.
    /// </summary>
    internal static class Common
    {
        /// <inheritdoc />
        internal class PropertyNullOrEmpty : ValidationError
        {
            /// <inheritdoc />
            public PropertyNullOrEmpty(string stringName) : base("Validation.Common.StringPropertyNullOrEmpty",
                $"Field {stringName} cannot be null or empty.")
            {
            }
        }

        /// <inheritdoc />
        internal class StringPropertyTooLong : ValidationError
        {
            /// <inheritdoc />
            public StringPropertyTooLong(string stringName, int maxLength) : base(
                "Validation.Common.StringPropertyTooLong",
                $"Field {stringName} is longer than maximum allowed length {maxLength}.")
            {
            }
        }

        /// <inheritdoc />
        internal class DateIsInThePast : ValidationError
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
        internal class PriorityNotDefined : ValidationError
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
        internal class NotValid : ValidationError
        {
            /// <inheritdoc />
            public NotValid() : base("Validation.EmailAddress.NotValid",
                "Provided email is not valid email address.")
            {
            }
        }

        /// <inheritdoc />
        internal class TooLong : ValidationError
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
        internal class TooLong : ValidationError
        {
            /// <inheritdoc />
            public TooLong() : base("Validation.Password.TooLong",
                $"Provided password is longer than allowed {IAuthService.MaxPasswordLength}.")
            {
            }
        }

        /// <inheritdoc />
        internal class TooShort : ValidationError
        {
            /// <inheritdoc />
            public TooShort() : base("Validation.Password.TooShort",
                $"Provided password is shorter than allowed {IAuthService.MinPasswordLength}.")
            {
            }
        }

        /// <inheritdoc />
        internal class NotContainLowercaseLetter : ValidationError
        {
            /// <inheritdoc />
            public NotContainLowercaseLetter() : base("Validation.Password.NotContainLowercaseLetter",
                "Password must contain at least 1 lowercase letter.")
            {
            }
        }

        /// <inheritdoc />
        internal class NotContainUppercaseLetter : ValidationError
        {
            /// <inheritdoc />
            public NotContainUppercaseLetter() : base("Validation.Password.NotContainUppercaseLetter",
                "Password must contain at least 1 uppercase letter.")
            {
            }
        }

        /// <inheritdoc />
        internal class NotContainDigit : ValidationError
        {
            /// <inheritdoc />
            public NotContainDigit() : base("Validation.Password.NotContainDigit",
                "Password must contain at least 1 digit.")
            {
            }
        }

        /// <inheritdoc />
        internal class NotContainCustomChar : ValidationError
        {
            /// <inheritdoc />
            public NotContainCustomChar() : base("Validation.Password.NotContainCustomChar",
                "Password must contain at least 1 of this custom chars: @$#!%*?&.")
            {
            }
        }
    }

    /// <summary>
    /// Query validation error static class.
    /// </summary>
    public static class Querying
    {
        /// <inheritdoc />
        internal class InvalidPageSize : ValidationError
        {
            /// <inheritdoc />
            public InvalidPageSize() : base("Validation.Querying.InvalidPageSize",
                $"Page size must be in range {IPageableQuery.MinPageSize}-{IPageableQuery.MaxPageSize}.")
            {
            }
        }

        /// <inheritdoc />
        internal class InvalidPageIndex : ValidationError
        {
            /// <inheritdoc />
            public InvalidPageIndex() : base("Validation.Querying.InvalidPageIndex",
                $"Page index must be greater than {IPageableQuery.MinPageIndex}.")
            {
            }
        }

        /// <inheritdoc />
        internal class InvalidSortOrder : ValidationError
        {
            /// <inheritdoc />
            public InvalidSortOrder() : base("Validation.Querying.InvalidSortOrder",
                "Sort order must be either 'asc' for ascending or 'desc' for descending.")
            {
            }
        }

        /// <inheritdoc />
        internal class NotSupportedOrderColumnSelector : ValidationError
        {
            /// <inheritdoc />
            public NotSupportedOrderColumnSelector(IEnumerable<string> supportedSelectors) :
                base("Validation.Querying.NotSupportedOrderColumnSelector",
                    $"Sort column must be one of supported: '{string.Join(", ", supportedSelectors)}'.")
            {
            }
        }
    }
}