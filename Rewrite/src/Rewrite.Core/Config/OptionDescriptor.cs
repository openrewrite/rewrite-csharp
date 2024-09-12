namespace Rewrite.Core.Config;

public class OptionDescriptor
{
    // Properties with public getters and private setters for immutability
    public string Name { get; }
    public string Type { get; }
    public string? DisplayName { get; }
    public string? Description { get; }
    public string? Example { get; }
    public List<string>? Valid { get; }
    public bool Required { get; }
    public object? Value { get; }

    // Constructor to initialize the properties
    public OptionDescriptor(
        string name,
        string type,
        string? displayName,
        string? description,
        string? example,
        List<string>? valid,
        bool required,
        object? value)
    {
        Name = name;
        Type = type;
        DisplayName = displayName;
        Description = description;
        Example = example;
        Valid = valid;
        Required = required;
        Value = value;
    }

    // Override Equals and GetHashCode to handle equality based on Name, Type, and Value
    public override bool Equals(object? obj)
    {
        if (obj is OptionDescriptor other)
        {
            return Name == other.Name &&
                   Type == other.Type &&
                   DisplayName == other.DisplayName &&
                   Description == other.Description &&
                   Example == other.Example &&
                   Valid.SafeSequenceEqual(other.Valid) &&
                   Required == other.Required &&
                   Equals(Value, other.Value);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Type, DisplayName, Description, Example, Valid, Required, Value);
    }
}
