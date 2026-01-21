using Microsoft.CodeAnalysis;

namespace Rewrite.RoslynRecipes.Helpers;

public static class LocationExtensions
{
    extension(Location location)
    {
        public async Task<SyntaxNode> GetNodeAsync(CancellationToken cancellationToken)
        {
            if (location.SourceTree == null)
                throw new InvalidOperationException("Source Tree is null");
            var root = await location.SourceTree.GetRootAsync(cancellationToken);
            var node = root.FindNode(location.SourceSpan);
            return node;
        }
    }
}