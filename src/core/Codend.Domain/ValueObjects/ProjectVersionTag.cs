using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using Codend.Domain.Core.Primitives;
using FluentResults;
using NullOrEmpty = Codend.Domain.Core.Errors.DomainErrors.ProjectVersionTag.NullOrEmpty;
using TagTooLong = Codend.Domain.Core.Errors.DomainErrors.ProjectVersionTag.TagTooLong;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// Project version tag value object.
/// </summary>
public sealed class ProjectVersionTag : ValueObject
{
    /// <summary>
    /// Maximum description length.
    /// </summary>
    public const int MaxLength = 20;

    /// <summary>
    /// Version tag value.
    /// </summary>
    public string Tag { get; }

    private ProjectVersionTag(string tag)
    {
        Tag = tag;
    }

    /// <summary>
    /// Creates <see cref="ProjectVersionTag"/> instance.
    /// </summary>
    /// <param name="tag">Tag value.</param>
    /// <returns>The <see cref="Result"/> of creation. Contains <see cref="ProjectVersionTag"/> or an <see cref="DomainErrors.DomainError"/>.</returns>
    public static Result<ProjectVersionTag> Create(string tag)
    {
        return Result
            .Ok(new ProjectVersionTag(tag))
            .Ensure<ProjectVersionTag, NullOrEmpty>(() => !string.IsNullOrEmpty(tag))
            .Ensure<ProjectVersionTag, TagTooLong>(() => tag.Length < MaxLength);
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Tag;
    }
}