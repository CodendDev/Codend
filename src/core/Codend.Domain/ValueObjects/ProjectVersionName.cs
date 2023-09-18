using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.StringValueObject;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// [Optional] Project version name value object.
/// </summary>
public sealed class ProjectVersionName : NullableStringValueObject, INullableStringValueObject<ProjectVersionName>
{
    /// <summary>
    /// Maximum name length.
    /// </summary>
    public static int MaxLength => 50;

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
            .Ensure(() => value is null || value.Length < MaxLength,
                new TooLong(nameof(ProjectVersionName), MaxLength));
    }
}