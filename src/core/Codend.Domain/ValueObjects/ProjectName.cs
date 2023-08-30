using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Core.Extensions;
using FluentResults;
using NullOrEmpty = Codend.Domain.Core.Errors.DomainErrors.ProjectName.NullOrEmpty;
using NameTooLong = Codend.Domain.Core.Errors.DomainErrors.ProjectName.NameTooLong;

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
    /// <returns>The <see cref="Result"/> of creation. Contains <see cref="ProjectName"/> or an <see cref="DomainErrors.DomainError"/>.</returns>

    public static Result<ProjectName> Create(string name)
    {
        return Result
            .Ok(new ProjectName(name))
            .Ensure<ProjectName, NullOrEmpty>(() => !string.IsNullOrEmpty(name))
            .Ensure<ProjectName, NameTooLong>(() => name.Length < MaxLength);
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Name;
    }
}