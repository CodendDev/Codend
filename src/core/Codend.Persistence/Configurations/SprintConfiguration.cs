using Codend.Domain.Entities;
using Codend.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codend.Persistence.Configurations;

/// <summary>
/// Entity framework configuration for the <see cref="Sprint"/> entity.
/// </summary>
internal sealed class SprintConfiguration : IEntityTypeConfiguration<Sprint>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Sprint> builder)
    {
        builder.ConfigureKeyId((Guid guid) => new SprintId(guid));

        builder.ConfigureSoftDeletableEntity();
    }
}