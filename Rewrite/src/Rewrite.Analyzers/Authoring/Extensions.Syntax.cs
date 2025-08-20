using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace Rewrite.Analyzers.Authoring;

public static partial class Extensions
{
    public static bool InheritsFrom(this INamedTypeSymbol symbol, string type)
    {
        var current = symbol.BaseType;
        while (current != null)
        {
            if (current.Name == type)
                return true;
            current = current.BaseType;
        }
        return false;
    }
    
    public static T? FindParent<T>(this SyntaxNode node) where T : class
    {
        var current = node;
        while(true)
        {
            current = current.Parent;
            if (current == null || current is T)
                return current as T;
        }
    }
    
    

    /// <summary>
    /// Generates a filename-safe string for a TypeDeclarationSyntax, including namespace, nesting, and generic arity.
    /// Example: Namespace.Outer`1.Inner`2.g.cs
    /// </summary>
    public static string GetInferredFilename(this TypeDeclarationSyntax typeDecl)
    {
        if (typeDecl == null)
            throw new ArgumentNullException(nameof(typeDecl));

        var segments = new List<string>();

        // Gather nested type chain
        var currentType = typeDecl;
        while (currentType != null)
        {
            var name = currentType.Identifier.Text;
            var arity = currentType.TypeParameterList?.Parameters.Count ?? 0;
            if (arity > 0)
                name += "`" + arity;

            segments.Insert(0, Sanitize(name));
            currentType = currentType.Parent as TypeDeclarationSyntax;
        }

        // Prepend namespace if available
        var ns = GetContainingNamespace(typeDecl);
        if (!string.IsNullOrEmpty(ns))
            segments.Insert(0, ns!); // make dot filename-safe if needed

        return string.Join(".", segments);
    }

    private static string? GetContainingNamespace(SyntaxNode? node)
    {
        while (node != null)
        {
            if (node is NamespaceDeclarationSyntax nds)
                return nds.Name.ToString();
            if (node is FileScopedNamespaceDeclarationSyntax fns)
                return fns.Name.ToString();
            node = node.Parent;
        }
        return null;
    }

    private static string Sanitize(string input)
    {
        var sb = new StringBuilder(input.Length);
        foreach (var c in input)
        {
            switch (c)
            {
                case '<':
                case '>':
                case ':':
                case '"':
                case '/':
                case '\\':
                case '|':
                case '?':
                case '*':
                    sb.Append('_');
                    break;
                default:
                    sb.Append(c);
                    break;
            }
        }
        return sb.ToString();
    }

    public static T PreservingParent<T>(this T node, Func<T, T> transformation) where T : SyntaxNode
    {
        var root = node.SyntaxTree.GetCompilationUnitRoot();
        var trackedRoot = root.TrackNodes(node);
        var trackedNode = trackedRoot.GetCurrentNode(node)!;
        // var originalInterfaceFullyQualifiedName = ParseTypeName(namedSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
        var mutatedVersion = transformation(trackedNode).NormalizeWhitespace();

        // reattach parent hierarchy
        var newRoot = trackedRoot.ReplaceNode(trackedNode, mutatedVersion);
        mutatedVersion = newRoot.GetCurrentNode(node)!;
        return mutatedVersion;
    }
    public static bool IsPartialTypeDeclaration(this SyntaxNode node) => node is TypeDeclarationSyntax cds && cds.Modifiers.Any(m => m.IsKind(PartialKeyword));

    public static int GetIndentationLevel(this SyntaxNode node, string ident = "    ")
    {
        var currentIdent = node.GetIndentation();

        if (string.IsNullOrEmpty(currentIdent) || string.IsNullOrEmpty(ident))
            return 0;

        int level = currentIdent.Length / ident.Length;

        // Account for cases where mixed spaces or tabs are used inconsistently
        // e.g., if ident is 4 spaces but indent is 6 spaces, we still floor it to 1
        return level;
    }
    public static string GetIndentation(this SyntaxNode node)
    {
        var leadingTrivia = node.GetLeadingTrivia();
    
        foreach (var trivia in leadingTrivia.Reverse())
        {
            if (trivia.IsKind(SyntaxKind.WhitespaceTrivia))
            {
                return trivia.ToFullString();
            }

            if (trivia.IsKind(SyntaxKind.EndOfLineTrivia))
            {
                // Once we cross a newline, we stop
                break;
            }
        }

        return string.Empty;
    }

    public static IncrementalValueProvider<TResult> Attach<TLeft, TRight, TResult>(
        this IncrementalValuesProvider<TLeft> left,
        IncrementalValueProvider<TRight> right,
        Func<TLeft, TRight, TResult> transform) where TLeft : notnull
    {
        throw new NotImplementedException();
    }
}