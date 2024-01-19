using Codend.Domain.Core.Errors;
using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// [Optional] Project version changelog value object.
/// </summary>
public sealed class ProjectVersionChangelog
    : NullableStringValueObject<ProjectVersionChangelog>, IStringMaxLengthValueObject
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
    public static Result<ProjectVersionChangelog> Create(string? value) => Validate(new ProjectVersionChangelog(value));
}