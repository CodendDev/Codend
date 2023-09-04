using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using FluentResults;
using NameTooLong = Codend.Domain.Core.Errors.DomainErrors.ProjectVersionName.NameTooLong;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// Project version name value object.
/// </summary>
public sealed class ProjectVersionName : NullableStringValueObject
{
    /// <summary>
    /// Maximum name length.
    /// </summary>
    public const int MaxLength = 50;

    private ProjectVersionName(string? value) : base(value)
    {
    }

    /// <summary>
    /// Creates <see cref="ProjectVersionName" /> instance.
    /// </summary>
    /// <param name="value">Project version name value.</param>
    /// <returns>The <see cref="Result"/> of creation. Contains <see cref="ProjectVersionName"/> or an <see cref="DomainErrors.DomainError"/>.</returns>
    public static Result<ProjectVersionName> Create(string? value)
    {
        return Result
            .Ok(new ProjectVersionName(value))
            .Ensure<ProjectVersionName, NameTooLong>(() => value is null || value.Length < MaxLength);
    }
}