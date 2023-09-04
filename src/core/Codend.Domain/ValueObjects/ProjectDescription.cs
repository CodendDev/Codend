using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;
using DescriptionTooLong = Codend.Domain.Core.Errors.DomainErrors.ProjectDescription.DescriptionTooLong;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// [Optional] Project description value object.
/// </summary>
public sealed class ProjectDescription : NullableStringValueObject
{
    /// <summary>
    /// Maximum description length.
    /// </summary>
    public const int MaxLength = 2000;

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
            .Ensure<ProjectDescription, DescriptionTooLong>(() => value is null || value.Length < MaxLength);
    }
}