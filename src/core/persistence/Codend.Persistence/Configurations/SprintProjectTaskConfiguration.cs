using Codend.Domain.Entities;
using Codend.Persistence.Extensions;
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
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne<Sprint>()
            .WithMany()
            .HasForeignKey(sprintTask => sprintTask.SprintId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}