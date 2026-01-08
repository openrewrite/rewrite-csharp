using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Formatting;

namespace Rewrite.RoslynRecipes.Helpers;

/// <summary>
/// Helper class for managing using directives and ensuring symbols are available at usage sites.
/// </summary>
public static class UsingsUtil
{
    public static void MaybeAddUsingAsync(
        this DocumentEditor editor,
        SemanticModel semanticModel,
        string namespaceName,
        CancellationToken cancellationToken)
    {
        var alreadyAvailable = semanticModel
            .GetImportScopes(0, cancellationToken)
            .SelectMany(x => x.Imports)
            .Select(x => x.NamespaceOrType)
            .Select(x => x.ToString())
            .Contains(namespaceName);
        if(alreadyAvailable)
            return;
        editor.ReplaceNode(editor.OriginalRoot, (current, sf) => 
            {
                var currentCompilationUnit = (CompilationUnitSyntax)current;
                
                var newUsings = AddUsingSorted(currentCompilationUnit.Usings, namespaceName);
                var newCompilationUnit = currentCompilationUnit.WithUsings(newUsings);
                return newCompilationUnit;
 
            });
            
    }

    private static SyntaxList<UsingDirectiveSyntax> AddUsingSorted(SyntaxList<UsingDirectiveSyntax> existingUsings, string namespaceName)
    {
        var newUsing = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(namespaceName))
            .NormalizeWhitespace()
            .FormatterAnnotated();
            // .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed)
            ;
        if (existingUsings.Select(x => x.Name?.ToString()).Contains(namespaceName))
            return existingUsings;
        SyntaxList<UsingDirectiveSyntax> result;
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

            // Check if we're inserting at the beginning and the first using has important trivia
            if (insertIndex == 0)
            {
                var firstUsing = existingUsings[0];
                var leadingTrivia = firstUsing.GetLeadingTrivia();

                // Check for non-whitespace trivia (like pragmas)
                // var nonWhitespaceTrivia = leadingTrivia.Where(t =>
                //     !t.IsKind(SyntaxKind.WhitespaceTrivia) &&
                //     !t.IsKind(SyntaxKind.EndOfLineTrivia)).ToList();

                if (leadingTrivia.Any())
                {
                    // Move the non-whitespace trivia to the new using
                    newUsing = newUsing.WithLeadingTrivia(leadingTrivia).WithAdditionalAnnotations(Formatter.Annotation);

                    // Remove the trivia from the old first using
                    var modifiedFirstUsing = firstUsing.WithoutLeadingTrivia();
                        // .WithLeadingTrivia(
                        // SyntaxFactory.TriviaList(SyntaxFactory.CarriageReturnLineFeed));

                    // Replace the first using with the modified version
                    existingUsings = existingUsings.Replace(firstUsing, modifiedFirstUsing);
                }
            }

            // Insert the using directive at the appropriate position
            result = existingUsings.Insert(insertIndex, newUsing);

        }
        else
        {
            newUsing = newUsing.WithTrailingTrivia(newUsing.GetTrailingTrivia()
                    .Add(SyntaxFactory.CarriageReturnLineFeed))  
                // extra line break if we're adding first using
                ;
            // No existing usings, add as the first using
            result = SyntaxFactory.SingletonList(newUsing);
        }

        return result;
    }
    
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
    public static async Task<Document> MaybeAddUsingAsync(
        Document document,
        SemanticModel semanticModel,
        SyntaxNode usageSite,
        string fullTypeName,
        CancellationToken cancellationToken)
    {

        var syntaxTree = await document.GetSyntaxTreeAsync(cancellationToken);
        var cu = syntaxTree!.GetCompilationUnitRoot();
        // Add the using directive
        var newCompilationUnit = await MaybeAddUsingAsync(cu, semanticModel, usageSite, fullTypeName, cancellationToken);
        var newDocument = document.WithSyntaxRoot(newCompilationUnit);
        return newDocument;
    }
    
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
    public static async Task<CompilationUnitSyntax> MaybeAddUsingAsync(
        CompilationUnitSyntax compilationUnitSyntax,
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
            return compilationUnitSyntax;
        }

        var typeName = fullTypeName.Substring(lastDotIndex + 1);
        var namespaceName = fullTypeName.Substring(0, lastDotIndex);

        // Check if the type is already available at the usage site
        if (IsTypeAvailable(semanticModel, usageSite, typeName, fullTypeName))
        {
            return compilationUnitSyntax;
        }

        // Add the using directive
        var newCompilationUnit = await AddUsingDirectiveAsync(compilationUnitSyntax,  namespaceName, cancellationToken);
        return newCompilationUnit;
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
    /// Removes a using directive for the specified namespace using a DocumentEditor.
    /// </summary>
    /// <param name="editor">The document editor to use for the modification.</param>
    /// <param name="namespaceName">The namespace to remove (e.g., "System.Threading.Tasks").</param>
    public static void MaybeRemoveUsing(
        this DocumentEditor editor,
        string namespaceName)
    {
        var compilationUnit = editor.OriginalRoot as CompilationUnitSyntax;
        if (compilationUnit == null)
            return;

        // Find the using directive to remove
        var usingToRemove = compilationUnit.Usings
            .FirstOrDefault(u => u.Name?.ToString() == namespaceName);

        if (usingToRemove == null)
            return;

        editor.RemoveNode(usingToRemove);
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
    private static async Task<CompilationUnitSyntax> AddUsingDirectiveAsync(
        CompilationUnitSyntax compilationUnit,
        string namespaceName,
        CancellationToken cancellationToken, SyntaxAnnotation? formatAnnotation = null)
    {

        // Check if the using directive already exists
        var existingUsings = compilationUnit.Usings;
        var hasUsing = existingUsings.Any(u => u.Name?.ToString() == namespaceName);

        if (hasUsing)
        {
            return compilationUnit;
        }

        var newUsings = AddUsingSorted(existingUsings, namespaceName);
        var newCompilationUnit = compilationUnit.WithUsings(newUsings);
        
        return newCompilationUnit;
    }

}