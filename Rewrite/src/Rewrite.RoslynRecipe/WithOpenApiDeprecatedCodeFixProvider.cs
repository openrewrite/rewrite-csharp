using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace Rewrite.RoslynRecipe
{
    /// <summary>
    /// Code fix provider that replaces the deprecated WithOpenApi method with AddOpenApiOperationTransformer.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(WithOpenApiDeprecatedCodeFixProvider)), Shared]
    public class WithOpenApiDeprecatedCodeFixProvider : CodeFixProvider
    {
        private const string Title = "Replace WithOpenApi with AddOpenApiOperationTransformer";

        /// <summary>
        /// Gets the diagnostic IDs that this code fix provider can fix.
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(WithOpenApiDeprecatedAnalyzer.DiagnosticId);

        /// <summary>
        /// Gets a value indicating whether the provider can fix multiple diagnostics in a single operation.
        /// </summary>
        /// <returns>The fix all provider for batch operations.</returns>
        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        /// <summary>
        /// Registers code fixes for the diagnostics reported by the WithOpenApiDeprecatedAnalyzer.
        /// </summary>
        /// <param name="context">The code fix context containing the diagnostics to fix. Must not be null.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root == null)
                return;

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the WithOpenApi invocation
            var node = root.FindNode(diagnosticSpan);
            var invocation = node.FirstAncestorOrSelf<InvocationExpressionSyntax>();
            if (invocation == null)
                return;

            // Register a code action that will invoke the fix
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: Title,
                    createChangedDocument: cancellationToken => ReplaceWithOpenApiAsync(context.Document, invocation, cancellationToken),
                    equivalenceKey: Title),
                diagnostic);
        }

        /// <summary>
        /// Replaces the WithOpenApi method call with AddOpenApiOperationTransformer.
        /// </summary>
        /// <param name="document">The document containing the code to fix. Must not be null.</param>
        /// <param name="invocation">The invocation expression to replace. Must not be null.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>A task that returns the updated document with the fix applied.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when document or invocation is null.</exception>
        private async Task<Document> ReplaceWithOpenApiAsync(Document document, InvocationExpressionSyntax invocation, CancellationToken cancellationToken)
        {
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            if (semanticModel == null)
                return document;

            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (root == null)
                return document;

            // Check if this is a member access expression (e.g., something.WithOpenApi())
            if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
                return document;

            // Create the new method name
            var newMethodName = SyntaxFactory.IdentifierName("AddOpenApiOperationTransformer");

            // Create the new member access expression with the updated method name
            var newMemberAccess = memberAccess.WithName(newMethodName);

            // Handle the arguments - need to transform the lambda if present
            var newArguments = TransformArguments(invocation.ArgumentList, semanticModel);

            // Create the new invocation
            var newInvocation = invocation
                .WithExpression(newMemberAccess)
                .WithArgumentList(newArguments);

            // Replace the old invocation with the new one
            var newRoot = root.ReplaceNode(invocation, newInvocation);

            return document.WithSyntaxRoot(newRoot);
        }

        /// <summary>
        /// Transforms the arguments from WithOpenApi format to AddOpenApiOperationTransformer format.
        /// </summary>
        /// <param name="argumentList">The original argument list from WithOpenApi. Must not be null.</param>
        /// <param name="semanticModel">The semantic model for type information. Must not be null.</param>
        /// <returns>The transformed argument list for AddOpenApiOperationTransformer.</returns>
        private ArgumentListSyntax TransformArguments(ArgumentListSyntax argumentList, SemanticModel semanticModel)
        {
            if (argumentList.Arguments.Count == 0)
            {
                // If there are no arguments, return an empty argument list
                // AddOpenApiOperationTransformer requires a lambda, so we'll create a default one
                var defaultLambda = SyntaxFactory.ParseExpression("(operation, context, ct) => Task.CompletedTask");
                return SyntaxFactory.ArgumentList(
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.Argument(defaultLambda)));
            }

            var firstArgument = argumentList.Arguments[0];
            var expression = firstArgument.Expression;

            // Check if the argument is a lambda expression
            if (expression is ParenthesizedLambdaExpressionSyntax lambda)
            {
                var transformedLambda = TransformLambda(lambda);
                return SyntaxFactory.ArgumentList(
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.Argument(transformedLambda)));
            }
            else if (expression is SimpleLambdaExpressionSyntax simpleLambda)
            {
                var transformedLambda = TransformSimpleLambda(simpleLambda);
                return SyntaxFactory.ArgumentList(
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.Argument(transformedLambda)));
            }

            // For other expression types (method groups, etc.), wrap in a lambda
            var wrappedLambda = CreateWrappedLambda(expression);
            return SyntaxFactory.ArgumentList(
                SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.Argument(wrappedLambda)));
        }

        /// <summary>
        /// Transforms a parenthesized lambda expression from WithOpenApi format to AddOpenApiOperationTransformer format.
        /// </summary>
        /// <param name="lambda">The original lambda expression. Must not be null.</param>
        /// <returns>The transformed lambda expression with async Task signature.</returns>
        private ExpressionSyntax TransformLambda(ParenthesizedLambdaExpressionSyntax lambda)
        {
            // WithOpenApi: (operation) => { operation.Summary = "..."; return operation; }
            // AddOpenApiOperationTransformer: (operation, context, ct) => { operation.Summary = "..."; return Task.CompletedTask; }

            // Create new parameter list with three parameters
            var parameters = SyntaxFactory.ParameterList(
                SyntaxFactory.SeparatedList(new[]
                {
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("operation")),
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("context")),
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("ct"))
                }));

            // Transform the body
            if (lambda.Body is BlockSyntax block)
            {
                var transformedBlock = TransformLambdaBlock(block);
                return SyntaxFactory.ParenthesizedLambdaExpression(
                    parameters,
                    transformedBlock);
            }
            else if (lambda.Body is ExpressionSyntax expr)
            {
                // For expression-bodied lambdas, wrap in a block that returns Task.CompletedTask
                var statements = new StatementSyntax[]
                {
                    SyntaxFactory.ExpressionStatement(expr),
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.ParseExpression("Task.CompletedTask"))
                };

                var newBlock = SyntaxFactory.Block(statements);
                return SyntaxFactory.ParenthesizedLambdaExpression(
                    parameters,
                    newBlock);
            }

            return lambda;
        }

        /// <summary>
        /// Transforms a simple lambda expression from WithOpenApi format to AddOpenApiOperationTransformer format.
        /// </summary>
        /// <param name="lambda">The original simple lambda expression. Must not be null.</param>
        /// <returns>The transformed lambda expression with async Task signature.</returns>
        private ExpressionSyntax TransformSimpleLambda(SimpleLambdaExpressionSyntax lambda)
        {
            // Create new parameter list with three parameters
            var parameters = SyntaxFactory.ParameterList(
                SyntaxFactory.SeparatedList(new[]
                {
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("operation")),
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("context")),
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("ct"))
                }));

            // Transform the body
            if (lambda.Body is BlockSyntax block)
            {
                var transformedBlock = TransformLambdaBlock(block);
                return SyntaxFactory.ParenthesizedLambdaExpression(
                    parameters,
                    transformedBlock);
            }
            else if (lambda.Body is ExpressionSyntax expr)
            {
                // For expression-bodied lambdas, wrap in a block that returns Task.CompletedTask
                var statements = new StatementSyntax[]
                {
                    SyntaxFactory.ExpressionStatement(expr),
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.ParseExpression("Task.CompletedTask"))
                };

                var newBlock = SyntaxFactory.Block(statements);
                return SyntaxFactory.ParenthesizedLambdaExpression(
                    parameters,
                    newBlock);
            }

            // Fallback: convert to parenthesized lambda
            return SyntaxFactory.ParenthesizedLambdaExpression(
                parameters,
                lambda.Body);
        }

        /// <summary>
        /// Transforms a lambda block by replacing return statements that return the operation with Task.CompletedTask.
        /// </summary>
        /// <param name="block">The original block syntax. Must not be null.</param>
        /// <returns>The transformed block with updated return statements.</returns>
        private BlockSyntax TransformLambdaBlock(BlockSyntax block)
        {
            var statements = block.Statements.ToList();

            for (int i = 0; i < statements.Count; i++)
            {
                if (statements[i] is ReturnStatementSyntax returnStatement)
                {
                    // Replace 'return operation;' with 'return Task.CompletedTask;'
                    if (returnStatement.Expression != null)
                    {
                        var newReturn = SyntaxFactory.ReturnStatement(
                            SyntaxFactory.ParseExpression("Task.CompletedTask"))
                            .WithLeadingTrivia(returnStatement.GetLeadingTrivia())
                            .WithTrailingTrivia(returnStatement.GetTrailingTrivia());

                        statements[i] = newReturn;
                    }
                }
            }

            // If there's no return statement, add one at the end
            bool hasReturn = statements.Any(s => s is ReturnStatementSyntax);
            if (!hasReturn)
            {
                statements.Add(SyntaxFactory.ReturnStatement(
                    SyntaxFactory.ParseExpression("Task.CompletedTask")));
            }

            return block.WithStatements(SyntaxFactory.List(statements));
        }

        /// <summary>
        /// Creates a wrapped lambda expression for non-lambda arguments.
        /// </summary>
        /// <param name="expression">The expression to wrap. Must not be null.</param>
        /// <returns>A lambda expression that wraps the original expression.</returns>
        private ExpressionSyntax CreateWrappedLambda(ExpressionSyntax expression)
        {
            // Create: (operation, context, ct) => { originalExpression(operation); return Task.CompletedTask; }
            var parameters = SyntaxFactory.ParameterList(
                SyntaxFactory.SeparatedList(new[]
                {
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("operation")),
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("context")),
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("ct"))
                }));

            var invocation = SyntaxFactory.InvocationExpression(
                expression,
                SyntaxFactory.ArgumentList(
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.Argument(SyntaxFactory.IdentifierName("operation")))));

            var statements = new StatementSyntax[]
            {
                SyntaxFactory.ExpressionStatement(invocation),
                SyntaxFactory.ReturnStatement(
                    SyntaxFactory.ParseExpression("Task.CompletedTask"))
            };

            var block = SyntaxFactory.Block(statements);

            return SyntaxFactory.ParenthesizedLambdaExpression(
                parameters,
                block);
        }
    }
}