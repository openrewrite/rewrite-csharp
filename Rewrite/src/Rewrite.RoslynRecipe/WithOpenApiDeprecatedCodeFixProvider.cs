using System;
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
using Microsoft.CodeAnalysis.Formatting;
using Rewrite.RoslynRecipe.Helpers;

namespace Rewrite.RoslynRecipe
{
    /// <summary>
    /// Code fix provider that replaces the deprecated WithOpenApi method with AddOpenApiOperationTransformer.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(WithOpenApiDeprecatedCodeFixProvider)), Shared]
    public class WithOpenApiDeprecatedCodeFixProvider : CodeFixProvider
    {
        private const string Title = "Replace WithOpenApi with AddOpenApiOperationTransformer";
        private const string NewMethodName = "AddOpenApiOperationTransformer";
        private const string OperationParameterName = "operation";
        private const string ContextParameterName = "context";
        private const string CancellationTokenParameterName = "ct";

        /// <summary>
        /// Gets the diagnostic IDs that this code fix provider can fix.
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(WithOpenApiDeprecatedAnalyzer.DiagnosticId);

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
                    createChangedDocument: cancellationToken =>
                        ReplaceWithOpenApiAsync(context.Document, invocation, cancellationToken),
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
        /// <exception cref="ArgumentNullException">Thrown when document or invocation is null.</exception>
        private async Task<Document> ReplaceWithOpenApiAsync(
            Document document,
            InvocationExpressionSyntax invocation,
            CancellationToken cancellationToken)
        {
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            if (semanticModel == null)
                return document;

            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (root == null)
                return document;

            // Check if this is a member access expression
            if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
                return document;

            // Create the new member access with updated method name
            var newMemberAccess = memberAccess.WithName(SyntaxFactory.IdentifierName(NewMethodName));

            // Transform the arguments
            var transformedLambda = CreateTransformedLambda(invocation.ArgumentList, semanticModel);

            // Create the new invocation
            var newInvocation = invocation
                .WithExpression(newMemberAccess)
                .WithArgumentList(SyntaxFactory.ArgumentList(
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.Argument(transformedLambda))));

            // Replace the old invocation with the new one
            var newRoot = root.ReplaceNode(invocation, newInvocation);
            var newDocument = document.WithSyntaxRoot(newRoot);

            // Ensure Task is available by looking for Task.CompletedTask in the lambda
            var updatedSemanticModel = await newDocument.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            if (updatedSemanticModel != null)
            {
                // Find any Task.CompletedTask references in the new tree
                var taskReferences = newRoot.DescendantNodes()
                    .OfType<MemberAccessExpressionSyntax>()
                    .Where(m => m.Name.Identifier.Text == "CompletedTask" &&
                               m.Expression is IdentifierNameSyntax id &&
                               id.Identifier.Text == "Task");

                if (taskReferences.Any())
                {
                    var firstTaskRef = taskReferences.First();
                    var (finalDocument, finalRoot) = await SymbolImporter.MaybeAddTaskUsingAsync(
                        newDocument,
                        newRoot,
                        updatedSemanticModel,
                        firstTaskRef,
                        cancellationToken);

                    // Update document and root after adding using
                    newDocument = finalDocument;
                    newRoot = finalRoot;
                }
            }

            // Format the document
            var formattedDocument = await Formatter.FormatAsync(
                newDocument,
                Formatter.Annotation,
                cancellationToken: cancellationToken);

            return formattedDocument;
        }

        /// <summary>
        /// Creates a transformed lambda expression for AddOpenApiOperationTransformer.
        /// </summary>
        /// <param name="argumentList">The original argument list from WithOpenApi.</param>
        /// <param name="semanticModel">The semantic model for type information.</param>
        /// <returns>A lambda expression compatible with AddOpenApiOperationTransformer.</returns>
        private ExpressionSyntax CreateTransformedLambda(ArgumentListSyntax argumentList, SemanticModel semanticModel)
        {
            // Create the new parameter list for AddOpenApiOperationTransformer
            var parameters = CreateNewParameterList();

            // If no arguments, create default lambda
            if (argumentList.Arguments.Count == 0)
            {
                return CreateDefaultLambda(parameters);
            }

            var firstArgument = argumentList.Arguments[0].Expression;

            // Handle different expression types
            return firstArgument switch
            {
                LambdaExpressionSyntax lambda => TransformLambdaExpression(lambda, parameters, semanticModel),
                _ => CreateWrappedMethodGroupLambda(firstArgument, parameters)
            };
        }

        /// <summary>
        /// Creates the new parameter list for the transformed lambda.
        /// </summary>
        private ParameterListSyntax CreateNewParameterList()
        {
            return SyntaxFactory.ParameterList(
                SyntaxFactory.SeparatedList(new[]
                {
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier(OperationParameterName)),
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier(ContextParameterName)),
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier(CancellationTokenParameterName))
                }));
        }

        /// <summary>
        /// Creates a default lambda expression that returns Task.CompletedTask.
        /// </summary>
        private ExpressionSyntax CreateDefaultLambda(ParameterListSyntax parameters)
        {
            var taskCompletedTask = SyntaxFactory.ParseExpression("Task.CompletedTask")
                .DiscardFormatting();

            return SyntaxFactory.ParenthesizedLambdaExpression(
                parameters,
                taskCompletedTask);
        }

        /// <summary>
        /// Transforms a lambda expression to the new format.
        /// </summary>
        private ExpressionSyntax TransformLambdaExpression(
            LambdaExpressionSyntax lambda,
            ParameterListSyntax parameters,
            SemanticModel semanticModel)
        {
            // Get the original parameter name
            string originalParameterName = GetOriginalParameterName(lambda);

            // Transform the body
            CSharpSyntaxNode transformedBody = TransformLambdaBody(
                lambda.Body,
                originalParameterName,
                semanticModel);

            return SyntaxFactory.ParenthesizedLambdaExpression(
                parameters,
                transformedBody);
        }

        /// <summary>
        /// Gets the original parameter name from a lambda expression.
        /// </summary>
        private string GetOriginalParameterName(LambdaExpressionSyntax lambda)
        {
            return lambda switch
            {
                SimpleLambdaExpressionSyntax simple => simple.Parameter.Identifier.Text,
                ParenthesizedLambdaExpressionSyntax parenthesized when parenthesized.ParameterList.Parameters.Count > 0
                    => parenthesized.ParameterList.Parameters[0].Identifier.Text,
                _ => "operation"
            };
        }

        /// <summary>
        /// Transforms the body of a lambda expression.
        /// </summary>
        private CSharpSyntaxNode TransformLambdaBody(
            CSharpSyntaxNode body,
            string originalParameterName,
            SemanticModel semanticModel)
        {
            // Replace parameter references if needed
            var parameterReplacer = new ParameterReplacementRewriter(
                originalParameterName,
                OperationParameterName);

            var updatedBody = (CSharpSyntaxNode)parameterReplacer.Visit(body);

            return updatedBody switch
            {
                BlockSyntax block => TransformBlockBody(block),
                ExpressionSyntax expr => CreateBlockFromExpression(expr),
                _ => updatedBody
            };
        }

        /// <summary>
        /// Transforms a block body by replacing return statements.
        /// </summary>
        private BlockSyntax TransformBlockBody(BlockSyntax block)
        {
            var rewriter = new ReturnStatementRewriter();
            var transformedBlock = (BlockSyntax)rewriter.Visit(block);

            // Ensure there's a return statement
            if (!transformedBlock.Statements.Any(s => s is ReturnStatementSyntax))
            {
                var statements = transformedBlock.Statements.Add(
                    CreateReturnTaskCompletedStatement());
                transformedBlock = transformedBlock.WithStatements(statements);
            }

            return transformedBlock.WithAdditionalAnnotations(Formatter.Annotation);
        }

        /// <summary>
        /// Creates a block from an expression.
        /// </summary>
        private BlockSyntax CreateBlockFromExpression(ExpressionSyntax expression)
        {
            return SyntaxFactory.Block(
                SyntaxFactory.ExpressionStatement(expression),
                CreateReturnTaskCompletedStatement())
                .WithAdditionalAnnotations(Formatter.Annotation);
        }

        /// <summary>
        /// Creates a return statement that returns Task.CompletedTask.
        /// </summary>
        private ReturnStatementSyntax CreateReturnTaskCompletedStatement()
        {
            return SyntaxFactory.ReturnStatement(
                SyntaxFactory.ParseExpression("Task.CompletedTask")
                    .DiscardFormatting());
        }

        /// <summary>
        /// Creates a wrapped lambda for method group expressions.
        /// </summary>
        private ExpressionSyntax CreateWrappedMethodGroupLambda(
            ExpressionSyntax methodGroup,
            ParameterListSyntax parameters)
        {
            // Create: methodGroup(operation);
            var invocation = SyntaxFactory.InvocationExpression(
                methodGroup,
                SyntaxFactory.ArgumentList(
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.Argument(
                            SyntaxFactory.IdentifierName(OperationParameterName)))));

            var block = SyntaxFactory.Block(
                SyntaxFactory.ExpressionStatement(invocation),
                CreateReturnTaskCompletedStatement())
                .WithAdditionalAnnotations(Formatter.Annotation);

            return SyntaxFactory.ParenthesizedLambdaExpression(parameters, block);
        }

        /// <summary>
        /// Rewriter that replaces parameter references in lambda bodies.
        /// </summary>
        private sealed class ParameterReplacementRewriter : CSharpSyntaxRewriter
        {
            private readonly string _oldParameterName;
            private readonly string _newParameterName;

            public ParameterReplacementRewriter(string oldParameterName, string newParameterName)
            {
                _oldParameterName = oldParameterName;
                _newParameterName = newParameterName;
            }

            public override SyntaxNode? VisitIdentifierName(IdentifierNameSyntax node)
            {
                if (node.Identifier.Text == _oldParameterName)
                {
                    return SyntaxFactory.IdentifierName(_newParameterName)
                        .WithTriviaFrom(node);
                }
                return base.VisitIdentifierName(node);
            }
        }

        /// <summary>
        /// Rewriter that replaces return statements with Task.CompletedTask.
        /// </summary>
        private sealed class ReturnStatementRewriter : CSharpSyntaxRewriter
        {
            public override SyntaxNode? VisitReturnStatement(ReturnStatementSyntax node)
            {
                // Replace any return statement with return Task.CompletedTask
                return SyntaxFactory.ReturnStatement(
                    SyntaxFactory.ParseExpression("Task.CompletedTask")
                        .DiscardFormatting())
                    .WithTriviaFrom(node);
            }
        }
    }
}