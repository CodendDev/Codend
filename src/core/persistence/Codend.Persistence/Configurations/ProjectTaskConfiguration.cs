using Codend.Domain.Core.Enums;
using Codend.Domain.Entities;
using Codend.Domain.ValueObjects;
using Codend.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Codend.Persistence.Configurations;

/// <summary>
/// Entity framework configuration for the <see cref="AbstractProjectTask"/> entity.
/// </summary>
internal sealed class ProjectTaskConfiguration : IEntityTypeConfiguration<AbstractProjectTask>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<AbstractProjectTask> builder)
    {
        builder.ConfigureKeyId((Guid guid) => new ProjectTaskId(guid));
        builder.ConfigureSoftDeletableEntity();
        builder.ConfigureCreatableEntity();

        builder
            .OwnsOne(projectTask => projectTask.Name,
                projectTaskNameBuilder =>
                {
                    projectTaskNameBuilder.WithOwner();
                    projectTaskNameBuilder.ConfigureStringValueObject(nameof(AbstractProjectTask.Name));
                });

        builder
            .OwnsOne(projectTask => projectTask.Description,
                projectNameBuilder =>
                {
                    projectNameBuilder.WithOwner();
                    projectNameBuilder.ConfigureNullableStringValueObject(nameof(AbstractProjectTask.Description));
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
            .HasColumnName(nameof(AbstractProjectTask.DueDate));

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