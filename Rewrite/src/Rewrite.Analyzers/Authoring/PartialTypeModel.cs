using System.CodeDom.Compiler;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rewrite.Analyzers.Authoring;

[PublicAPI]
public abstract record PartialTypeModel
{
    internal const string ReplaceToken = "//~REPLACE";
    protected PartialTypeModel(TypeDeclarationSyntax partialType, string fileName)
    {
        // PartialType = partialType;
        FileName = fileName;
        
        var root = partialType.SyntaxTree.GetRoot();
        var hierarchy = partialType.AncestorsAndSelf().ToList();

        root = root.TrackNodes(hierarchy);
        var children = new SyntaxList<MemberDeclarationSyntax>();
        root = new Stripper().Visit(root);
        foreach(var node in hierarchy)
        {
            var currentNode = (MemberDeclarationSyntax)root.GetCurrentNode(node)!;
            var modifiedNode = currentNode;
            if(modifiedNode is TypeDeclarationSyntax typeDeclaration)
            {
                modifiedNode = typeDeclaration
                    // .WithBaseList(null)
                    .WithParameterList(null) // strip out primary constructor syntax
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

            IndentationContext.Increase();
        }
        partialType = root.GetCurrentNode(partialType)!;
        var strippedTypeDeclaration = partialType
            .WithOpenBraceToken(partialType.OpenBraceToken
                .WithTrailingTrivia(
                    SyntaxFactory.CarriageReturnLineFeed,
                    SyntaxFactory.Comment(ReplaceToken),
                    SyntaxFactory.CarriageReturnLineFeed))
            .WithCloseBraceToken(partialType.CloseBraceToken
                .WithLeadingTrivia(partialType.CloseBraceToken.LeadingTrivia.LastOrDefault(x => x.IsKind(SyntaxKind.WhitespaceTrivia))))
            ;
        root = root.ReplaceNode(partialType, strippedTypeDeclaration);


        var result = root.ToFullString();
        PartialDeclarationTemplate = result;
        IdentLevel = IndentationContext.CurrentLevel;
        IndentationContext.Reset();
    }

    public static (string Before, string After, int BodyIndentationLevel) GetPartialDeclaration(TypeDeclarationSyntax partialType)
    {
        var root = partialType.SyntaxTree.GetCompilationUnitRoot();
        var hierarchy = partialType.AncestorsAndSelf().ToList();

        root = root.TrackNodes(hierarchy);
        var children = new SyntaxList<MemberDeclarationSyntax>();
        root = (CompilationUnitSyntax)new Stripper().Visit(root);
        foreach(var node in hierarchy)
        {
            if (node is CompilationUnitSyntax)
            {
                root = root.WithMembers(children);
                break;
            }
            var currentNode = (MemberDeclarationSyntax)root.GetCurrentNode(node)!;
            var modifiedNode = currentNode;
            if(modifiedNode is TypeDeclarationSyntax typeDeclaration)
            {
                modifiedNode = typeDeclaration
                    // .WithBaseList(null)
                    .WithParameterList(null) // strip out primary constructor syntax
                    .WithMembers(children);
                root = root.ReplaceNode(currentNode, modifiedNode);
                children = new SyntaxList<MemberDeclarationSyntax>(modifiedNode);
            }
            if(modifiedNode is BaseNamespaceDeclarationSyntax nsDeclaration)
            {
                modifiedNode = nsDeclaration.WithMembers(children);
                root = root.ReplaceNode(currentNode, modifiedNode);
                children = SyntaxFactory.SingletonList(modifiedNode);
                // break;
            }

            IndentationContext.Increase();
        }
        partialType = root.GetCurrentNode(partialType)!;
        var strippedTypeDeclaration = partialType
            .WithOpenBraceToken(partialType.OpenBraceToken
                .WithTrailingTrivia(SyntaxFactory.Comment(ReplaceToken)))
            .WithCloseBraceToken(partialType.CloseBraceToken.WithoutTrivia())
            // .WithCloseBraceToken(partialType.CloseBraceToken
            //     .WithLeadingTrivia(SyntaxFactory.CarriageReturnLineFeed));
            ;
        root = root.ReplaceNode(partialType, strippedTypeDeclaration);
        var parts = root.ToFullString().Split(new[] { ReplaceToken }, StringSplitOptions.None);
        var before = parts[0];
        var after = parts[1];
        var bodyIndentationLevel = partialType.GetIndentationLevel() + 1;
        return (before.Trim(), after.Trim(), bodyIndentationLevel);
    }

    public static string RenderPartialBody(TypeDeclarationSyntax partialType, params MemberDeclarationSyntax[] body)
    {
        var sb =  new StringBuilder();
        foreach (var member in body)
        {
            sb.AppendLine(member.ToFullString());
        }

        return RenderPartialBody(partialType, sb.ToString());
    }

    public static string RenderPartialBody(TypeDeclarationSyntax partialType, string body)
    {
        var sb = new StringBuilder();
        using var writer = new IndentedTextWriter(new StringWriter(sb), "    ");
        var partialDeclaration = GetPartialDeclaration(partialType);
        writer.WriteLine("#nullable enable");
        writer.WriteLine(partialDeclaration.Before);
        writer.Indent = partialDeclaration.BodyIndentationLevel;
        writer.WriteLine(body);
        writer.Indent--;
        writer.Write(partialDeclaration.After);
        return sb.ToString();
    }
    
    public string PartialDeclarationTemplate { get; private set; }
    public int IdentLevel { get; private set; }
    public string FileName { get; }
    

    public void GenerateDeclarationTemplate(TypeDeclarationSyntax typeDeclarationSyntax)
    {
        
    }
    
    
    /// <summary>
    /// Strips out things that shouldn't be redeclared, like attributes and pragma trivia
    /// </summary>
    private class Stripper : CSharpSyntaxRewriter
    {
        public override SyntaxNode? VisitAttributeList(AttributeListSyntax node)
        {
            return null;
        }

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