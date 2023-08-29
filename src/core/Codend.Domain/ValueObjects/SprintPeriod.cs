using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Extensions;
using Codend.Domain.Core.Primitives;
using FluentResults;

namespace Codend.Domain.ValueObjects;

/// <summary>
/// Project sprint period.
/// </summary>
public sealed class SprintPeriod : ValueObject
{
    /// <summary>
    /// Sprint period start date.
    /// </summary>
    public DateTime StartDate { get; }

    /// <summary>
    /// Sprint period end date.
    /// </summary>
    public DateTime EndDate { get; }

    // TODO: Make sure to set startdate as beggining of the day and enddate as end of the day
    public SprintPeriod(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    /// <summary>
    /// Creates <see cref="SprintPeriod"/> instance.
    /// </summary>
    /// <param name="startDate">StartDate value.</param>
    /// <param name="endDate">EndDate value.</param>
    /// <returns>The result of sprint period creation. Returns sprint period or an error.</returns>
    public static Result<SprintPeriod> Create(DateTime startDate, DateTime endDate)
    {
        return Result
            .Ok(new SprintPeriod(startDate, endDate))
            .Ensure(() => startDate.CompareTo(endDate) < 0, new DomainErrors.ProjectVersionTag.TagTooLong());
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return StartDate;
        yield return EndDate;
    }
}