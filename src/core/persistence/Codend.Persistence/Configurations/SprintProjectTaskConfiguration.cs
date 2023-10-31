using Codend.Domain.Entities;
using Codend.Persistence.Extensions;
using Codend.Shared.Infrastructure.Lexorank;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codend.Persistence.Configurations;

public class SprintProjectTaskConfiguration : IEntityTypeConfiguration<SprintProjectTask>
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
                position => position.Value,
                value => new Lexorank(value));
    }
}