#nullable enable

using System;
using System.Linq;
using Gmc.Cloud.Infrastructure.Utils;

namespace EagleArchiveTests.CommunicationPiece.MetadataGeneration;

// The result of this generator is very hard to evaluate (e.g. for expected size) since random values change their meaning with changing structure of the object. 
class MetadataGeneratorRandomValuesStrategy : IMetadataGeneratorStrategy 
{
    readonly Random _random = new(42);

    public bool ShouldSkipProperty(Type type, string[] propertyPath) =>
        MetadataGeneratorUtils.GetMetadataPropertyPathsToSkip()
            .Any(propertyPathToSkip =>
                propertyPathToSkip.SequenceEqual(propertyPath)
                || (
                    propertyPathToSkip.Length == propertyPath.Length
                    && propertyPathToSkip[^1] == propertyPath[^1]
                    && propertyPath
                        .Select(GetPropertyNameWithoutIndex)
                        .SequenceEqual(propertyPathToSkip)
                )
            );

    static string GetPropertyNameWithoutIndex(string propertyName)
    {
        var braceIndex = propertyName.IndexOf('[');
        return braceIndex == -1 ? propertyName : propertyName[..braceIndex];
    }

    public object GetValue(Type type, string[] propertyPath)
    {
        var valueName = propertyPath[^1];
        
        if (type == typeof(bool))
        {
            return true;
        }
        else if (type == typeof(int))
        {
            return int.MinValue + _random.Next();
        }
        else if (type == typeof(uint))
        {
            return uint.MaxValue - (uint)_random.Next();
        }
        else if (type == typeof(long))
        {
            return long.MinValue + _random.Next();
        }
        else if (type == typeof(ulong))
        {
            return ulong.MaxValue - (ulong)_random.Next();
        }
        else if (type == typeof(decimal))
        {
            return decimal.MinValue + _random.Next();
        }
        else if (type == typeof(double))
        {
            return double.MinValue + _random.Next();
        }
        else if (type == typeof(DateTime))
        {
            return new DateTime(2019, 12, 24, 17, 42, 59, DateTimeKind.Utc).AddDays(_random.NextDouble() * 100);
        }
        else if (type == typeof(string))
        {
            return valueName + " " + _random.Next();
        }
        else if (MetadataGeneratorUtils.IsList(type))
        {
            return _random.Next(1, 10);
        }
        else if (type == typeof(Guid))
        {
            return Guid.NewGuid();
        }
        else if (type.IsClass && type != typeof(string))
        {
            return type.CreatePristineObject<object>();
        }
        else
        {
            throw new NotSupportedException(
                $"Cannot generate data for unsupported type {type} of value '{valueName}'. " +
                $"If this is a new value, add a generator for its type.");
        }
    }
}
