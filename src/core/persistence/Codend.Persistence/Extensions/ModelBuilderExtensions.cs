using Codend.Domain.Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codend.Persistence.Extensions;

/// <summary>
/// Extensions methods for <see cref="ModelBuilder"/> class.
/// </summary>
internal static class ModelBuilderExtensions
{
    /// <summary>
    /// Configures entity id
    /// </summary>
    /// <param name="builder">The model builder.</param>
    /// <param name="idConverter">An expression to convert objects when reading data from the store.</param>
    /// <typeparam name="T">Entity class.</typeparam>
    /// <typeparam name="TKey">Entity key class.</typeparam>
    /// <typeparam name="TKeyPrimitive">Primitive class.</typeparam>
    internal static void ConfigureKeyId<T, TKey, TKeyPrimitive>(
        this EntityTypeBuilder<T> builder,
        Func<TKeyPrimitive, TKey> idConverter)
        where T : class, IEntity<TKey>
        where TKey : IEntityId<TKeyPrimitive>
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasConversion(
                id => id.Value,
                value => idConverter(value));
    }


    /// <summary>
    /// Configuration for entity which implements <see cref="ISoftDeletableEntity"/>.
    /// </summary>
    /// <param name="builder">The model builder.</param>
    /// <typeparam name="T"><see cref="ISoftDeletableEntity"/> entity</typeparam>
    internal static void ConfigureSoftDeletableEntity<T>(this EntityTypeBuilder<T> builder)
        where T : class, ISoftDeletableEntity
    {
        builder.Property(project => project.Deleted).HasDefaultValue(false);
        builder.HasQueryFilter(project => !project.Deleted);
    }
}