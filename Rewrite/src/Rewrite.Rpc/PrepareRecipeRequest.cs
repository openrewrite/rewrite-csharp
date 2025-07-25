using Newtonsoft.Json.Linq;
using Rewrite.Core;

namespace Rewrite.Rpc;

public class PrepareRecipeRequest
{
    /// <summary>
    /// Recipe ID in URI format. Use <see cref="InstallableRecipe.ParseUri(string)"/> to extract.
    /// </summary>
    public required string Id { get; set; }
    /// <summary>
    /// Arguments for recipe options (property names / values)
    /// </summary>
    public required Dictionary<string, JToken> Options { get; set; }
}