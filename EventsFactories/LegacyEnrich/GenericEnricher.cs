using System.Reflection;

namespace EventsFactories.LegacyEnrich;

public class GenericEnricher
{
    const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public;

    void Enrich(object? request, object? ev)
    {
        if (request == null) return;
        if (ev == null) return;
        var srcFields = request.GetType().GetProperties(Flags);
        var destFields = ev.GetType().GetProperties(Flags);
        foreach (var property in srcFields)
        {
            var dest = TryGetDestinationProperty(property, destFields);
            if (dest != null && dest.CanWrite)
                dest.SetValue(ev, property.GetValue(request, null), null);
        }
    }

    PropertyInfo? TryGetDestinationProperty(PropertyInfo property, PropertyInfo[] destFields)
    {
        foreach (var destField in destFields)
        {
            if (destField.Name == property.Name && destField.PropertyType == property.PropertyType)
                return destField;
        }

        return null;
    }

    public void EnrichProperties(object from, object to)
    {
        Enrich(from, to);
    }
}
