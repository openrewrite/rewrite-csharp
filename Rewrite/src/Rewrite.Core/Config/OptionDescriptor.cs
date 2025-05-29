using System.ComponentModel;
using System.Reflection;

namespace Rewrite.Core.Config;

#if Analyzer
internal
#else
[PublicAPI]
public 
#endif
class OptionDescriptor
{
    public required string Name { get; init; }
    public required string Type { get; init; }
    public string? DisplayName { get;init; }
    public string? Description { get;init; }
    public string? Example { get;init; }
    public bool Required { get;init; }
    public override string ToString() => $"{Name}: {Description}";
#if !Analyzer
    public static List<OptionDescriptor> FromRecipeType<TRecipe>() where TRecipe : Recipe
    {
        var recipeType = typeof(TRecipe);
        var properties = recipeType
            .GetProperties()
            .Where(x => x.DeclaringType == recipeType)
            .ToList();

        var result = properties.Select(prop =>
        {
            var displayName = prop.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
            var description = prop.GetCustomAttribute<DescriptionAttribute>()?.Description;
            return new OptionDescriptor()
            {
                Name = prop.Name,
                DisplayName = displayName,
                Description = description,
                Type = prop.PropertyType.AssemblyQualifiedName!
            };
        }).ToList();
        return result;
    }
    
#endif
}