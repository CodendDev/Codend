using Codend.Domain.Core.Errors;
using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// ProjectTask name value object
/// </summary>
public sealed class ProjectTaskName : StringValueObject<ProjectTaskName>, IStringMaxLengthValueObject
{
    /// <summary>
    /// Maximum name length.
    /// </summary>
    public static int MaxLength => 150;

    private ProjectTaskName(string value) : base(value)
    {
    }

    /// <summary>
    /// Creates new <see cref="ProjectTaskName"/> value object with given <paramref name="value"/> string.
    /// Additionally checks whether the maximum length is exceeded or string value is not null or empty.
    /// </summary>
    /// <param name="value">Name for the ProjectTask.</param>
    /// <returns>The <see cref="Result"/> of creation. Contains <see cref="ProjectTaskName"/> or an <see cref="DomainErrors.DomainError"/>.</returns>
    public static Result<ProjectTaskName> Create(string value) => Validate(new ProjectTaskName(value));
}