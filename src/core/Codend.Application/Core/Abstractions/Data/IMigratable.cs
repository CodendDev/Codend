namespace Codend.Application.Core.Abstractions.Data;

/// <summary>
/// Represents database to which migrations can be applied.
/// </summary>
public interface IMigratable
{
    /// <summary>
    /// Executes the migration process.
    /// </summary>
    void Migrate();
}