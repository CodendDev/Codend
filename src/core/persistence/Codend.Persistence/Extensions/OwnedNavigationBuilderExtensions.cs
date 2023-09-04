using Codend.Domain.Core.Abstractions;
using Codend.Domain.ValueObjects.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codend.Persistence.Extensions;

public static class OwnedNavigationBuilderExtensions
{
    internal static void ConfigureStringValueObject<TOwner, TOwned>(
        this OwnedNavigationBuilder<TOwner, TOwned> builder, string columnName)
        where TOwner : class, IEntity
        where TOwned : class, IStringValueObject
    {
        builder
            .Property(stringValue => stringValue.Value)
            .HasColumnName(columnName)
            .HasMaxLength(TOwned.MaxLength)
            .IsRequired();
    }

    internal static void ConfigureNullableStringValueObject<TOwner, TOwned>(
        this OwnedNavigationBuilder<TOwner, TOwned> builder, string columnName)
        where TOwner : class, IEntity
        where TOwned : class, INullableStringValueObject
    {
        builder
            .Property(stringValue => stringValue.Value)
            .HasColumnName(columnName)
            .HasMaxLength(TOwned.MaxLength);
    }
}