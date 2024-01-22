using Codend.Domain.Entities;
using Codend.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codend.Persistence.Configurations;

internal sealed class EpicConfiguration : IEntityTypeConfiguration<Epic>
{
    public void Configure(EntityTypeBuilder<Epic> builder)
    {
        builder.ConfigureKeyId((Guid guid) => new EpicId(guid));
        builder.ConfigureCreatableEntity();
        builder.ConfigureSoftDeletableEntity();

        builder
            .HasOne<Project>()
            .WithMany()
            .HasForeignKey(epic => epic.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);


        builder.OwnsOne(epic => epic.Name,
            epicNameBuilder =>
            {
                epicNameBuilder.WithOwner();
                epicNameBuilder.ConfigureStringValueObject(nameof(Epic.Name));
            });

        builder.OwnsOne(epic => epic.Description,
            epicDescriptionBuilder =>
            {
                epicDescriptionBuilder.WithOwner();
                epicDescriptionBuilder.ConfigureStringValueObject(nameof(Epic.Description));
            });

        builder
            .HasMany<Story>()
            .WithOne()
            .HasForeignKey(story => story.EpicId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder
            .HasOne<ProjectTaskStatus>()
            .WithMany()
            .HasForeignKey(epic => epic.StatusId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}