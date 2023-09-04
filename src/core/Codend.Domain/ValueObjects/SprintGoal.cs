using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using FluentResults;
using GoalTooLong = Codend.Domain.Core.Errors.DomainErrors.SprintGoal.GoalTooLong;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// Project sprint goal.
/// </summary>
public sealed class SprintGoal : NullableStringValueObject
{
    /// <summary>
    /// Maximum goal length.
    /// </summary>
    public const int MaxLength = 200;

    private SprintGoal(string? value) : base(value)
    {
    }

    /// <summary>
    /// Creates <see cref="SprintGoal"/> instance.
    /// </summary>
    /// <param name="value">Goal value.</param>
    /// <returns>The result of sprint goal creation. Returns sprint goal or an error.</returns>
    public static Result<SprintGoal> Create(string? value)
    {
        return Result
            .Ok(new SprintGoal(value))
            .Ensure<SprintGoal, GoalTooLong>(() => value is null || value.Length < MaxLength);
    }
}