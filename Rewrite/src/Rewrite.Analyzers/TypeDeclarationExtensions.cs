using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rewrite.Analyzers;

public static class TypeDeclarationExtensions
{
    private const string ReplaceToken = "//~REPLACE";
    public static string RenderPartial(this TypeDeclarationSyntax typeDeclarationSyntax, Func<string> body)
    {
        var root = typeDeclarationSyntax.SyntaxTree.GetRoot();
        var heirarchy = typeDeclarationSyntax.AncestorsAndSelf().ToList();

        root = root.TrackNodes(heirarchy);
        var children = new SyntaxList<MemberDeclarationSyntax>();
        root = new Stripper().Visit(root)!;
        foreach(var node in heirarchy)
        {
            var currentNode = (MemberDeclarationSyntax)root.GetCurrentNode(node)!;
            // var modifiedNode = currentNode.WithoutTrivia().WithAttributeLists([]);
            var modifiedNode = currentNode;
            if(modifiedNode is TypeDeclarationSyntax typeDeclaration)
            {
                modifiedNode = typeDeclaration
                    .WithBaseList(null)
                    .WithParameterList(null)
                    .WithMembers(children);
                root = root.ReplaceNode(currentNode, modifiedNode);
                children = new SyntaxList<MemberDeclarationSyntax>(modifiedNode);
            }
            if(modifiedNode is BaseNamespaceDeclarationSyntax nsDeclaration)
            {
                modifiedNode = nsDeclaration.WithMembers(children);
                root = root.ReplaceNode(currentNode, modifiedNode);
                break;
            }
        }
        typeDeclarationSyntax = root.GetCurrentNode(typeDeclarationSyntax)!;
        root = root.ReplaceNode(typeDeclarationSyntax, typeDeclarationSyntax.WithOpenBraceToken(typeDeclarationSyntax.OpenBraceToken.WithTrailingTrivia(
            SyntaxFactory.CarriageReturnLineFeed,
            SyntaxFactory.Comment(ReplaceToken),
            SyntaxFactory.CarriageReturnLineFeed)));


        var result = root.ToFullString();
        result = result.Replace(ReplaceToken, body());
        result = $"#nullable enable\n{result}";
        return result;
    }

    private class Stripper : CSharpSyntaxRewriter
    {
        public override SyntaxNode? VisitAttributeList(AttributeListSyntax node)
        {
            return null;
        }

        // public override SyntaxNode? Visit(SyntaxNode? node)
        // {
        //     if (node == null) return node;
        //     node = node.WithTrailingTrivia(OnlyWhitespaceTrivia(node.GetTrailingTrivia()));
        //     node = node.WithLeadingTrivia(OnlyWhitespaceTrivia(node.GetLeadingTrivia()));
        //     return base.Visit(node);
        // }

        public override SyntaxToken VisitToken(SyntaxToken token)
        {
            token = token.WithTrailingTrivia(OnlyWhitespaceTrivia(token.TrailingTrivia));
            token = token.WithLeadingTrivia(OnlyWhitespaceTrivia(token.LeadingTrivia));
            return token;
        }

        private SyntaxTriviaList OnlyWhitespaceTrivia(SyntaxTriviaList? trivia)
        {
            trivia ??= new SyntaxTriviaList();
            return new SyntaxTriviaList(trivia.Value.Where(x => x.IsKind(SyntaxKind.EndOfLineTrivia) || x.IsKind(SyntaxKind.WhitespaceTrivia)));
        }
    }
}
