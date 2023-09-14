using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.StringValueObject;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// [Optional] Project description value object.
/// </summary>
public sealed class ProjectDescription : NullableStringValueObject, INullableStringValueObject<ProjectDescription>
{
    /// <summary>
    /// Maximum description length.
    /// </summary>
    public static int MaxLength => 2000;

    private ProjectDescription(string? value) : base(value)
    {
    }

    /// <summary>
    /// Creates <see cref="ProjectDescription"/> instance.
    /// </summary>
    /// <param name="value">Description value.</param>
    /// <returns>The <see cref="Result"/> of creation. Contains <see cref="ProjectDescription"/> or an <see cref="DomainErrors.DomainError"/>.</returns>
    public static Result<ProjectDescription> Create(string? value)
    {
        return Result
            .Ok(new ProjectDescription(value))
            .Ensure(() => value is null || value.Length < MaxLength,
                new TooLong(nameof(ProjectDescription), MaxLength));
    }
}