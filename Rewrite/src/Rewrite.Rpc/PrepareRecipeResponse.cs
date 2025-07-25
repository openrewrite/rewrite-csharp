using Rewrite.Core.Config;

namespace Rewrite.Rpc;

public class PrepareRecipeResponse
{
    /// <summary>
    /// The ID that the remote is using to refer to a specific instance of the recipe.
    /// </summary>
    public required string Id { get; set; } 
    public required RecipeDescriptor Options { get; set; }
    public required string EditVisitor { get; set; }
    public required string ScanVisitor { get; set; }
}