using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using Codend.Domain.Core.Primitives;
using FluentResults;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// ProjectTask name.
/// </summary>
public sealed class ProjectTaskName : ValueObject
{
    /// <summary>
    /// Maximum description lenghth.
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
    /// Creates new ProjectTaskName value object with given <paramref name="name"/> string.
    /// Additionaly it checkes whether the maximum length is not exceeded and string value is not null/empty.
    /// </summary>
    /// <param name="name">Name for the ProjectTask.</param>
    /// <returns>Result of the create operation. Returns ProjestTaskName or an error.</returns>
    public static Result<ProjectTaskName> Create(string name)
    {
        return Result.Ok(new ProjectTaskName(name))
            .Ensure(() => !string.IsNullOrEmpty(name), new DomainErrors.ProjectTaskName.NameNullOrEmpty())
            .Ensure(() => name.Length < MaxLength, new DomainErrors.ProjectTaskName.NameTooLong());
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Name;
    }
}