using Codend.Domain.Core.Abstractions;
using Codend.Domain.ValueObjects.Abstractions;
using Codend.Domain.ValueObjects.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codend.Persistence.Extensions;

internal static class OwnedNavigationBuilderExtensions
{
    internal static void ConfigureStringValueObject<TOwner, TOwned>(
        this OwnedNavigationBuilder<TOwner, TOwned> builder, string columnName)
        where TOwner : class, IEntity
        where TOwned : StringValueObject, IStringMaxLengthValueObject
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
        where TOwned : NullableStringValueObject, IStringMaxLengthValueObject
    {
        builder
            .Property(stringValue => stringValue.Value)
            .HasColumnName(columnName)
            .HasMaxLength(TOwned.MaxLength);
    }
}