using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Serilog.Core;
using Serilog.Events;

namespace Rewrite.Tests;

/// <summary>
/// Makes complex objects be written by serilog as a formatted json block on a new line
/// </summary>
public class PrettyJsonDestructuringPolicy : IDestructuringPolicy
{
    public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, [NotNullWhen(true)] out LogEventPropertyValue? result)
    {
        // Optionally limit what types this applies to
        if (value == null || value is string || value.GetType().IsPrimitive)
        {
            result = null;
            return false;
        }

        var json = JsonConvert.SerializeObject(value, Formatting.Indented);
        result = new ScalarValue($"\n{json}");
        return true;
    }
}