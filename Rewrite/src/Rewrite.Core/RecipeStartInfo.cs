
namespace Rewrite.Core.Config;

public class RecipeStartInfo
{
    private readonly Dictionary<string,RecipeArgument> _arguments;
    
    public IReadOnlyDictionary<string, RecipeArgument> Arguments { get; private set; }
    internal RecipeStartInfo(IReadOnlyList<OptionDescriptor> options)
    {
        _arguments = options.Select(x => new RecipeArgument
        {
            Name = x.Name,
            Description = x.Description,
            Type = x.Type,
            DisplayName = x.DisplayName,
            Example = x.Example,
            Required = x.Required,
        }).ToDictionary(x => x.Name, x => x);
        Arguments = _arguments.AsReadOnly();

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
}