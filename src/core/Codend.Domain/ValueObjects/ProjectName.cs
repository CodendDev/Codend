using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Core.Extensions;
using FluentResults;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// Project name value object.
/// </summary>
public sealed class ProjectName : ValueObject
{
    /// <summary>
    /// Maximum name length.
    /// </summary>
    public const int MaxLength = 100;

    /// <summary>
    /// Project name value.
    /// </summary>
    public string Name { get; }

    private ProjectName(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Creates <see cref="ProjectName"/> instance.
    /// </summary>
    /// <param name="name">Name value.</param>
    /// <returns>The result of creation. Returns project name or an error.</returns>
    public static Result<ProjectName> Create(string name)
    {
        return Result
            .Ok(new ProjectName(name))
            .Ensure(() => !string.IsNullOrEmpty(name), new DomainErrors.ProjectName.NullOrEmpty())
            .Ensure(() => name.Length < MaxLength, new DomainErrors.ProjectName.NameTooLong());
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Name;
    }
}