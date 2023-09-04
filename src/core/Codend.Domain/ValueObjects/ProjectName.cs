using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;
using NullOrEmpty = Codend.Domain.Core.Errors.DomainErrors.ProjectName.NullOrEmpty;
using NameTooLong = Codend.Domain.Core.Errors.DomainErrors.ProjectName.NameTooLong;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// Project name value object.
/// </summary>
public sealed class ProjectName : StringValueObject
{
    /// <summary>
    /// Maximum name length.
    /// </summary>
    public const int MaxLength = 100;

    private ProjectName(string value) : base(value)
    {
    }

    /// <summary>
    /// Creates <see cref="ProjectName"/> instance.
    /// </summary>
    /// <param name="value">Name value.</param>
    /// <returns>The <see cref="Result"/> of creation. Contains <see cref="ProjectName"/> or an <see cref="DomainErrors.DomainError"/>.</returns>
    public static Result<ProjectName> Create(string value)
    {
        return Result
            .Ok(new ProjectName(value))
            .Ensure<ProjectName, NullOrEmpty>(() => !string.IsNullOrEmpty(value))
            .Ensure<ProjectName, NameTooLong>(() => value.Length < MaxLength);
    }
}