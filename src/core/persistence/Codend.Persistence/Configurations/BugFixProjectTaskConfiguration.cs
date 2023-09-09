using Codend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codend.Persistence.Configurations;

/// <summary>
/// Entity framework configuration for the <see cref="BugfixProjectTask"/> entity.
/// </summary>
internal sealed class ExampleProjectTaskConfiguration : IEntityTypeConfiguration<BugfixProjectTask>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<BugfixProjectTask> builder)
    {
    }
}