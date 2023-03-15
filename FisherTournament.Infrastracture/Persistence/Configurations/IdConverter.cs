namespace FisherTournament.Infrastracture.Persistence.Configurations;

using System.Linq.Expressions;
using FisherTournament.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class IdConverter<T> : ValueConverter<T, Guid> where T : GuidId<T>
{
    public IdConverter()
        : base(x => x.Value, x => GuidId<T>.Create(x))
    {
    }
}

public static partial class Extension
{
    public static PropertyBuilder<T> HasGuidIdConversion<T>(
            this PropertyBuilder<T> propertyBuilder)
        where T : GuidId<T>
    {
        var converter = new ValueConverter<T, Guid>(
            x => x.Value,
            x => GuidId<T>.Create(x));

        propertyBuilder.HasConversion(converter);

        return propertyBuilder;
    }

    public static PropertyBuilder HasGuidIdConversion<T>(
            this PropertyBuilder propertyBuilder)
        where T : GuidId<T>
    {
        var converter = new ValueConverter<T, Guid>(
            x => x.Value,
            x => GuidId<T>.Create(x));

        propertyBuilder.HasConversion(converter);

        return propertyBuilder;
    }

    public static void HasGuidIdKey<F, T>(
            this EntityTypeBuilder<F> modelBuilder,
            Expression<Func<F, T>> keyExpression)
        where T : GuidId<T>
        where F : class
    {
        // cast keyExpression to Expression<Func<F, object?>>
        Expression<Func<F, object?>> keyExpression2 = Expression.Lambda<Func<F, object?>>(
            keyExpression.Body,
            keyExpression.Parameters);

        var propertyBuilder = modelBuilder.HasKey(
            keyExpression2);

        var converter = new ValueConverter<T, Guid>(
            x => x.Value,
            x => GuidId<T>.Create(x));

        modelBuilder.Property(keyExpression)
                    .HasConversion(converter)
                    .ValueGeneratedNever();
    }
}