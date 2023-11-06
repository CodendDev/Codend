using Codend.Domain.Core.Enums;
using Codend.Domain.Entities;
using Codend.Domain.ValueObjects;
using Codend.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Codend.Persistence.Configurations;

/// <summary>
/// Entity framework configuration for the <see cref="BaseProjectTask"/> entity.
/// </summary>
internal sealed class ProjectTaskConfiguration : IEntityTypeConfiguration<BaseProjectTask>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<BaseProjectTask> builder)
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
                    projectTaskNameBuilder.ConfigureStringValueObject(nameof(BaseProjectTask.Name));
                });

        builder
            .OwnsOne(projectTask => projectTask.Description,
                projectNameBuilder =>
                {
                    projectNameBuilder.WithOwner();
                    projectNameBuilder.ConfigureNullableStringValueObject(nameof(BaseProjectTask.Description));
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
            .HasColumnName(nameof(BaseProjectTask.DueDate));

        builder
            .HasUserIdProperty(projectTask => projectTask.OwnerId);

        builder
#pragma warning disable CS8603 // Possible null reference return.
            .HasUserIdProperty(projectTask => projectTask.AssigneeId);
#pragma warning restore CS8603 // Possible null reference return.

        builder
            .Property(projectTask => projectTask.EstimatedTime)
            .HasConversion(new TimeSpanToTicksConverter())
            .HasPrecision(0);

        builder
            .Property(projectTask => projectTask.StoryPoints);

        builder
            .HasOne<Story>()
            .WithMany()
            .HasForeignKey(task => task.StoryId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}