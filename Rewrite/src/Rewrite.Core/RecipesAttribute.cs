namespace Rewrite.Core;


/// <summary>
/// Must be put on all assemblies containing recipies
/// </summary>
/// <example>
///  [assembly: HasRecipesAttribute]
/// </example>
[AttributeUsage(AttributeTargets.Assembly)]
public class RecipesAttribute(params string[] recipeDescriptorsJson) : Attribute
{
    public string[] RecipeDescriptorsJson { get; set; } = recipeDescriptorsJson;
}

