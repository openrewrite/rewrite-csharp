using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Simplification;

namespace Rewrite.RoslynRecipes.Helpers;

public static class DocumentEditorExtensions
{
    extension(DocumentEditor editor)
    {
        public void RenameSymbol(SemanticModel model, ISymbol symbol, string newName)
        {
            var compilationUnit = model.SyntaxTree.GetCompilationUnitRoot();
            var usages = model.FindLocations(symbol).ToList();
            foreach (var location in usages)
            {
                var originalToken = compilationUnit.FindToken(location.SourceSpan.Start);

                editor.ReplaceNode(originalToken.Parent!, (currentNode, generator) =>
                {
                    // return currentNode;
                    var oldTokens = currentNode.DescendantTokens().Where(x => x.IsKind(SyntaxKind.IdentifierToken) && x.Text == originalToken.Text);
                    var newNode = currentNode.ReplaceTokens(oldTokens, (original, current) => SyntaxFactory.Identifier(newName)
                        .WithTriviaFrom(current)
                        .WithAdditionalAnnotations(Formatter.Annotation));
                    return newNode;
                });
            }
        }

        // public void Remap(string oldSymbolDocumentationCommentId, string newSymbolDocumentationCommentId, SyntaxNode? scope = null)
        // {
        //     var model = editor.SemanticModel;
        //     var oldSymbol = DocumentationCommentId.GetFirstSymbolForDeclarationId(oldSymbolDocumentationCommentId, editor.SemanticModel.Compilation) 
        //                     ?? throw new InvalidOperationException($"Unable to resolve {oldSymbolDocumentationCommentId}");
        //     var newSymbol = DocumentationCommentId.GetFirstSymbolForDeclarationId(newSymbolDocumentationCommentId, editor.SemanticModel.Compilation)
        //                     ?? throw new InvalidOperationException($"Unable to resolve {newSymbolDocumentationCommentId}");
        //     var usages = model.FindLocations(oldSymbol, scope).ToList();
        //     if (!usages.Any())
        //     {
        //         return;
        //     }
        //     var compilationUnit = model.SyntaxTree.GetCompilationUnitRoot();
        //
        //     foreach (var location in usages)
        //     {
        //         
        //         var oldTypeNameNode = compilationUnit.FindNode(location.SourceSpan);
        //         var newSymbolQualifiedString = newSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        //         var newNode = newSymbol.Kind switch
        //         {
        //             SymbolKind.NamedType => SyntaxFactory.ParseTypeName(newSymbolQualifiedString),
        //             SymbolKind.Property or SymbolKind.Field => SyntaxFactory.ParseName(newSymbolQualifiedString),
        //             _ => throw new InvalidOperationException($"Not supported")
        //         };
        //         newNode = newNode
        //             .WithTriviaFrom(oldTypeNameNode)
        //             .WithAdditionalAnnotations(
        //                 Formatter.Annotation, 
        //                 Simplifier.Annotation, 
        //                 Simplifier.AddImportsAnnotation, 
        //                 SymbolAnnotation.Create(newSymbol));
        //
        //         editor.ReplaceNode(oldTypeNameNode, (x, y) => newNode);
        //
        //     }
        //     
        // }

        public void ReplaceType(ITypeSymbol oldType, ITypeSymbol newType, SyntaxNode? scope = null)
        {
            var model = editor.SemanticModel;
            var usages = model.FindLocations(oldType, scope).ToList();
            if (!usages.Any())
            {
                return;
            }
            //var currentCu = (CompilationUnitSyntax)await editor.GetChangedDocument().GetSyntaxRootAsync();
            //var currentNamespaces = currentCu.Usings.Select(x => x.Name.ToString()).ToHashSet();
            // var willAddUsing = false;

            var compilationUnit = model.SyntaxTree.GetCompilationUnitRoot();
            //var namespacesToAdd = 
            foreach (var location in usages)
            {

                var oldTypeNameNode = compilationUnit.FindNode(location.SourceSpan);


                // var newTypeNameNode = newType
                //     .ToIdentifierName()
                //     .WithTriviaFrom(oldTypeNameNode)
                //     .WithAdditionalAnnotations(Formatter.Annotation)
                //     .WithRequiredNamespace(newType.ContainingNamespace.ToString());
                //
                var newTypeNameNode = SyntaxFactory.ParseTypeName(newType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))
                        .WithTriviaFrom(oldTypeNameNode)
                        .WithAdditionalAnnotations(Formatter.Annotation, Simplifier.Annotation, Simplifier.AddImportsAnnotation, SymbolAnnotation.Create(newType))
                    // .WithRequiredNamespace(newType.ContainingNamespace.ToString())
                    ;


                editor.ReplaceNode(oldTypeNameNode, (x, y) => newTypeNameNode);

            }

            // if(willAddUsing)
            // {
            //     var namespaceName = newType.ContainingNamespace.ToString();
            //
            //
            //     //var existingUsings = editor.GetChangedDocument()
            //     editor.ReplaceNode(compilationUnit, (current, sf) => 
            //     {
            //         var currentCompilationUnit = (CompilationUnitSyntax)current;
            //         var requiredNamespaceName = newType.ContainingNamespace.ToDisplayString();
            //         var alreadyAdded = currentCompilationUnit.Usings.Select(x => x.NamespaceOrType.ToString()).Contains(requiredNamespaceName);
            //         if(alreadyAdded)
            //             return currentCompilationUnit;
            //     
            //         var newUsing = SyntaxFactory.UsingDirective(
            //                 SyntaxFactory.ParseName(namespaceName))
            //             .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);
            //
            //         var existingUsings = currentCompilationUnit.Usings;
            //
            //         CompilationUnitSyntax newCompilationUnit;
            //     
            //         if (existingUsings.Count > 0)
            //         {
            //             // Find where to insert based on alphabetical order
            //             int insertIndex = 0;
            //             foreach (var existingUsing in existingUsings)
            //             {
            //                 var existingName = existingUsing.Name?.ToString() ?? "";
            //                 if (string.Compare(namespaceName, existingName, StringComparison.Ordinal) < 0)
            //                 {
            //                     break;
            //                 }
            //                 insertIndex++;
            //             }
            //
            //             // Check if we're inserting at the beginning and the first using has important trivia
            //             if (insertIndex == 0)
            //             {
            //                 var firstUsing = existingUsings[0];
            //                 var leadingTrivia = firstUsing.GetLeadingTrivia();
            //
            //                 // Check for non-whitespace trivia (like pragmas)
            //                 var nonWhitespaceTrivia = leadingTrivia.Where(t =>
            //                     !t.IsKind(SyntaxKind.WhitespaceTrivia) &&
            //                     !t.IsKind(SyntaxKind.EndOfLineTrivia)).ToList();
            //
            //                 if (nonWhitespaceTrivia.Any())
            //                 {
            //                     // Move the non-whitespace trivia to the new using
            //                     newUsing = newUsing.WithLeadingTrivia(leadingTrivia);
            //
            //                     // Remove the trivia from the old first using
            //                     var modifiedFirstUsing = firstUsing.WithLeadingTrivia(
            //                         SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed));
            //
            //                     // Replace the first using with the modified version
            //                     existingUsings = existingUsings.Replace(firstUsing, modifiedFirstUsing);
            //                 }
            //             }
            //
            //             // Insert the using directive at the appropriate position
            //             var newUsings = existingUsings.Insert(insertIndex, newUsing);
            //             newCompilationUnit = currentCompilationUnit
            //                     .WithUsings(newUsings)
            //                 ;
            //         }
            //         else
            //         {
            //             newUsing = newUsing.WithTrailingTrivia(newUsing.GetTrailingTrivia()
            //                     .Add(SyntaxFactory.CarriageReturnLineFeed))  
            //                 // extra line break if we're adding first using
            //                 ;
            //             // No existing usings, add as the first using
            //             newCompilationUnit = currentCompilationUnit.WithUsings(
            //                 SyntaxFactory.SingletonList(newUsing));
            //         }
            //
            //         return newCompilationUnit;
            //
            //     
            //     });
            //
            //     //editor.AddMember(compilationUnit, SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(newType.ContainingNamespace.ToDisplayString())));
            // }
        }

        // public async Task EnsureNamespaces(SemanticModel semanticModel, CancellationToken cancellationToken)
        // {
        //     var changedDocument = editor.GetChangedDocument();
        //     var newTree = (await changedDocument.GetSyntaxTreeAsync(cancellationToken))!;
        //     var newCompilationUnit = newTree.GetCompilationUnitRoot();
        //
        //     var requiredNamespaces = newCompilationUnit.GetRequiredNamespaces()
        //         .SelectMany(x => x.RequiredNamespaces)
        //         .Distinct()
        //         .ToList();
        //     foreach (var requiredNamespace in requiredNamespaces)
        //     {
        //         editor.MaybeAddUsingAsync(semanticModel, requiredNamespace, cancellationToken);
        //     }
        // }

        // public async Task<Document> GetChangedDocumentFormatted(CancellationToken cancellationToken)
        // {
        //     var document = editor.GetChangedDocument();
        //     document = await RemoveUnusedImports(document, cancellationToken);
        //     return document;
        // }

    }
}