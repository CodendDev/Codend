using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using FluentResults;
using NameNullOrEmpty = Codend.Domain.Core.Errors.DomainErrors.ProjectTaskName.NameNullOrEmpty;
using NameTooLong = Codend.Domain.Core.Errors.DomainErrors.ProjectTaskName.NameTooLong;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// ProjectTask name value object
/// </summary>
public sealed class ProjectTaskName : StringValueObject
{
    /// <summary>
    /// Maximum name length.
    /// </summary>
    public const int MaxLength = 150;

    private ProjectTaskName(string value): base(value)
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
            .Ensure<ProjectTaskName, NameNullOrEmpty>(() => !string.IsNullOrEmpty(value))
            .Ensure<ProjectTaskName, NameTooLong>(() => value.Length < MaxLength);
    }
}