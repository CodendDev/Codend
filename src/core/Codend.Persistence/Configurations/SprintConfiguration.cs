using Codend.Domain.Entities;
using Codend.Domain.ValueObjects;
using Codend.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codend.Persistence.Configurations;

/// <summary>
/// Entity framework configuration for the <see cref="Sprint"/> entity.
/// </summary>
internal sealed class SprintConfiguration : IEntityTypeConfiguration<Sprint>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Sprint> builder)
    {
        builder.ConfigureKeyId((Guid guid) => new SprintId(guid));
        
        builder
            .OwnsOne(sprint => sprint.Period,
            sprintBuilder =>
            {
                sprintBuilder
                    .Property(period => period.StartDate)
                    .HasColumnName(nameof(SprintPeriod.StartDate))
                    .HasPrecision(0)
                    .IsRequired();
                
                sprintBuilder
                    .Property(period => period.EndDate)
                    .HasColumnName(nameof(SprintPeriod.EndDate))
                    .HasPrecision(0)
                    .IsRequired();
            });
        
        builder
            .OwnsOne(sprint => sprint.Goal,
            sprintBuilder =>
            {
                sprintBuilder
                    .Property(period => period.Goal)
                    .HasColumnName(nameof(Sprint.Goal))
                    .HasMaxLength(SprintGoal.MaxLength);
            });

        builder.ConfigureSoftDeletableEntity();
    }
}