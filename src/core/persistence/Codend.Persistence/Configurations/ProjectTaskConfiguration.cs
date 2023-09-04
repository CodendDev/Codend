using Codend.Domain.Core.Enums;
using Codend.Domain.Entities;
using Codend.Domain.ValueObjects;
using Codend.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Codend.Persistence.Configurations;

/// <summary>
/// Entity framework configuration for the <see cref="ProjectTask"/> entity.
/// </summary>
internal sealed class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ProjectTask> builder)
    {
        builder.ConfigureKeyId((Guid guid) => new ProjectTaskId(guid));
        builder.ConfigureSoftDeletableEntity();
        builder.ConfigureCreatableEntity();

        builder
            .OwnsOne(projectTask => projectTask.Name,
                projectTaskNameBuilder =>
                {
                    projectTaskNameBuilder.WithOwner();

                    projectTaskNameBuilder.Property(projectTaskName => projectTaskName.Value)
                        .HasColumnName(nameof(ProjectTask.Name))
                        .HasMaxLength(ProjectTaskName.MaxLength)
                        .IsRequired();
                });

        builder
            .OwnsOne(projectTask => projectTask.Description,
                projectNameBuilder =>
                {
                    projectNameBuilder.WithOwner();

                    projectNameBuilder.Property(projectTaskName => projectTaskName.Value)
                        .HasColumnName(nameof(ProjectTask.Description))
                        .HasMaxLength(ProjectTaskDescription.MaxLength)
                        .IsRequired();
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
            .HasColumnName(nameof(ProjectTask.DueDate));

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(projectTask => projectTask.OwnerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(projectTask => projectTask.AssigneeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .Property(projectTask => projectTask.EstimatedTime)
            .HasConversion(new TimeSpanToTicksConverter())
            .HasPrecision(0);

        builder
            .Property(projectTask => projectTask.StoryPoints);
    }
}