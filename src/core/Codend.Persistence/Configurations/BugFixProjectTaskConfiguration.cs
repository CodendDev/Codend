using Codend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codend.Persistence.Configurations;

/// <summary>
/// Entity framework configuration for the <see cref="BugFixProjectTask"/> entity.
/// </summary>
internal sealed class ExampleProjectTaskConfiguration : IEntityTypeConfiguration<BugFixProjectTask>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<BugFixProjectTask> builder)
    {
    }
}