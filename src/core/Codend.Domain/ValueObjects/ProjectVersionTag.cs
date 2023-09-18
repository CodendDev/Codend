using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.StringValueObject;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// Project version tag value object.
/// </summary>
public sealed class ProjectVersionTag : StringValueObject, IStringValueObject<ProjectVersionTag>
{
    /// <summary>
    /// Maximum description length.
    /// </summary>
    public static int MaxLength => 20;

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
            .Ensure(() => !string.IsNullOrEmpty(value), new NullOrEmpty(nameof(ProjectVersionTag)))
            .Ensure(() => value.Length < MaxLength, new TooLong(nameof(ProjectVersionTag), MaxLength));
    }
}