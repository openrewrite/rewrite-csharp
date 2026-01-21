using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Simplification;

namespace Rewrite.RoslynRecipes.Helpers;

public static class DocumentExtensions
{
    extension(Document document)
    {
        public async Task<Document> RemoveUnusedImports(CancellationToken cancellationToken)
        {
            var model = await document.GetSemanticModelAsync(cancellationToken) ?? throw new NullReferenceException("SemanticModel is null");
            var unusedReferences = model.Compilation.GetDiagnostics(cancellationToken)
                .Where(x => x.Id == "CS8019")
                .ToList();
            var newEditor = await DocumentEditor.CreateAsync(document, cancellationToken);
            var root = await document.GetSyntaxRootAsync(cancellationToken) ??  throw new NullReferenceException("SyntaxRoot is null");
            var remainingUsings = root.ChildNodes().OfType<UsingDirectiveSyntax>().Count();
            foreach (var unusedReferenceDiagnostic in unusedReferences)
            {
                var importNode = await unusedReferenceDiagnostic.Location.GetNodeAsync(cancellationToken);
                newEditor.RemoveNode(importNode);
                remainingUsings--;
            }
            var firstNonUsing = root.ChildNodes().SkipWhile(x => x is UsingDirectiveSyntax).Take(1).FirstOrDefault();
            if(remainingUsings == 0 && firstNonUsing != null && firstNonUsing.HasLeadingTrivia)
            {
                var newTrivia = firstNonUsing.GetLeadingTrivia().SkipWhile(x => x.IsKind(SyntaxKind.WhitespaceTrivia) || x.IsKind(SyntaxKind.EndOfLineTrivia));
                newEditor.ReplaceNode(firstNonUsing, firstNonUsing.WithLeadingTrivia(newTrivia));
            }
            return newEditor.GetChangedDocument();
        }
    
    }
    
    
}