using Codend.Domain.Core.Enums;
using Codend.Domain.Entities;
using Codend.Domain.ValueObjects;
using Codend.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Codend.Persistence.Configurations;

/// <summary>
/// Entity framework configuration for the <see cref="ProjectTaskBase"/> entity.
/// </summary>
internal sealed class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTaskBase>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ProjectTaskBase> builder)
    {
        builder.ToTable("ProjectTask");
        builder.ConfigureKeyId((Guid guid) => new ProjectTaskId(guid));
        builder.ConfigureSoftDeletableEntity();
        builder.ConfigureCreatableEntity();

        builder
            .OwnsOne(projectTask => projectTask.Name,
                projectTaskNameBuilder =>
                {
                    projectTaskNameBuilder.WithOwner();
                    projectTaskNameBuilder.ConfigureStringValueObject(nameof(ProjectTaskBase.Name));
                });

        builder
            .OwnsOne(projectTask => projectTask.Description,
                projectNameBuilder =>
                {
                    projectNameBuilder.WithOwner();
                    projectNameBuilder.ConfigureNullableStringValueObject(nameof(ProjectTaskBase.Description));
                });

        builder
            .Property(projectTask => projectTask.Priority)
            .HasConversion(priority => priority.Value,
                p => ProjectTaskPriority.FromValue(p))
            .HasColumnName(nameof(ProjectTaskPriority))
            .IsRequired();

        builder
            .HasOne<ProjectTaskStatus>()
            .WithMany()
            .HasForeignKey(projectTask => projectTask.StatusId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .Property(projectTask => projectTask.DueDate)
            .HasColumnName(nameof(ProjectTaskBase.DueDate));

        builder
            .HasUserIdProperty(projectTask => projectTask.OwnerId);

        builder
            .HasUserIdProperty(projectTask => projectTask.AssigneeId);

        builder
            .Property(projectTask => projectTask.EstimatedTime)
            .HasConversion(new TimeSpanToTicksConverter())
            .HasPrecision(0);

        builder
            .Property(projectTask => projectTask.StoryPoints);
    }
}