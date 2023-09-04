using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using FluentResults;
using DescriptionTooLong = Codend.Domain.Core.Errors.DomainErrors.ProjectVersionChangelog.DescriptionTooLong;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// Project version changelog value object.
/// </summary>
public sealed class ProjectVersionChangelog : NullableStringValueObject
{
    /// <summary>
    /// Maximum description length.
    /// </summary>
    public const int MaxLength = 3000;

    private ProjectVersionChangelog(string? value):base(value)
    {
    }

    /// <summary>
    /// Creates <see cref="ProjectVersionChangelog" /> instance.
    /// </summary>
    /// <param name="value">Changelog value.</param>
    /// <returns>The <see cref="Result"/> of creation. Contains <see cref="ProjectVersionChangelog"/> or an <see cref="DomainErrors.DomainError"/>.</returns>
    public static Result<ProjectVersionChangelog> Create(string? value)
    {
        return Result
            .Ok(new ProjectVersionChangelog(value))
            .Ensure<ProjectVersionChangelog, DescriptionTooLong>(() => value is null || value.Length < MaxLength);
    }
}