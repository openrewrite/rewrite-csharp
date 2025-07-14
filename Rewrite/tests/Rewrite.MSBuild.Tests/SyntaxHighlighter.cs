#nullable enable
using System;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Spectre.Console;

namespace Rewrite.MSBuild.Tests;

public static class SyntaxHighlighter
{
    /// <summary>
    /// Renders C# code with syntax highlighting to the console using Spectre.Console,
    /// styled to resemble Rider dark theme.
    /// </summary>
    /// <param name="code">The C# source code to render.</param>
    public static void Render(string code)
    {
        AnsiConsole.Write(GetMarkup(code)); // Final newline
    }
    
    public static Markup GetMarkup(string code)
    {
        var tree = CSharpSyntaxTree.ParseText(code);
        var root = tree.GetRoot();
        var sb = new StringBuilder();
        var visitor = new Visitor(sb);
        visitor.Visit(root);
        return new Markup(sb.ToString());
    }

    private sealed class Visitor : CSharpSyntaxWalker
    {
        // private readonly IAnsiConsole _console;
        private StringBuilder _out = new();
        public Visitor(StringBuilder result)
            : base(SyntaxWalkerDepth.StructuredTrivia)
        {
            _out = result;
        }

        public override void VisitToken(SyntaxToken token)
        {
            WriteTrivia(token.LeadingTrivia);
            WriteTokenWithColor(token);
            WriteTrivia(token.TrailingTrivia);
        }

        private void WriteTrivia(SyntaxTriviaList triviaList)
        {
            foreach (var trivia in triviaList)
            {
                var text = trivia.ToFullString();
                string markup = trivia.Kind() switch
                {
                    SyntaxKind.SingleLineCommentTrivia or
                    SyntaxKind.MultiLineCommentTrivia => $"[italic grey]{Markup.Escape(text)}[/]",
                    SyntaxKind.WhitespaceTrivia or
                    SyntaxKind.EndOfLineTrivia => Markup.Escape(text),
                    _ => Markup.Escape(text)
                };

                _out.Append(markup);
            }
        }

        private void WriteTokenWithColor(SyntaxToken token)
        {
            var text = token.Text;

            string color = token.Kind() switch
            {
                SyntaxKind.StringLiteralToken => "orange1",
                SyntaxKind.CharacterLiteralToken => "green",
                SyntaxKind.NumericLiteralToken => "green",
                SyntaxKind.IdentifierToken => "cyan",
                _ when token.IsKeyword() => "blue bold",
                _ when SyntaxFacts.IsPunctuation(token.Kind()) => "silver",
                _ => "default"
            };

            string markup = $"[{color}]{Markup.Escape(text)}[/]";
            _out.Append(markup);
        }
        
    }
}
