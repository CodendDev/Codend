using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using Codend.Domain.Core.Primitives;
using FluentResults;
using NullOrEmpty = Codend.Domain.Core.Errors.DomainErrors.ProjectVersionName.NullOrEmpty;
using NameTooLong = Codend.Domain.Core.Errors.DomainErrors.ProjectVersionName.NameTooLong;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// Project version name value object.
/// </summary>
public sealed class ProjectVersionName : ValueObject
{
    /// <summary>
    /// Maximum name length.
    /// </summary>
    public const int MaxLength = 50;

    /// <summary>
    /// Version name value.
    /// </summary>
    public string Name { get; }

    private ProjectVersionName(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Creates <see cref="ProjectVersionName" /> instance.
    /// </summary>
    /// <param name="name">Project version name value.</param>
    /// <returns>The <see cref="Result"/> of creation. Contains <see cref="ProjectVersionName"/> or an <see cref="DomainErrors.DomainError"/>.</returns>
    public static Result<ProjectVersionName> Create(string name)
    {
        return Result
            .Ok(new ProjectVersionName(name))
            .Ensure<ProjectVersionName, NullOrEmpty>(() => !string.IsNullOrEmpty(name))
            .Ensure<ProjectVersionName, NameTooLong>(() => name.Length < MaxLength);
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Name;
    }
}