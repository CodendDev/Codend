using Codend.Domain.Entities;
using Codend.Domain.ValueObjects;
using Codend.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codend.Persistence.Configurations;

/// <summary>
/// Entity framework configuration for the <see cref="Project"/> entity.
/// </summary>
internal sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ConfigureKeyId((Guid guid) => new ProjectId(guid));
        builder.ConfigureSoftDeletableEntity();
        builder.ConfigureCreatableEntity();

        builder
            .HasMany<ProjectTask>()
            .WithOne()
            .HasForeignKey(projectTask => projectTask.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany<ProjectVersion>()
            .WithOne()
            .HasForeignKey(projectVersion => projectVersion.ProjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany<Sprint>()
            .WithOne()
            .HasForeignKey(sprint => sprint.ProjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.OwnsOne(project => project.Name,
            projectNameBuilder =>
            {
                projectNameBuilder.WithOwner();

                projectNameBuilder
                    .Property(projectName => projectName.Name)
                    .HasColumnName(nameof(Project.Name))
                    .HasMaxLength(ProjectName.MaxLength)
                    .IsRequired();
            });

        builder.OwnsOne(project => project.Description,
            projectDescriptionBuilder =>
            {
                projectDescriptionBuilder.WithOwner();

                projectDescriptionBuilder
                    .Property(projectDescription => projectDescription.Description)
                    .HasColumnName(nameof(Project.Description))
                    .HasMaxLength(ProjectDescription.MaxLength);
            });

        builder
            .HasMany<ProjectTaskStatus>()
            .WithOne()
            .HasForeignKey(status => status.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne<User>()
            .WithMany(user => user.ProjectsOwned)
            .HasForeignKey(project => project.OwnerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany<User>()
            .WithMany(user => user.ParticipatingInProjects)
            .UsingEntity("ProjectMember");
    }
}