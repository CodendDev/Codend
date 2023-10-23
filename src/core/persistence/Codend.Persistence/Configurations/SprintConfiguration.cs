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
        builder.ConfigureSoftDeletableEntity();
        builder.ConfigureCreatableEntity();

        builder
            .OwnsOne(sprint => sprint.Period,
                sprintPeriod =>
                {
                    sprintPeriod
                        .Property(period => period.StartDate)
                        .HasColumnName(nameof(SprintPeriod.StartDate))
                        .HasPrecision(0)
                        .IsRequired();

                    sprintPeriod
                        .Property(period => period.EndDate)
                        .HasColumnName(nameof(SprintPeriod.EndDate))
                        .HasPrecision(0)
                        .IsRequired();
                });
        
        builder.OwnsOne(sprint => sprint.Name,
            epicNameBuilder =>
            {
                epicNameBuilder.WithOwner();
                epicNameBuilder.ConfigureStringValueObject(nameof(Sprint.Name));
            });

        builder
            .OwnsOne(sprint => sprint.Goal,
                sprintGoal => sprintGoal.ConfigureNullableStringValueObject(nameof(Sprint.Goal)));

        builder
            .HasMany<BaseProjectTask>()
            .WithMany()
            .UsingEntity("SprintProjectTask");
    }
}