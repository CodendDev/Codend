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
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                position => position.Value,
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                value => new Lexorank(value));
    }
}