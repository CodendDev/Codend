using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.StringValueObject;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// ProjectTask name value object
/// </summary>
public sealed class ProjectTaskName : StringValueObject, IStringValueObject<ProjectTaskName>
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
    public static Result<ProjectTaskName> Create(string value)
    {
        return Result
            .Ok(new ProjectTaskName(value))
            .Ensure(() => !string.IsNullOrEmpty(value), new NullOrEmpty(nameof(ProjectTaskName)))
            .Ensure(() => value.Length < MaxLength, new TooLong<ProjectTaskName>());
    }
}