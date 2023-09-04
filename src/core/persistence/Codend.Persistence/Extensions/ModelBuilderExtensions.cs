using System.Linq.Expressions;
using Codend.Domain.Core.Abstractions;
using Codend.Domain.Entities;
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
    /// <typeparam name="T"><see cref="ISoftDeletableEntity"/> entity.</typeparam>
    internal static void ConfigureSoftDeletableEntity<T>(this EntityTypeBuilder<T> builder)
        where T : class, ISoftDeletableEntity
    {
        builder
            .Property(entity => entity.DeletedOnUtc)
            .HasPrecision(0);

        builder.Property(entity => entity.Deleted).HasDefaultValue(false);
        builder.HasQueryFilter(entity => !entity.Deleted);
    }

    /// <summary>
    /// Configuration for entity which implements <see cref="ICreatableEntity"/>.
    /// </summary>
    /// <param name="builder">The model builder.</param>
    /// <typeparam name="T"><see cref="ICreatableEntity"/> entity.</typeparam>
    internal static void ConfigureCreatableEntity<T>(this EntityTypeBuilder<T> builder)
        where T : class, ICreatableEntity
    {
        builder
            .Property(entity => entity.CreatedOn)
            .HasPrecision(0);
    }

    internal static void HasUserIdProperty<TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, UserId>> propertyExpression)
        where TEntity : class, IEntity
    {
        builder
            .Property(propertyExpression)
            .HasConversion(id => id.Value, value => new UserId(value));
    }
}