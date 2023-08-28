using Codend.Domain.Entities;
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
    }
}