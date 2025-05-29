
namespace Rewrite.Core.Config;

public class RecipeStartInfo
{
    public required string Id { get; init; }
    public required TypeName TypeName { get; init; }
    public required RecipeKind Kind { get; init; }
    public required string DisplayName { get; init; }
    public required string Description { get; init; }
    
    private readonly Dictionary<string,RecipeArgument> _arguments = new();

    public IReadOnlyDictionary<string, RecipeArgument> Arguments
    {
        get => _arguments;
        internal init => _arguments = new Dictionary<string, RecipeArgument>(value);
    }

    internal RecipeStartInfo()
    {
       
    }

    public RecipeStartInfo WithOption(string propertyName, object? value)
    {
        if (!_arguments.TryGetValue(propertyName, out var argument))
        {
            throw new ArgumentException($"Unknown property {propertyName}");
        }

        if (argument.Required && value == null)
        {
            throw new ArgumentException($"Property {propertyName} is required");
        }

        if (value != null && TypeName.Parse(argument.Type) != TypeName.Parse(value.GetType().AssemblyQualifiedName!))
        {
            throw new ArgumentException($"Cannot assign {value} of type {value.GetType()} to property of type {argument.Type}");
        }

        argument.Value = value;
        return this;
    }
    
    public override string ToString() => $"[{Id}] {DisplayName}";

}