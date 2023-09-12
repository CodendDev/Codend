using Ardalis.SmartEnum;

namespace Codend.Domain.Core.Enums;

/// <summary>
/// Unmutable task priority values.
/// </summary>
public sealed class ProjectTaskPriority : SmartEnum<ProjectTaskPriority>
{
    public static readonly ProjectTaskPriority VeryHigh = new ProjectTaskPriority(nameof(VeryHigh), 1);
    public static readonly ProjectTaskPriority High = new ProjectTaskPriority(nameof(High), 2);
    public static readonly ProjectTaskPriority Normal = new ProjectTaskPriority(nameof(Normal), 3);
    public static readonly ProjectTaskPriority Low = new ProjectTaskPriority(nameof(Low), 4);
    public static readonly ProjectTaskPriority VeryLow = new ProjectTaskPriority(nameof(VeryLow), 5);

    public static IEnumerable<ProjectTaskPriority> DefaultList()
    {
        yield return VeryLow;
        yield return Low;
        yield return Normal;
        yield return High;
        yield return VeryHigh;
    }

    /// <summary>
    /// Parameterless constructor for ef.
    /// </summary>
    private ProjectTaskPriority() : base("default", 0)
    {
    }

    private ProjectTaskPriority(string name, int value) : base(name, value)
    {
    }
}