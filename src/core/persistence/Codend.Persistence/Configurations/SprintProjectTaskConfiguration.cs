using Codend.Domain.Entities;
using Codend.Persistence.Extensions;
using Codend.Shared.Infrastructure.Lexorank;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codend.Persistence.Configurations;

internal sealed class SprintProjectTaskConfiguration : IEntityTypeConfiguration<SprintProjectTask>
{
    public void Configure(EntityTypeBuilder<SprintProjectTask> builder)
    {
        builder.ConfigureKeyId((Guid guid) => new SprintProjectTaskId(guid));
        builder.ConfigureCreatableEntity();

        builder
            .HasOne<BaseProjectTask>()
            .WithMany()
            .HasForeignKey(sprintTask => sprintTask.TaskId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne<Epic>()
            .WithMany()
            .HasForeignKey(sprintTask => sprintTask.EpicId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne<Story>()
            .WithMany()
            .HasForeignKey(sprintTask => sprintTask.StoryId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne<Sprint>()
            .WithMany()
            .HasForeignKey(sprintTask => sprintTask.SprintId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .Property(sprintProjectTask => sprintProjectTask.Position)
            .HasConversion(
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                position => position.Value,
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                value => new Lexorank(value));
    }
}