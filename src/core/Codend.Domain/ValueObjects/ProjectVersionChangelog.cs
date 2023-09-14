using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.StringValueObject;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// [Optional] Project version changelog value object.
/// </summary>
public sealed class ProjectVersionChangelog : NullableStringValueObject,
    INullableStringValueObject<ProjectVersionChangelog>
{
    /// <summary>
    /// Maximum description length.
    /// </summary>
    public static int MaxLength => 3000;

    private ProjectVersionChangelog(string? value) : base(value)
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
            .Ensure(() => value is null || value.Length < MaxLength,
                new TooLong(nameof(ProjectVersionChangelog), MaxLength));
    }
}