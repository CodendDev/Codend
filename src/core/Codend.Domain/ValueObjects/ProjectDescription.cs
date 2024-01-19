using Codend.Domain.Core.Errors;
using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// [Optional] Project description value object.
/// </summary>
public sealed class ProjectDescription :
    NullableStringValueObject<ProjectDescription>, IStringMaxLengthValueObject
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
    public static Result<ProjectDescription> Create(string? value) => Validate(new ProjectDescription(value));
}