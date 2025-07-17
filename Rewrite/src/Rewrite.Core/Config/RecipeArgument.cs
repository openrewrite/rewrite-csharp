namespace Rewrite.Core.Config;

#if Analyzer
internal
#else
[PublicAPI]
public 
#endif
class RecipeArgument : OptionDescriptor
{
    internal RecipeArgument()
    {
    }

    public object? Value { get; internal set; }

    private Type _type = null!;
    private string _typeName = null!;

    public override required string Type
    {
        get => _typeName;
        init
        {
            _typeName = value;
            var type = value switch
            {
                "string" => typeof(string),
                "int" => typeof(int),
                "long" => typeof(long),
                "bool" => typeof(bool),
                "float" => typeof(float),
                "double" => typeof(double),
                _ => System.Type.GetType(value)
            }  ?? throw new TypeLoadException($"Type {value} cannot be loaded");
            _type = type;
        }
    }

    public Type GetArgumentType() => _type;

    public override string ToString() => $"{Name}: {Value}";

}