using Newtonsoft.Json;
using Rewrite.Core.Config;
using StreamJsonRpc;

namespace Rewrite.Rpc;

public interface ILanguageServer
{
    /// <summary>
    /// Parses solution and returns list of IDs of parsed LSTs
    /// </summary>
    public Task<string[]> Parse(ParseRequest request);
    public Task<VisitResponse> Visit(VisitRequest request);
    public Task<string> Print(PrintRequest request);
    public Task<PrepareRecipeResponse> PrepareRecipe(PrepareRecipeRequest request);
    /// <summary>
    /// Generates new files as part of the recipe
    /// </summary>
    /// <param name="request"></param>
    /// <returns>A list of IDs for remote objects</returns>
    public Task<string[]> Generate(GenerateRequest request);

    public Task<List<RecipeDescriptor>> GetRecipes();
    public Task<List<RpcObjectData>> GetObject(GetObjectRequest request);
}