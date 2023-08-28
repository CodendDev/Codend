using Codend.Domain.Entities;
using Codend.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codend.Persistence.Configurations;

/// <summary>
/// Entity framework configuration for the <see cref="Backlog"/> entity.
/// </summary>
internal sealed class BacklogConfiguration : IEntityTypeConfiguration<Backlog>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Backlog> builder)
    {
        builder.ConfigureKeyId((Guid guid) => new BacklogId(guid));

        builder
            .HasMany(backlog => backlog.ProjectTasks)
            .WithOne()
            .HasForeignKey(projectTask => projectTask.BacklogId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}