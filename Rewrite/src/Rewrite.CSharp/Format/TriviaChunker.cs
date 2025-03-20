using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Rewrite.RewriteCSharp.Format;

/// <summary>
/// Computes changes to Whitespace c# code using c# token based chunking
/// </summary>
internal class TriviaChunker
{


    /// <summary>
    /// Splits a C# code string into chunks based on syntax tokens and their trivia.
    /// </summary>
    /// <param name="csharpCode">The C# code to chunk.</param>
    /// <returns>
    ///     An array of string chunks representing tokens and their surrounding trivia.
    ///     The elements will be in alternating order ( trivia / token / trivia ... / token / trivia )
    /// </returns>
    public string[] Chunk(string csharpCode)
    {
        var result = new List<string>();

        // Parse text into syntax tree and get first token
        var root = CSharpSyntaxTree.ParseText(csharpCode).GetCompilationUnitRoot();
        var token = root.GetFirstToken();
        SyntaxToken prevToken = SyntaxFactory.Token(SyntaxKind.None);

        // Process each token and collect trivia
        do
        {
            // Combine trailing trivia from previous token with leading trivia of current token
            var trivia = $"{prevToken.TrailingTrivia.ToFullString()}{token.LeadingTrivia.ToFullString()}";
            result.Add(trivia);

            // Add token text and prepare for next iteration
            result.Add(token.ToString());
            prevToken = token;
            token = token.GetNextToken();
        } while (!token.IsKind(SyntaxKind.None));

        // Add final trailing trivia
        result.Add(prevToken.TrailingTrivia.ToFullString());

        return result.ToArray();
    }
}
