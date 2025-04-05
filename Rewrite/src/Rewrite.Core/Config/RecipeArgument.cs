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
}