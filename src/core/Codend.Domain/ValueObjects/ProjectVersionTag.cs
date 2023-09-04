using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using FluentResults;
using NullOrEmpty = Codend.Domain.Core.Errors.DomainErrors.ProjectVersionTag.NullOrEmpty;
using TagTooLong = Codend.Domain.Core.Errors.DomainErrors.ProjectVersionTag.TagTooLong;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// Project version tag value object.
/// </summary>
public sealed class ProjectVersionTag : StringValueObject
{
    /// <summary>
    /// Maximum description length.
    /// </summary>
    public const int MaxLength = 20;

    private ProjectVersionTag(string value) : base(value)
    {
    }

    /// <summary>
    /// Creates <see cref="ProjectVersionTag"/> instance.
    /// </summary>
    /// <param name="value">Tag value.</param>
    /// <returns>The <see cref="Result"/> of creation. Contains <see cref="ProjectVersionTag"/> or an <see cref="DomainErrors.DomainError"/>.</returns>
    public static Result<ProjectVersionTag> Create(string value)
    {
        return Result
            .Ok(new ProjectVersionTag(value))
            .Ensure<ProjectVersionTag, NullOrEmpty>(() => !string.IsNullOrEmpty(value))
            .Ensure<ProjectVersionTag, TagTooLong>(() => value.Length < MaxLength);
    }
}