using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using Codend.Domain.Core.Primitives;
using FluentResults;
using DescriptionTooLong = Codend.Domain.Core.Errors.DomainErrors.ProjectTaskDescription.DescriptionTooLong;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// ProjectTask description value object.
/// </summary>
public sealed class ProjectTaskDescription : ValueObject
{
    /// <summary>
    /// Maximum description length.
    /// </summary>
    public const int MaxLength = 2000;

    /// <summary>
    /// Description value.
    /// </summary>
    public string Description { get; }

    private ProjectTaskDescription(string description)
    {
        Description = description;
    }

    /// <summary>
    /// Creates new <see cref="ProjectTaskDescription"/> value object with given <paramref name="description"/> string.
    /// Additionally checks whether the maximum length is exceeded.
    /// </summary>
    /// <param name="description">Description for the new <see cref="ProjectTaskDescription"/>.</param>
    /// <returns>The <see cref="Result"/> of creation. Contains <see cref="ProjectTaskDescription"/> or an <see cref="DomainErrors.DomainError"/>.</returns>
    public static Result<ProjectTaskDescription> Create(string description)
    {
        return Result
            .Ok(new ProjectTaskDescription(description))
            .Ensure<ProjectTaskDescription, DescriptionTooLong>(() => description.Length < MaxLength);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Description;
    }
}