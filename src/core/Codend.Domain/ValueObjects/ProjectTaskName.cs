using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using Codend.Domain.Core.Primitives;
using FluentResults;
using NameNullOrEmpty = Codend.Domain.Core.Errors.DomainErrors.ProjectTaskName.NameNullOrEmpty;
using NameTooLong = Codend.Domain.Core.Errors.DomainErrors.ProjectTaskName.NameTooLong;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// ProjectTask name value object
/// </summary>
public sealed class ProjectTaskName : ValueObject
{
    /// <summary>
    /// Maximum description length.
    /// </summary>
    public const int MaxLength = 150;

    /// <summary>
    /// ProjectTaskName value.
    /// </summary>
    public string Name { get; }

    private ProjectTaskName(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Creates new <see cref="ProjectTaskName"/> value object with given <paramref name="name"/> string.
    /// Additionally checks whether the maximum length is exceeded or string value is not null or empty.
    /// </summary>
    /// <param name="name">Name for the ProjectTask.</param>
    /// <returns>The <see cref="Result"/> of creation. Contains <see cref="ProjectTaskName"/> or an <see cref="DomainErrors.DomainError"/>.</returns>
    public static Result<ProjectTaskName> Create(string name)
    {
        return Result
            .Ok(new ProjectTaskName(name))
            .Ensure<ProjectTaskName, NameNullOrEmpty>(() => !string.IsNullOrEmpty(name))
            .Ensure<ProjectTaskName, NameTooLong>(() => name.Length < MaxLength);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Name;
    }
}