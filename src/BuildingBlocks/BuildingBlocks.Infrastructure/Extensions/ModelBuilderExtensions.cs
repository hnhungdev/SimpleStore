using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BuildingBlocks.Infrastructure.Extensions;

public static partial class ModelBuilderExtensions
{
    public static ModelBuilder UseSnakeCaseNaming(
        this ModelBuilder builder)
    {
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            ConfigureEntity(entity);
        }

        return builder;
    }

    private static void ConfigureEntity(IMutableEntityType entity)
    {
        var tableName = entity.GetTableName();

        if (!string.IsNullOrEmpty(tableName))
        {
            entity.SetTableName(ToSnakeCase(tableName));
        }

        foreach (var property in entity.GetProperties())
        {
            var columnName = property.GetColumnName();

            if (!string.IsNullOrEmpty(columnName))
            {
                property.SetColumnName(ToSnakeCase(columnName));
            }
        }

        foreach (var complexProperty in entity.GetComplexProperties())
        {
            ConfigureComplexProperty(complexProperty);
        }
    }

    private static void ConfigureComplexProperty(
        IMutableComplexProperty complexProperty,
        string? parentPrefix = null)
    {
        var currentPrefix = string.IsNullOrEmpty(parentPrefix)
            ? complexProperty.Name
            : $"{parentPrefix}_{complexProperty.Name}";

        foreach (var property in complexProperty.ComplexType.GetProperties())
        {
            var columnName = $"{currentPrefix}_{property.Name}";

            property.SetColumnName(ToSnakeCase(columnName));
        }

        foreach (var nestedProperty in complexProperty.ComplexType.GetComplexProperties())
        {
            ConfigureComplexProperty(
                nestedProperty,
                currentPrefix);
        }
    }

    private static string ToSnakeCase(string name)
    {
        if (string.IsNullOrEmpty(name))
            return name;

        return SnakeCaseRegex().Replace(name, "$1_$2").ToLowerInvariant();
    }

    [GeneratedRegex(@"([a-z0-9])([A-Z])")]
    private static partial Regex SnakeCaseRegex();
}