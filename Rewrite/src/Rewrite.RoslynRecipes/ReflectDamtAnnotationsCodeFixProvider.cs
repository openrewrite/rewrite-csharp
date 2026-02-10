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
    /// Provides a code fix that replaces <c>DynamicallyAccessedMemberTypes.All</c> with the recommended
    /// more restricted set of member types for types implementing <c>IReflect</c> or deriving from
    /// <c>Type</c>/<c>TypeInfo</c>.
    /// </summary>
    /// <remarks>
    /// The replacement uses the specific member types recommended by .NET 10:
    /// <c>PublicFields | NonPublicFields | PublicMethods | NonPublicMethods |
    /// PublicProperties | NonPublicProperties | PublicConstructors | NonPublicConstructors</c>.
    /// See <see href="https://learn.microsoft.com/en-us/dotnet/core/compatibility/reflection/10/ireflect-damt-annotations">
    /// More restricted annotations on InvokeMember/FindMembers/DeclaredMembers documentation</see> for details.
    /// </remarks>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ReflectDamtAnnotationsCodeFixProvider)), Shared]
    public sealed class ReflectDamtAnnotationsCodeFixProvider : CodeFixProvider
    {
        private const string Title = "Replace DynamicallyAccessedMemberTypes.All with restricted member types";

        internal const string ReplacementExpression =
            "DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | " +
            "DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods | " +
            "DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | " +
            "DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors";

        /// <summary>
        /// Gets the diagnostic IDs that this code fix provider can handle.
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds { get; } =
            ImmutableArray.Create(ReflectDamtAnnotationsAnalyzer.DiagnosticId);

        /// <summary>
        /// Gets the fix all provider for batch fixing multiple instances.
        /// </summary>
        /// <returns>A batch fix all provider that can fix multiple instances at once.</returns>
        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        /// <summary>
        /// Registers code fixes for diagnostics about <c>DynamicallyAccessedMemberTypes.All</c>
        /// on types implementing <c>IReflect</c> or deriving from <c>Type</c>/<c>TypeInfo</c>.
        /// </summary>
        /// <param name="context">The code fix context containing the diagnostics to fix.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root is null) return;

            foreach (var diagnostic in context.Diagnostics)
            {
                var diagnosticSpan = diagnostic.Location.SourceSpan;
                var node = root.FindNode(diagnosticSpan, getInnermostNodeForTie: true);

                if (node is not AttributeSyntax attribute)
                    continue;

                // Verify the argument contains DynamicallyAccessedMemberTypes.All
                if (attribute.ArgumentList is null || attribute.ArgumentList.Arguments.Count == 0)
                    continue;

                var argExpression = attribute.ArgumentList.Arguments[0].Expression;
                var allAccess = FindAllMemberAccess(argExpression);
                if (allAccess is null)
                    continue;

                context.RegisterCodeFix(
                    CodeAction.Create(
                        title: Title,
                        createChangedDocument: ct =>
                            ReplaceAllWithRestrictedAsync(context.Document, allAccess, ct),
                        equivalenceKey: Title),
                    diagnostic);
            }
        }

        /// <summary>
        /// Finds the <c>DynamicallyAccessedMemberTypes.All</c> member access expression within
        /// the given expression tree.
        /// </summary>
        /// <param name="expression">The expression to search. Must not be null.</param>
        /// <returns>The member access expression for <c>.All</c>, or null if not found.</returns>
        private static MemberAccessExpressionSyntax? FindAllMemberAccess(ExpressionSyntax expression)
        {
            if (expression is MemberAccessExpressionSyntax memberAccess &&
                memberAccess.Name.Identifier.Text == "All")
                return memberAccess;

            if (expression is BinaryExpressionSyntax binary)
                return FindAllMemberAccess(binary.Left) ?? FindAllMemberAccess(binary.Right);

            return null;
        }

        /// <summary>
        /// Replaces the <c>DynamicallyAccessedMemberTypes.All</c> expression with the recommended
        /// restricted set of member types.
        /// </summary>
        /// <param name="document">The document containing the code to fix.</param>
        /// <param name="allExpression">The syntax node for the <c>.All</c> member access.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>A task that returns the updated document with the restricted member types.</returns>
        private static async Task<Document> ReplaceAllWithRestrictedAsync(
            Document document,
            SyntaxNode allExpression,
            CancellationToken cancellationToken)
        {
            var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);

            var replacement = SyntaxFactory.ParseExpression(ReplacementExpression)
                .FormatterAnnotated();

            editor.ReplaceNode(allExpression, (current, gen) => replacement);

            var changedDocument = editor.GetChangedDocument();
            return await Formatter.FormatAsync(
                changedDocument,
                Formatter.Annotation,
                cancellationToken: cancellationToken);
        }
    }
}
