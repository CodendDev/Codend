using Codend.Domain.Entities;
using Codend.Persistence.Extensions;
using Codend.Shared.Infrastructure.Lexorank;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codend.Persistence.Configurations;

/// <summary>
/// Entity framework configuration for the <see cref="ProjectTaskStatus"/> entity.
/// </summary>
internal sealed class ProjectTaskStatusConfiguration : IEntityTypeConfiguration<ProjectTaskStatus>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ProjectTaskStatus> builder)
    {
        builder.ConfigureKeyId((Guid guid) => new ProjectTaskStatusId(guid));

        builder.OwnsOne(projectTaskStatus => projectTaskStatus.Name,
            projectTaskStatusNameBuilder =>
            {
                projectTaskStatusNameBuilder.WithOwner();
                projectTaskStatusNameBuilder.ConfigureStringValueObject(nameof(ProjectTaskStatus.Name));
            });

        builder
            .Property(projectTaskStatus => projectTaskStatus.Position)
            .HasConversion(
                position => position.Value,
                value => new Lexorank(value));
    }
}