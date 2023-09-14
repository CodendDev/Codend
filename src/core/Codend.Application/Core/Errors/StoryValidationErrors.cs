using Codend.Domain.Core.Enums;

namespace Codend.Application.Core.Errors;

/// <summary>
/// Validation error static class.
/// </summary>
public static partial class ValidationErrors
{
    /// <summary>
    /// Story validation error static class.
    /// </summary>
    public static class Story
    {
        /// <inheritdoc />
        public class NotFoundOrUserUnauthorized : ValidationError
        {
            /// <inheritdoc />
            public NotFoundOrUserUnauthorized() : base("Story.NotFoundOrUserUnauthorized",
                $"Story with given id not found or user is unauthorized.")
            {
            }
        }
    }
}