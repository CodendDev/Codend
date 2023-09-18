using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using FluentResults;
using static Codend.Domain.Core.Errors.DomainErrors.StringValueObject;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// Project name value object.
/// </summary>
public sealed class ProjectName : StringValueObject, IStringValueObject<ProjectName>
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
    public static Result<ProjectName> Create(string value)
    {
        return Result
            .Ok(new ProjectName(value))
            .Ensure(() => !string.IsNullOrEmpty(value), new NullOrEmpty<ProjectName>())
            .Ensure(() => value.Length < MaxLength, new TooLong<ProjectName>());
    }
}