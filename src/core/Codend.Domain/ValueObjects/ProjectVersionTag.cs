using Codend.Domain.Core.Errors;
using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// Project version tag value object.
/// </summary>
public sealed class ProjectVersionTag : StringValueObject<ProjectVersionTag>, IStringMaxLengthValueObject
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
    public static Result<ProjectVersionTag> Create(string value) => Validate(new ProjectVersionTag(value));
}