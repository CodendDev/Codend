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

        builder
            .HasOne<Backlog>(project => project.Backlog)
            .WithOne()
            .HasForeignKey<Backlog>(backlog => backlog.ProjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(project => project.ProjectVersions)
            .WithOne()
            .HasForeignKey(projectVersion => projectVersion.ProjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany(project => project.Sprints)
            .WithOne()
            .HasForeignKey(projectVersion => projectVersion.ProjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.ConfigureSoftDeletableEntity();

        builder.OwnsOne(project => project.ProjectName,
            projectNameBuilder =>
            {
                projectNameBuilder.WithOwner();

                projectNameBuilder
                    .Property(projectName => projectName.Name)
                    .HasColumnName(nameof(Project.ProjectName))
                    .HasMaxLength(ProjectName.MaxLength)
                    .IsRequired();
            });
        
        builder.OwnsOne(project => project.ProjectDescription,
            projectDescriptionBuilder =>
            {
                projectDescriptionBuilder.WithOwner();

                projectDescriptionBuilder
                    .Property(projectDescription => projectDescription.Description)
                    .HasColumnName(nameof(Project.ProjectDescription))
                    .HasMaxLength(ProjectDescription.MaxLength);
            });
    }
}