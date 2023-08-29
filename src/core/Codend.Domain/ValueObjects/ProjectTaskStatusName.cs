using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using Codend.Domain.Core.Primitives;
using FluentResults;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// ProjectTaskStatus name.
/// </summary>
public sealed class ProjectTaskStatusName : ValueObject
{
    /// <summary>
    /// Maximum ProjectTaskStatus lenghth.
    /// </summary>
    public const int MaxLength = 100;

    /// <summary>
    /// ProjetTaskStatus value.
    /// </summary>
    public string Name { get; }

    private ProjectTaskStatusName(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Creates new ProjectTaskStatusName value object with given <paramref name="name"/> string.
    /// Additionaly it checkes whether the maximum length is not exceeded and string value is not null/empty.
    /// </summary>
    /// <param name="name">Name for the ProjectTaskStatus.</param>
    /// <returns>Result of the create operation. Returns ProjestTaskStatusName or an error.</returns>
    public static Result<ProjectTaskStatusName> Create(string name)
    {
        return Result.Ok(new ProjectTaskStatusName(name))
            .Ensure(() => !string.IsNullOrEmpty(name), new DomainErrors.ProjectTaskStatus.NameNullOrEmpty())
            .Ensure(() => name.Length < MaxLength, new DomainErrors.ProjectTaskStatus.NameTooLong());
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Name;
    }
}