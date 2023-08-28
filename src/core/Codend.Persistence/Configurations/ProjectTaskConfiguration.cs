using Codend.Domain.Entities;
using Codend.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
    }
}