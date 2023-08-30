using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using Codend.Domain.Core.Primitives;
using FluentResults;
using NameNullOrEmpty = Codend.Domain.Core.Errors.DomainErrors.ProjectTaskStatus.NameNullOrEmpty;
using NameTooLong = Codend.Domain.Core.Errors.DomainErrors.ProjectTaskStatus.NameTooLong;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// ProjectTaskStatus name value object.
/// </summary>
public sealed class ProjectTaskStatusName : ValueObject
{
    /// <summary>
    /// Maximum ProjectTaskStatus length.
    /// </summary>
    public const int MaxLength = 150;

    /// <summary>
    /// ProjectTaskStatus value.
    /// </summary>
    public string Name { get; }

    private ProjectTaskStatusName(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Creates new <see cref="ProjectTaskStatusName"/> value object with given <paramref name="name"/> string.
    /// Additionally checks whether the maximum length is exceeded or string value is not null or empty.
    /// </summary>
    /// <param name="name">Name for the ProjectTaskStatus.</param>
    /// <returns>The <see cref="Result"/> of creation. Contains <see cref="ProjectTaskStatusName"/> or an <see cref="DomainErrors.DomainError"/>.</returns>
    public static Result<ProjectTaskStatusName> Create(string name)
    {
        return Result
            .Ok(new ProjectTaskStatusName(name))
            .Ensure<ProjectTaskStatusName, NameNullOrEmpty>(() => !string.IsNullOrEmpty(name))
            .Ensure<ProjectTaskStatusName, NameTooLong>(() => name.Length < MaxLength);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Name;
    }
}