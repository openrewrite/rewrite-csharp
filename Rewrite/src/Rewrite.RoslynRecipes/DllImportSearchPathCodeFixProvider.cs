using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Formatting;
using Rewrite.RoslynRecipes.Helpers;

namespace Rewrite.RoslynRecipes
{
    /// <summary>
    /// Provides code fixes for <c>DllImportSearchPath.AssemblyDirectory</c> used as the sole search
    /// flag, which no longer falls back to OS default search in .NET 10.
    /// </summary>
    /// <remarks>
    /// For <c>DefaultDllImportSearchPaths</c> attributes, the fix removes the attribute entirely
    /// so the runtime uses its default search behavior (assembly directory first, then OS defaults).
    /// For <c>NativeLibrary.Load</c>/<c>TryLoad</c> calls, the fix replaces the
    /// <c>DllImportSearchPath.AssemblyDirectory</c> argument with <c>null</c>.
    /// See <see href="https://learn.microsoft.com/en-us/dotnet/core/compatibility/interop/10.0/search-assembly-directory">
    /// DllImportSearchPath.AssemblyDirectory behavior change documentation</see> for details.
    /// </remarks>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DllImportSearchPathCodeFixProvider)),
     Shared]
    public sealed class DllImportSearchPathCodeFixProvider : CodeFixProvider
    {
        private const string RemoveAttributeTitle = "Remove DefaultDllImportSearchPaths attribute";
        private const string ReplaceWithNullTitle = "Replace with null to use default search behavior";

        /// <summary>
        /// Gets the diagnostic IDs that this code fix provider can handle.
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds { get; } =
            ImmutableArray.Create(
                DllImportSearchPathAnalyzer.AttributeDiagnosticId,
                DllImportSearchPathAnalyzer.MethodDiagnosticId);

        /// <summary>
        /// Gets the fix all provider for batch fixing multiple instances.
        /// </summary>
        /// <returns>A batch fix all provider that can fix multiple instances at once.</returns>
        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        /// <summary>
        /// Registers code fixes for diagnostics about <c>DllImportSearchPath.AssemblyDirectory</c>
        /// used as the sole search flag.
        /// </summary>
        /// <param name="context">The code fix context containing the diagnostics to fix.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken)
                .ConfigureAwait(false);
            if (root is null) return;

            foreach (var diagnostic in context.Diagnostics)
            {
                var diagnosticSpan = diagnostic.Location.SourceSpan;
                var node = root.FindNode(diagnosticSpan, getInnermostNodeForTie: true);

                if (diagnostic.Id == DllImportSearchPathAnalyzer.AttributeDiagnosticId)
                {
                    if (node is not AttributeSyntax attribute)
                        continue;

                    context.RegisterCodeFix(
                        CodeAction.Create(
                            title: RemoveAttributeTitle,
                            createChangedDocument: ct =>
                                RemoveAttributeAsync(context.Document, attribute, ct),
                            equivalenceKey: RemoveAttributeTitle),
                        diagnostic);
                }
                else if (diagnostic.Id == DllImportSearchPathAnalyzer.MethodDiagnosticId)
                {
                    if (node is not MemberAccessExpressionSyntax)
                        continue;

                    context.RegisterCodeFix(
                        CodeAction.Create(
                            title: ReplaceWithNullTitle,
                            createChangedDocument: ct =>
                                ReplaceWithNullAsync(context.Document, node, ct),
                            equivalenceKey: ReplaceWithNullTitle),
                        diagnostic);
                }
            }
        }

        /// <summary>
        /// Removes the <c>DefaultDllImportSearchPaths</c> attribute from the document.
        /// If it is the only attribute in its attribute list, the entire list (including brackets) is removed.
        /// </summary>
        /// <param name="document">The document containing the attribute to remove.</param>
        /// <param name="attribute">The attribute syntax node to remove.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>A task that returns the updated document with the attribute removed.</returns>
        private static async Task<Document> RemoveAttributeAsync(
            Document document,
            AttributeSyntax attribute,
            CancellationToken cancellationToken)
        {
            var editor = await DocumentEditor.CreateAsync(document, cancellationToken)
                .ConfigureAwait(false);

            if (attribute.Parent is AttributeListSyntax attributeList &&
                attributeList.Attributes.Count == 1)
            {
                // Only attribute in the list - remove the whole list including brackets
                editor.RemoveNode(attributeList);
            }
            else
            {
                // Multiple attributes in the list - remove just this one
                editor.RemoveNode(attribute);
            }

            return editor.GetChangedDocument();
        }

        /// <summary>
        /// Replaces the <c>DllImportSearchPath.AssemblyDirectory</c> argument with <c>null</c>
        /// to restore default search behavior.
        /// </summary>
        /// <param name="document">The document containing the code to fix.</param>
        /// <param name="assemblyDirNode">The syntax node for the <c>DllImportSearchPath.AssemblyDirectory</c> expression.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>A task that returns the updated document with <c>null</c> replacing the search path.</returns>
        private static async Task<Document> ReplaceWithNullAsync(
            Document document,
            SyntaxNode assemblyDirNode,
            CancellationToken cancellationToken)
        {
            var editor = await DocumentEditor.CreateAsync(document, cancellationToken)
                .ConfigureAwait(false);

            var nullLiteral = SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)
                .FormatterAnnotated();

            editor.ReplaceNode(assemblyDirNode, (current, gen) => nullLiteral.WithTriviaFrom(current));

            var changedDocument = editor.GetChangedDocument();
            return await Formatter.FormatAsync(
                changedDocument,
                Formatter.Annotation,
                cancellationToken: cancellationToken);
        }
    }
}
