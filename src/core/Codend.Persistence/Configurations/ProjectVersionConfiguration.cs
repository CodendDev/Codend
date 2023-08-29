using Codend.Domain.Entities;
using Codend.Domain.ValueObjects;
using Codend.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codend.Persistence.Configurations;

/// <summary>
/// Entity framework configuration for the <see cref="ProjectVersion"/> entity.
/// </summary>
internal sealed class ProjectVersionConfiguration : IEntityTypeConfiguration<ProjectVersion>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ProjectVersion> builder)
    {
        builder.ConfigureKeyId((Guid guid) => new ProjectVersionId(guid));

        builder.ConfigureSoftDeletableEntity();

        builder.OwnsOne(projectVersion => projectVersion.Changelog,
            projectVersionBuilder =>
            {
                projectVersionBuilder.WithOwner();

                projectVersionBuilder
                    .Property(changelog => changelog.Changelog)
                    .HasColumnName(nameof(ProjectVersion.Changelog))
                    .HasMaxLength(ProjectVersionChangelog.MaxLength);
            });
    }
}