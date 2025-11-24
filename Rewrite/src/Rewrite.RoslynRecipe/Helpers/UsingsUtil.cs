using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace Rewrite.RoslynRecipe.Helpers
{
    /// <summary>
    /// Helper class for managing using directives and ensuring symbols are available at usage sites.
    /// </summary>
    public static class UsingsUtil
    {
        
        /// <summary>
        /// Adds a using directive for the specified type if it's not already available at the usage site.
        /// </summary>
        /// <param name="document">The document to potentially add the using directive to.</param>
        /// <param name="root">The syntax root of the document.</param>
        /// <param name="semanticModel">The semantic model for the document.</param>
        /// <param name="usageSite">The syntax node where the type will be used.</param>
        /// <param name="fullTypeName">The fully qualified type name (e.g., "System.Threading.Tasks.Task").</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>A tuple containing the potentially updated document and root.</returns>
        public static async Task<(Document document, SyntaxNode root)> MaybeAddUsingAsync(
            Document document,
            SyntaxNode root,
            SemanticModel semanticModel,
            SyntaxNode usageSite,
            string fullTypeName,
            CancellationToken cancellationToken)
        {
            // Extract the simple type name from the full type name
            var lastDotIndex = fullTypeName.LastIndexOf('.');
            if (lastDotIndex <= 0)
            {
                // No namespace, nothing to add
                return (document, root);
            }

            var typeName = fullTypeName.Substring(lastDotIndex + 1);
            var namespaceName = fullTypeName.Substring(0, lastDotIndex);

            // Check if the type is already available at the usage site
            if (IsTypeAvailable(semanticModel, usageSite, typeName, fullTypeName))
            {
                return (document, root);
            }

            // Add the using directive
            return await AddUsingDirectiveAsync(document, root, namespaceName, cancellationToken);
        }

        /// <summary>
        /// Removes a using directive for the specified namespace if it exists in the document.
        /// </summary>
        /// <param name="document">The document to potentially remove the using directive from.</param>
        /// <param name="namespaceName">The namespace to remove (e.g., "System.Threading.Tasks").</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>The document with the using directive removed if it was present.</returns>
        public static async Task<Document> MaybeRemoveUsingAsync(
            Document document,
            string namespaceName,
            CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            if (root == null)
                return document;

            var compilationUnit = root as CompilationUnitSyntax;
            if (compilationUnit == null)
                return document;

            // Find and remove the using directive if it exists
            var usingToRemove = compilationUnit.Usings
                .FirstOrDefault(u => u.Name?.ToString() == namespaceName);

            if (usingToRemove == null)
                return document;

            // Remove the using directive
            var newUsings = compilationUnit.Usings.Remove(usingToRemove);
            var newCompilationUnit = compilationUnit.WithUsings(newUsings);

            // Return the updated document
            return document.WithSyntaxRoot(newCompilationUnit);
        }

        /// <summary>
        /// Checks if a type is available (accessible) at a given usage site.
        /// </summary>
        /// <param name="semanticModel">The semantic model for the document.</param>
        /// <param name="usageSite">The syntax node where the type will be used.</param>
        /// <param name="typeName">The simple name of the type.</param>
        /// <param name="fullTypeName">The fully qualified type name.</param>
        /// <returns>True if the type is available at the usage site; otherwise, false.</returns>
        private static bool IsTypeAvailable(SemanticModel semanticModel, SyntaxNode usageSite, string typeName, string fullTypeName)
        {
            // Get the position in the syntax tree
            var position = usageSite.SpanStart;

            // Look up all symbols with the given name that are accessible at this position
            var symbols = semanticModel.LookupSymbols(position, name: typeName);

            // Check if any of the found symbols match our expected type
            foreach (var symbol in symbols)
            {
                if (symbol is INamedTypeSymbol typeSymbol)
                {
                    var symbolFullName = typeSymbol.ToDisplayString();
                    if (symbolFullName == fullTypeName)
                    {
                        return true;
                    }
                }
            }

            // Also check if the type can be bound directly (in case it's in a using static or global using)
            var typeInfo = semanticModel.GetSpeculativeTypeInfo(
                position,
                SyntaxFactory.ParseExpression(typeName),
                SpeculativeBindingOption.BindAsTypeOrNamespace);

            if (typeInfo.Type != null && typeInfo.Type.ToDisplayString() == fullTypeName)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a using directive to the document if it doesn't already exist.
        /// </summary>
        /// <param name="document">The document to add the using directive to.</param>
        /// <param name="root">The syntax root of the document.</param>
        /// <param name="namespaceName">The namespace to import.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>A tuple containing the updated document and root.</returns>
        private static async Task<(Document document, SyntaxNode root)> AddUsingDirectiveAsync(
            Document document,
            SyntaxNode root,
            string namespaceName,
            CancellationToken cancellationToken, SyntaxAnnotation? formatAnnotation = null)
        {
            var compilationUnit = root as CompilationUnitSyntax;
            if (compilationUnit == null)
            {
                return (document, root);
            }

            // Check if the using directive already exists
            var existingUsings = compilationUnit.Usings;
            var hasUsing = existingUsings.Any(u => u.Name?.ToString() == namespaceName);

            if (hasUsing)
            {
                return (document, root);
            }

            // Create the new using directive
            var newUsing = SyntaxFactory.UsingDirective(
                SyntaxFactory.ParseName(namespaceName))
                .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);

            if (formatAnnotation != null)
            {
                newUsing = newUsing.WithAdditionalAnnotations(formatAnnotation);
            }

            // Find the right place to insert the using directive
            CompilationUnitSyntax newCompilationUnit;

            if (existingUsings.Count > 0)
            {
                // Find where to insert based on alphabetical order
                int insertIndex = 0;
                foreach (var existingUsing in existingUsings)
                {
                    var existingName = existingUsing.Name?.ToString() ?? "";
                    if (string.Compare(namespaceName, existingName, StringComparison.Ordinal) < 0)
                    {
                        break;
                    }
                    insertIndex++;
                }

                // Insert the using directive at the appropriate position
                var newUsings = existingUsings.Insert(insertIndex, newUsing);
                newCompilationUnit = compilationUnit.WithUsings(newUsings);
            }
            else
            {
                newUsing = newUsing.WithTrailingTrivia(newUsing.GetTrailingTrivia().Add(SyntaxFactory.CarriageReturnLineFeed)); // extra line break if we're adding first using 
                // No existing usings, add as the first using
                newCompilationUnit = compilationUnit.WithUsings(
                    SyntaxFactory.SingletonList(newUsing));
            }

            // Create a new document with the updated syntax tree
            var newDocument = document.WithSyntaxRoot(newCompilationUnit);
            var newRoot = await newDocument.GetSyntaxRootAsync(cancellationToken);

            return (newDocument, newRoot!);
        }

    }
}