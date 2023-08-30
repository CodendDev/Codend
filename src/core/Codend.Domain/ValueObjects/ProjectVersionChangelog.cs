using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using Codend.Domain.Core.Primitives;
using FluentResults;
using DescriptionTooLong = Codend.Domain.Core.Errors.DomainErrors.ProjectVersionChangelog.DescriptionTooLong;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// Project version changelog value object.
/// </summary>
public sealed class ProjectVersionChangelog : ValueObject
{
    /// <summary>
    /// Maximum description length.
    /// </summary>
    public const int MaxLength = 3000;

    /// <summary>
    /// Version changelog value.
    /// </summary>
    public string Changelog { get; }

    private ProjectVersionChangelog(string changelog)
    {
        Changelog = changelog;
    }

    /// <summary>
    /// Creates <see cref="ProjectVersionChangelog" /> instance.
    /// </summary>
    /// <param name="changelog">Changelog value.</param>
    /// <returns>The <see cref="Result"/> of creation. Contains <see cref="ProjectVersionChangelog"/> or an <see cref="DomainErrors.DomainError"/>.</returns>
    public static Result<ProjectVersionChangelog> Create(string changelog)
    {
        return Result
            .Ok(new ProjectVersionChangelog(changelog))
            .Ensure<ProjectVersionChangelog, DescriptionTooLong>(() => changelog.Length < MaxLength);
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Changelog;
    }
}