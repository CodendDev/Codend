using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using Codend.Domain.Core.Primitives;
using FluentResults;
using DescriptionTooLong = Codend.Domain.Core.Errors.DomainErrors.ProjectDescription.DescriptionTooLong;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// Project description value object.
/// </summary>
public sealed class ProjectDescription : ValueObject
{
    /// <summary>
    /// Maximum description length.
    /// </summary>
    public const int MaxLength = 2000;

    /// <summary>
    /// Project description value.
    /// </summary>
    public string Description { get; }

    private ProjectDescription(string description)
    {
        Description = description;
    }

    /// <summary>
    /// Creates <see cref="ProjectDescription"/> instance.
    /// </summary>
    /// <param name="name">Description value.</param>
    /// <returns>The <see cref="Result"/> of creation. Contains <see cref="ProjectDescription"/> or an <see cref="DomainErrors.DomainError"/>.</returns>
    public static Result<ProjectDescription> Create(string name)
    {
        return Result
            .Ok(new ProjectDescription(name))
            .Ensure<ProjectDescription, DescriptionTooLong>(() => name.Length < MaxLength);
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Description;
    }
}