using Codend.Domain.Entities;
using Codend.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codend.Persistence.Configurations;

internal sealed class StoryConfiguration : IEntityTypeConfiguration<Story>
{
    public void Configure(EntityTypeBuilder<Story> builder)
    {
        builder.ConfigureKeyId((Guid guid) => new StoryId(guid));
        builder.ConfigureCreatableEntity();
        builder.ConfigureSoftDeletableEntity();

        builder
            .HasOne<Project>()
            .WithMany()
            .HasForeignKey(story => story.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.OwnsOne(story => story.Name,
            storyNameBuilder =>
            {
                storyNameBuilder.WithOwner();
                storyNameBuilder.ConfigureStringValueObject(nameof(Story.Name));
            });

        builder.OwnsOne(story => story.Description,
            storyDescriptionBuilder =>
            {
                storyDescriptionBuilder.WithOwner();
                storyDescriptionBuilder.ConfigureStringValueObject(nameof(Story.Description));
            });
        
        builder
            .HasOne<ProjectTaskStatus>()
            .WithMany()
            .HasForeignKey(story => story.StatusId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}