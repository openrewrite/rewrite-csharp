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
    /// Provides automatic code fixes for the deprecated WithOpenApi method by replacing it with AddOpenApiOperationTransformer.
    /// This transformation is necessary for ASP.NET Core applications migrating to newer OpenAPI integration patterns.
    /// </summary>
    /// <remarks>
    /// The WithOpenApi method was deprecated in favor of AddOpenApiOperationTransformer which provides better async support
    /// and additional context parameters for more flexible OpenAPI operation transformations.
    ///
    /// Example transformation:
    /// Before:
    /// <code>
    /// app.MapGet("/api", () => "test")
    ///    .WithOpenApi(operation => {
    ///        operation.Summary = "Test API";
    ///        return operation;
    ///    });
    /// </code>
    ///
    /// After:
    /// <code>
    /// app.MapGet("/api", () => "test")
    ///    .AddOpenApiOperationTransformer((operation, context, ct) => {
    ///        operation.Summary = "Test API";
    ///        return Task.CompletedTask;
    ///    });
    /// </code>
    /// </remarks>
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
        /// Gets the fix all provider for batch operations, allowing multiple WithOpenApi calls
        /// to be fixed simultaneously across a document or solution.
        /// </summary>
        /// <returns>The well-known batch fixer for applying the same fix to multiple diagnostics.</returns>
        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        /// <summary>
        /// Registers code fixes for WithOpenApi deprecation warnings by offering to replace them
        /// with the modern AddOpenApiOperationTransformer method.
        /// </summary>
        /// <param name="context">The code fix context containing the diagnostics to fix and document information.</param>
        /// <returns>A task representing the asynchronous registration operation.</returns>
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var documentEditor = await DocumentEditor.CreateAsync(context.Document);
            
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
        /// Performs the actual replacement of WithOpenApi with AddOpenApiOperationTransformer,
        /// including transforming the lambda signature and body to match the new API requirements.
        /// </summary>
        /// <param name="document">The document containing the code to fix.</param>
        /// <param name="invocation">The WithOpenApi invocation expression to replace.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>A task that returns the updated document with the transformation applied.</returns>
        /// <remarks>
        /// This method handles several transformation scenarios:
        /// 1. Parameterless calls: WithOpenApi() becomes AddOpenApiOperationTransformer((operation, context, ct) => Task.CompletedTask)
        /// 2. Simple lambdas: WithOpenApi(op => op) becomes AddOpenApiOperationTransformer((operation, context, ct) => Task.CompletedTask)
        /// 3. Complex lambdas: WithOpenApi(op => { op.Summary = "X"; return op; }) becomes
        ///    AddOpenApiOperationTransformer((operation, context, ct) => { operation.Summary = "X"; return Task.CompletedTask; })
        /// 4. Method groups: WithOpenApi(Configure) becomes AddOpenApiOperationTransformer((operation, context, ct) => { Configure(operation); return Task.CompletedTask; })
        /// </remarks>
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
                               id.Identifier.Text == "Task")
                    .ToArray();

                if (taskReferences.Any())
                {
                    var firstTaskRef = taskReferences.First();
                    var finalDocument = await UsingsUtil.MaybeAddUsingAsync(
                        newDocument,
                        updatedSemanticModel,
                        firstTaskRef,
                        "System.Threading.Tasks.Task",
                        cancellationToken);

                    // Update document and root after adding using
                    newDocument = finalDocument;
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
        /// Creates a transformed lambda expression compatible with AddOpenApiOperationTransformer's signature,
        /// converting from the old single-parameter format to the new three-parameter async format.
        /// </summary>
        /// <param name="argumentList">The original argument list from the WithOpenApi invocation.</param>
        /// <param name="semanticModel">The semantic model for type information and symbol resolution.</param>
        /// <returns>A lambda expression with the signature (operation, context, ct) => Task that performs the same operation transformation.</returns>
        /// <example>
        /// Input: operation => operation.Summary = "Test"
        /// Output: (operation, context, ct) => { operation.Summary = "Test"; return Task.CompletedTask; }
        /// </example>
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
        /// Creates the standardized parameter list for AddOpenApiOperationTransformer with three parameters:
        /// operation (OpenApiOperation), context (ApiDescriptionProviderContext), and ct (CancellationToken).
        /// </summary>
        /// <returns>A parameter list with the three required parameters for the new API.</returns>
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
        /// Creates a default lambda expression for parameterless WithOpenApi() calls that simply
        /// returns Task.CompletedTask without performing any operation modifications.
        /// </summary>
        /// <param name="parameters">The new parameter list for the lambda.</param>
        /// <returns>A lambda expression: (operation, context, ct) => Task.CompletedTask</returns>
        private ExpressionSyntax CreateDefaultLambda(ParameterListSyntax parameters)
        {
            var taskCompletedTask = SyntaxFactory.ParseExpression("Task.CompletedTask")
                .DiscardFormatting();

            return SyntaxFactory.ParenthesizedLambdaExpression(
                parameters,
                taskCompletedTask);
        }

        /// <summary>
        /// Transforms an existing lambda expression from the old WithOpenApi format to the new
        /// AddOpenApiOperationTransformer format, updating parameter names and converting return statements.
        /// </summary>
        /// <param name="lambda">The original lambda expression from WithOpenApi.</param>
        /// <param name="parameters">The new parameter list with three parameters.</param>
        /// <param name="semanticModel">The semantic model for analysis.</param>
        /// <returns>A transformed lambda compatible with AddOpenApiOperationTransformer.</returns>
        /// <example>
        /// Input: op => { op.Deprecated = true; return op; }
        /// Output: (operation, context, ct) => { operation.Deprecated = true; return Task.CompletedTask; }
        /// </example>
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
        /// Extracts the parameter name from the original lambda expression to enable proper
        /// parameter reference replacement in the lambda body.
        /// </summary>
        /// <param name="lambda">The lambda expression to extract the parameter name from.</param>
        /// <returns>The original parameter name (e.g., "op", "operation", "x") or "operation" as default.</returns>
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
        /// Transforms the body of a lambda expression by replacing parameter references with the new
        /// parameter name and converting return statements from returning the operation to returning Task.CompletedTask.
        /// </summary>
        /// <param name="body">The original lambda body to transform.</param>
        /// <param name="originalParameterName">The original parameter name to replace (e.g., "op").</param>
        /// <param name="semanticModel">The semantic model for analysis.</param>
        /// <returns>The transformed lambda body with updated parameter references and return statements.</returns>
        /// <remarks>
        /// Special cases handled:
        /// - Simple identity lambdas (op => op) are converted to just Task.CompletedTask
        /// - Expression bodies are wrapped in blocks with return Task.CompletedTask
        /// - Statement blocks have their return statements replaced with return Task.CompletedTask
        /// </remarks>
        private CSharpSyntaxNode TransformLambdaBody(
            CSharpSyntaxNode body,
            string originalParameterName,
            SemanticModel semanticModel)
        {
            // For simple expression lambdas that just return the parameter (e.g., op => op),
            // we don't need to include the expression in the new lambda body
            if (body is IdentifierNameSyntax id && id.Identifier.Text == originalParameterName)
            {
                // Just return Task.CompletedTask without including the operation expression
                return SyntaxFactory.ParseExpression("Task.CompletedTask")
                    .DiscardFormatting();
            }

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
        /// Transforms a block body by replacing all return statements that return the operation
        /// with statements that return Task.CompletedTask, maintaining the operation modifications.
        /// </summary>
        /// <param name="block">The block syntax to transform.</param>
        /// <returns>A transformed block with return statements converted to return Task.CompletedTask.</returns>
        /// <example>
        /// Input: <code>{ operation.Summary = "Test"; return operation; }</code>
        /// Output: <code>{ operation.Summary = "Test"; return Task.CompletedTask; }</code>
        /// </example>
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
        /// Converts an expression body lambda into a block body with the expression as a statement
        /// followed by a return Task.CompletedTask statement.
        /// </summary>
        /// <param name="expression">The expression to wrap in a block.</param>
        /// <returns>A block containing the expression as a statement and return Task.CompletedTask.</returns>
        /// <example>
        /// Input: operation.Summary = "Test"
        /// Output: { operation.Summary = "Test"; return Task.CompletedTask; }
        /// </example>
        private BlockSyntax CreateBlockFromExpression(ExpressionSyntax expression)
        {
            return SyntaxFactory.Block(
                SyntaxFactory.ExpressionStatement(expression),
                CreateReturnTaskCompletedStatement())
                .WithAdditionalAnnotations(Formatter.Annotation);
        }

        /// <summary>
        /// Creates a return statement that returns Task.CompletedTask, used to replace
        /// the original return statements that returned the modified operation.
        /// </summary>
        /// <returns>A return statement: return Task.CompletedTask;</returns>
        private ReturnStatementSyntax CreateReturnTaskCompletedStatement()
        {
            return SyntaxFactory.ReturnStatement(
                SyntaxFactory.ParseExpression("Task.CompletedTask")
                    .DiscardFormatting());
        }

        /// <summary>
        /// Creates a wrapped lambda for method group expressions, converting them into a lambda
        /// that invokes the method and then returns Task.CompletedTask.
        /// </summary>
        /// <param name="methodGroup">The method group expression to wrap.</param>
        /// <param name="parameters">The new parameter list for the lambda.</param>
        /// <returns>A lambda that invokes the method group and returns Task.CompletedTask.</returns>
        /// <example>
        /// Input: ConfigureOperation (where ConfigureOperation is a method)
        /// Output: (operation, context, ct) => { ConfigureOperation(operation); return Task.CompletedTask; }
        /// </example>
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
        /// Rewriter that replaces all references to the old parameter name with the new standardized
        /// "operation" parameter name throughout the lambda body.
        /// </summary>
        /// <example>
        /// Transforms: op.Summary = "Test"
        /// Into: operation.Summary = "Test"
        /// </example>
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
        /// Rewriter that replaces all return statements in the lambda body to return Task.CompletedTask
        /// instead of returning the modified operation object.
        /// </summary>
        /// <example>
        /// Transforms: return operation;
        /// Into: return Task.CompletedTask;
        /// </example>
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