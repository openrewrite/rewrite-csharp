using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Operations;

namespace Rewrite.RoslynRecipe.Helpers
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