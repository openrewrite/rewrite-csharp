using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Rewrite.RoslynRecipes.Helpers;

/// <summary>
/// Rewriter that, for EVERY token, strips all whitespace/newline trivia
/// and guarantees a single elastic marker on both leading and trailing sides.
/// Non-whitespace trivia (comments, directives, regions) are preserved.
/// </summary>
internal sealed class ElasticizeAllTokensRewriter : CSharpSyntaxRewriter
{
    public ElasticizeAllTokensRewriter() : base(visitIntoStructuredTrivia: true)
    {
    }

    public override SyntaxToken VisitToken(SyntaxToken token)
    {
        // Visit into children/structured trivia first
        token = base.VisitToken(token);

        var leading = MakeLeadingElastic(token.LeadingTrivia);
        var trailing = MakeTrailingElastic(token.TrailingTrivia);

        return token.WithLeadingTrivia(leading)
            .WithTrailingTrivia(trailing);
    }

    private static SyntaxTriviaList MakeLeadingElastic(SyntaxTriviaList trivia)
    {
        // Keep non-whitespace trivia; separate directives so they stay first
        var directives = new System.Collections.Generic.List<SyntaxTrivia>();
        var others = new System.Collections.Generic.List<SyntaxTrivia>();

        foreach (var tv in trivia)
        {
            if (IsWhitespaceOrEol(tv)) continue;
            if (tv.IsDirective) directives.Add(tv);
            else others.Add(tv);
        }

        // Ensure a single elastic marker is present even if there was no trivia
        // Order: directives (must remain first), then elastic, then other trivia (e.g., comments)
        var list = TriviaList();
        if (directives.Count > 0) list = list.AddRange(directives);
        list = list.Add(ElasticMarker);
        if (others.Count > 0) list = list.AddRange(others);
        return list;
    }

    private static SyntaxTriviaList MakeTrailingElastic(SyntaxTriviaList trivia)
    {
        // Keep non-whitespace trivia; keep directives but place them after the elastic marker
        var directives = new System.Collections.Generic.List<SyntaxTrivia>();
        var others = new System.Collections.Generic.List<SyntaxTrivia>();

        foreach (var tv in trivia)
        {
            if (IsWhitespaceOrEol(tv)) continue;
            if (tv.IsDirective) directives.Add(tv);
            else others.Add(tv);
        }

        // Order: other trivia (e.g., comments), then elastic, then directives (rare on trailing, but preserved)
        var list = TriviaList();
        if (others.Count > 0) list = list.AddRange(others);
        list = list.Add(ElasticMarker);
        if (directives.Count > 0) list = list.AddRange(directives);
        return list;
    }

    private static bool IsWhitespaceOrEol(SyntaxTrivia trivia)
        => trivia.Kind() is SyntaxKind.WhitespaceTrivia
            or SyntaxKind.EndOfLineTrivia;
}