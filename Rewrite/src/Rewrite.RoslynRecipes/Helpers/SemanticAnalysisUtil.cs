using Microsoft.CodeAnalysis;

namespace Rewrite.RoslynRecipes.Helpers
{
    /// <summary>
    /// Provides utilities for semantic analysis
    /// </summary>
    public static class SemanticAnalysisUtil
    {

        public static bool IsSymbolOneOf(this SyntaxNode node, SemanticModel semanticModel, params IEnumerable<string> symbolNames)
        {
            var symbolInfo = semanticModel.GetSymbolInfo(node);
            var symbol = symbolInfo.Symbol;
            if (symbol == null)
            {
                return false;
            }

            // Use GetDocumentationCommentId for precise identification when available
            var symbolKey = symbol.GetDocumentationCommentId();
            if(symbolKey == null)
                return false;

            if (symbolNames.Contains(symbolKey))
            {
                return true;
            }

            return false;
        }
        
    }
}