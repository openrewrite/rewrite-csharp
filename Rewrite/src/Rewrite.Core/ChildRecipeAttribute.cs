namespace Rewrite.Core;

public class ChildRecipeAttribute(params string[] recipes) : Attribute
{
    public string[] Recipes { get; } = recipes;
}