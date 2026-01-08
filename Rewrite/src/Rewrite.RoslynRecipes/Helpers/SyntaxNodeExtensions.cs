using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;

namespace Rewrite.RoslynRecipes.Helpers;

public static class SyntaxNodeExtensions
{
    private const string RequiredNamespaceSyntaxAnnotationKind = "RequiredNamespace";
    extension<TNode>(TNode node) where TNode : SyntaxNode
    {
        /// <summary>
        /// Returns a copy of this node tree with all whitespace and newline trivia
        /// replaced by elastic markers (<see cref="SyntaxFactory.ElasticMarker"/>).
        /// Guarantees that every token has one elastic marker on both leading and trailing sides.
        /// This allows the code formatter to fully format the code to standards without trying to preserve
        /// any existing formatting
        /// </summary>
        public TNode DiscardFormatting()
        {
            var rewriter = new ElasticizeAllTokensRewriter();
            return (TNode)rewriter.Visit(node)!;
        }

        /// <summary>
        /// Returns a copy of this node tree with all whitespace and newline trivia
        /// replaced by elastic markers (<see cref="SyntaxFactory.ElasticMarker"/>).
        /// <br/>
        /// Annotates the node with <see cref="Formatter.Annotation"/> so that it can be
        /// selectively formatted by the formatter
        /// </summary>
        public TNode FormatterAnnotated() => node.DiscardFormatting().WithAdditionalAnnotations(Formatter.Annotation);
        
        public TNode WithRequiredNamespace(string @namespace) => node.WithAdditionalAnnotations(new SyntaxAnnotation(RequiredNamespaceSyntaxAnnotationKind, @namespace));

        public IEnumerable<(SyntaxNode Node, IEnumerable<string> RequiredNamespaces)> GetRequiredNamespaces() => node
            .GetAnnotatedNodes(RequiredNamespaceSyntaxAnnotationKind)
            .Select(x => (x, x.GetAnnotations(RequiredNamespaceSyntaxAnnotationKind)
                .Select(a => a.Data)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x!)));



    }
}