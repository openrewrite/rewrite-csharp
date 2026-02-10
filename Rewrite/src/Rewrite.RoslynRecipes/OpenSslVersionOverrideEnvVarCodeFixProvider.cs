using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Rewrite.RoslynRecipes.Helpers;

namespace Rewrite.RoslynRecipes
{
    /// <summary>
    /// Provides a code fix that replaces the old environment variable name
    /// <c>CLR_OPENSSL_VERSION_OVERRIDE</c> with <c>DOTNET_OPENSSL_VERSION_OVERRIDE</c>
    /// in calls to <see cref="System.Environment.GetEnvironmentVariable(string)"/> and
    /// <see cref="System.Environment.SetEnvironmentVariable(string, string?)"/>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://learn.microsoft.com/en-us/dotnet/core/compatibility/cryptography/10.0/version-override">
    /// CLR_OPENSSL_VERSION_OVERRIDE renamed documentation</see> for details.
    /// </remarks>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(OpenSslVersionOverrideEnvVarCodeFixProvider)), Shared]
    public sealed class OpenSslVersionOverrideEnvVarCodeFixProvider : CodeFixProvider
    {
        private const string Title = "Replace with 'DOTNET_OPENSSL_VERSION_OVERRIDE'";

        /// <summary>
        /// Gets the diagnostic IDs that this code fix provider can handle.
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds { get; } =
            ImmutableArray.Create(OpenSslVersionOverrideEnvVarAnalyzer.DiagnosticId);

        /// <summary>
        /// Gets the fix all provider for batch fixing multiple instances.
        /// </summary>
        /// <returns>A batch fix all provider that can fix multiple instances at once.</returns>
        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        /// <summary>
        /// Registers code fixes for the specified context containing diagnostics
        /// about the old CLR_OPENSSL_VERSION_OVERRIDE environment variable name.
        /// </summary>
        /// <param name="context">The code fix context containing the diagnostics to fix.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root == null) return;

            foreach (var diagnostic in context.Diagnostics)
            {
                var diagnosticSpan = diagnostic.Location.SourceSpan;
                var node = root.FindNode(diagnosticSpan, getInnermostNodeForTie: true);

                if (node is not LiteralExpressionSyntax literal)
                    continue;

                context.RegisterCodeFix(
                    CodeAction.Create(
                        title: Title,
                        createChangedDocument: ct => ReplaceEnvVarNameAsync(context.Document, node, ct),
                        equivalenceKey: Title),
                    diagnostic);
            }
        }

        /// <summary>
        /// Replaces the old environment variable name string literal with the new one.
        /// </summary>
        /// <param name="document">The document containing the code to fix.</param>
        /// <param name="node">The syntax node representing the old string literal.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>A task that returns the updated document with the variable name replaced.</returns>
        private static async Task<Document> ReplaceEnvVarNameAsync(
            Document document,
            SyntaxNode node,
            CancellationToken cancellationToken)
        {
            var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);

            var newLiteral = SyntaxFactory.LiteralExpression(
                    SyntaxKind.StringLiteralExpression,
                    SyntaxFactory.Literal(OpenSslVersionOverrideEnvVarAnalyzer.NewEnvVarName))
                .FormatterAnnotated();

            editor.ReplaceNode(node, (current, gen) => newLiteral.WithTriviaFrom(current));

            return editor.GetChangedDocument();
        }
    }
}
