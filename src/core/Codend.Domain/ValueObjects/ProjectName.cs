using Codend.Domain.Core.Errors;
using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// Project name value object.
/// </summary>
public sealed class ProjectName : StringValueObject<ProjectName>, IStringMaxLengthValueObject
{
    /// <summary>
    /// Maximum name length.
    /// </summary>
    public static int MaxLength => 100;

    private ProjectName(string value) : base(value)
    {
    }

    /// <summary>
    /// Creates <see cref="ProjectName"/> instance.
    /// </summary>
    /// <param name="value">Name value.</param>
    /// <returns>The <see cref="Result"/> of creation. Contains <see cref="ProjectName"/> or an <see cref="DomainErrors.DomainError"/>.</returns>
    public static Result<ProjectName> Create(string value) => Validate(new ProjectName(value));
}