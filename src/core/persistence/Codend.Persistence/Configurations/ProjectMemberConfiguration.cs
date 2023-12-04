using Codend.Domain.Entities;
using Codend.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codend.Persistence.Configurations;

/// <summary>
/// Entity framework configuration for the <see cref="ProjectMember"/> entity.
/// </summary>
internal sealed class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ProjectMember> builder)
    {
        builder.ConfigureKeyId((Guid guid) => new ProjectMemberId(guid));
        builder.ConfigureCreatableEntity();

        builder
            .Property(projectMember => projectMember.ProjectId)
            .HasConversion(id => id.Value, value => new ProjectId(value));
        builder
            .HasUserIdProperty(projectMember => projectMember.MemberId);

        builder
            .Property(member => member.NotificationEnabled)
            .IsRequired();
    }
}