#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JsonBenchmarks.Dto;

namespace EagleArchiveTests.CommunicationPiece.MetadataGeneration;

class MetadataGenerator
{
    readonly IMetadataGeneratorStrategy _metadataGeneratorStrategy;

    public MetadataGenerator(IMetadataGeneratorStrategy? metadataGeneratorStrategy = null)
    {
        _metadataGeneratorStrategy = metadataGeneratorStrategy ?? new MetadataGeneratorRandomValuesStrategy();
    }
    
    public Metadata Next(VisitUsedValue? visitUsedValue = null)
    {
        var metadata = new Metadata();
        lock (_metadataGeneratorStrategy)
        {
            GenerateValuesForObject(metadata, [], visitUsedValue);    
        }
        return metadata;
    }

    void GenerateValuesForObject(object obj, string[] propertyPath, VisitUsedValue? visitUsedValue = null)
    {
        foreach (var property in GetSettableObjectProperties(obj.GetType()))
        {
            var currentPropertyType = property.PropertyType;
            string[] currentPropertyPath = [.. propertyPath, property.Name];

            if (_metadataGeneratorStrategy.ShouldSkipProperty(currentPropertyType, currentPropertyPath))
            {
                continue;
            }
            
            var value = GenerateValueForProperty(currentPropertyPath, currentPropertyType, visitUsedValue);
            property.SetValue(obj, value);
        }
    }

    object? GenerateValueForProperty(string[] propertyPath, Type type, VisitUsedValue? visitUsedValue = null)
    {
        if (type.IsGenericType)
        {
            var genericType = type.GetGenericTypeDefinition();
            if (genericType == typeof(Nullable<>))
            {
                type = type.GetGenericArguments()[0];
            }
        }

        object? value;
        
        if (MetadataGeneratorUtils.IsList(type))
        {
            var itemsInListCount = _metadataGeneratorStrategy.GetValue(type, propertyPath);

            if (itemsInListCount is not int count)
            {
                value = null;
            }
            else
            {
                var itemType = type.GetGenericArguments()[0];
                var itemList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType))!;
                
                visitUsedValue?.Invoke(type, propertyPath, itemsInListCount);
            
                var items = Enumerable.Range(0, count).Select(i => GenerateValueForProperty([.. propertyPath.Take(propertyPath.Length - 1), $"{propertyPath[^1]}[{i}]"], itemType, visitUsedValue));
                foreach (var item in items)
                {
                    itemList.Add(item);
                }
                value = itemList;
            }
        }
        else if ((type.IsClass && type != typeof(string)))
        {
            value = _metadataGeneratorStrategy.GetValue(type, propertyPath);
            if (value is not null)
            {
                GenerateValuesForObject(value, propertyPath, visitUsedValue);
            }
        }
        else
        {
            value = _metadataGeneratorStrategy.GetValue(type, propertyPath);
        }

        visitUsedValue?.Invoke(type, propertyPath, value);
        return value;
    }

    static IEnumerable<PropertyInfo> GetSettableObjectProperties(Type type)
    {
        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanWrite && p.GetIndexParameters().Length == 0);
    }
}
