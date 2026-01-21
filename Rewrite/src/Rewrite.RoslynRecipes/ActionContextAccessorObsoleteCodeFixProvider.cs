using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Simplification;
using Rewrite.RoslynRecipes.Helpers;

namespace Rewrite.RoslynRecipes;

/// <summary>
/// Provides code fixes for migrating from the obsolete IActionContextAccessor to IHttpContextAccessor.
/// </summary>
/// <remarks>
/// This code fix provider handles the migration from IActionContextAccessor to IHttpContextAccessor,
/// including renaming fields/properties, updating property accesses, and managing using directives.
/// All syntax tree editing is done through DocumentEditor as per best practices.
/// </remarks>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ActionContextAccessorObsoleteCodeFixProvider)), Shared]
public sealed class ActionContextAccessorObsoleteCodeFixProvider : CodeFixProvider
{
    /// <summary>
    /// Gets the diagnostic IDs that this code fix provider can handle.
    /// </summary>
    public sealed override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(ActionContextAccessorObsoleteAnalyzer.DiagnosticId);

    /// <summary>
    /// Gets the fix all provider for batch fixing multiple instances.
    /// </summary>
    /// <returns>A batch fix all provider that can fix multiple instances at once.</returns>
    public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    /// <summary>
    /// Registers code fixes for the specified context.
    /// </summary>
    /// <param name="context">The code fix context containing the diagnostics to fix.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var document = context.Document;
        var root = await document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root == null) return;
        // context.Document.Project.Solution
        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        // Find the parameter type node that triggered the diagnostic
        var node = root.FindNode(diagnosticSpan);
        var parameter = node.FirstAncestorOrSelf<ParameterSyntax>();
        if (parameter == null) return;

        // Check if ModelState is used (from diagnostic properties)
        bool hasModelStateUsage = diagnostic.Properties.ContainsKey("HasModelStateUsage") &&
                                   diagnostic.Properties["HasModelStateUsage"] == "true";
        
        
        if (!hasModelStateUsage)
        {
            
            // Normal case: perform full refactoring
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: "Replace IActionContextAccessor with IHttpContextAccessor",
                    createChangedDocument: ct => ReplaceActionContextAccessorAsync(document, parameter, ct),
                    equivalenceKey: "ReplaceActionContextAccessor"),
                diagnostic);
        }
    }

    /// <summary>
    /// Performs the full refactoring from IActionContextAccessor to IHttpContextAccessor.
    /// </summary>
    private async Task<Document> ReplaceActionContextAccessorAsync(
        Document document,
        ParameterSyntax parameter,
        CancellationToken cancellationToken)
    {
        var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
        var semanticModel = editor.SemanticModel;
        var root = editor.OriginalRoot;

        // Get the parameter symbol
        var parameterSymbol = semanticModel.GetDeclaredSymbol(parameter, cancellationToken);
        if (parameterSymbol == null) return document;
        
        // 1. Find the containing type declaration
        var containingType = parameter.FirstAncestorOrSelf<TypeDeclarationSyntax>();
        if (containingType == null) return document;

        // For primary constructors (records/classes), the parameter is directly in the type declaration
        // For regular constructors, we have a ConstructorDeclarationSyntax
        var constructor = parameter.FirstAncestorOrSelf<ConstructorDeclarationSyntax>();
        var isPrimaryConstructor = constructor == null;

        // 2. Find the field or property that stores the injected value (only for regular constructors)
        ISymbol? storageSymbol = null;
        if (!isPrimaryConstructor && constructor != null)
        {
            storageSymbol = FindStorageFieldOrProperty(constructor, parameterSymbol, semanticModel, cancellationToken);
        }

        
         
        var actionContextAssignments = containingType.DescendantNodes()
            .OfType<LocalDeclarationStatementSyntax>()
            .Where(ld => IsActionContextAssignment(ld, storageSymbol, semanticModel, cancellationToken))
            .ToList();
        


        
        // 3. Determine the new names
        // var oldParameterName = parameterSymbol.Name;
        // var newParameterName = ConvertToHttpContextAccessorName(oldParameterName);
        // var newFieldName = storageSymbol != null
        //     ? ConvertToHttpContextAccessorName(storageSymbol.Name)
        //     : "_httpContextAccessor";

        // var httpContextAccessorType = SyntaxFactory.IdentifierName("IHttpContextAccessor");
        var actionContextAccessorType = semanticModel.Compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Mvc.Infrastructure.IActionContextAccessor") 
                                    ?? throw new InvalidOperationException($"Can't resolve type Microsoft.AspNetCore.Mvc.Infrastructure.IActionContextAccessor");
        var httpContextAccessorType = semanticModel.Compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Http.IHttpContextAccessor")
                                      ?? throw new InvalidOperationException($"Can't resolve type Microsoft.AspNetCore.Http.IHttpContextAccessor");;
        
            
        // 4. Collect all replacements for parameter type

        // var newParameter = parameter;
        // if (parameter.Type != null)
        // {
        //     newParameter = newParameter.WithType(httpContextAccessorType.WithTriviaFrom(parameter.Type));
        // }
        editor.ReplaceType(actionContextAccessorType, httpContextAccessorType);

        // For primary constructors in records/classes, parameters use PascalCase (they become properties)
        // For regular constructor parameters, use camelCase
        var newParameterName = isPrimaryConstructor ? "HttpContextAccessor" : "httpContextAccessor";
        editor.RenameSymbol(semanticModel, parameterSymbol, newParameterName);


        if (storageSymbol != null)
        {
            var httpContextAccessorMemberName = storageSymbol.Kind == SymbolKind.Property ? "HttpContextAccessor" : "_httpContextAccessor";
            editor.RenameSymbol(semanticModel, storageSymbol, httpContextAccessorMemberName);


            var memberAccesses = containingType.DescendantNodes()
                .Where(x => x is MemberAccessExpressionSyntax or MemberBindingExpressionSyntax)
                .Select(x =>
                {
                    return (Node: x, Name: x switch
                    {
                        MemberAccessExpressionSyntax ma => ma.Name,
                        MemberBindingExpressionSyntax mb => mb.Name,
                        _ => null!
                    });
                })
                .Where(m => m.Name.IsSymbolOneOf(semanticModel,
                    "P:Microsoft.AspNetCore.Mvc.ActionContext.ActionDescriptor",
                    "P:Microsoft.AspNetCore.Mvc.ActionContext.RouteData",
                    "P:Microsoft.AspNetCore.Mvc.ActionContext.HttpContext"
                ))
                .ToList();
            foreach (var memberAccess in memberAccesses) // ActionContext.
            {
                var propertyName = memberAccess.Name.Identifier.Text;

                switch (propertyName)
                {
                    case "ActionDescriptor":
                        editor.ReplaceNode(memberAccess.Node, (current, gen) =>
                            SyntaxFactory.ParseExpression($"{httpContextAccessorMemberName}.HttpContext?.GetEndpoint()?.Metadata.GetMetadata<ActionDescriptor>()")
                                .WithRequiredNamespace("Microsoft.AspNetCore.Http")
                                .FormatterAnnotated());
                        break;
                    case "RouteData":
                        editor.ReplaceNode(memberAccess.Node, (current, gen) =>
                            SyntaxFactory.ParseExpression($"{httpContextAccessorMemberName}.HttpContext?.GetRouteData()")
                                .WithRequiredNamespace("Microsoft.AspNetCore.Routing")
                                .WithAdditionalAnnotations([
                                    // SymbolAnnotation.Create("M:Microsoft.AspNetCore.Routing.RoutingHttpContextExtensions.GetRouteData(Microsoft.AspNetCore.Http.HttpContext)"),
                                    SymbolAnnotation.Create("T:Microsoft.AspNetCore.Routing.RoutingHttpContextExtensions"),
                                    Formatter.Annotation,
                                    Simplifier.Annotation,
                                    Simplifier.AddImportsAnnotation
                                ])
                                .FormatterAnnotated());
                        break;
                    case "HttpContext":
                        SyntaxNode nodeToReplace;

                        if (memberAccess.Node is MemberBindingExpressionSyntax)
                        {
                            var actionContextIdentifierNode = containingType.FindNode(memberAccess.Node.GetFirstToken().GetPreviousToken().Span);
                            var ancestorConditionalAccess = actionContextIdentifierNode.AncestorsAndSelf().OfType<ConditionalAccessExpressionSyntax>().First();
                            nodeToReplace = ancestorConditionalAccess;
                            var rightSideParent = memberAccess.Node.Parent!;
                            var newNode = rightSideParent.ReplaceNode(memberAccess.Node, SyntaxFactory.ParseExpression($"{httpContextAccessorMemberName}.HttpContext")
                                .FormatterAnnotated());
                            editor.ReplaceNode(nodeToReplace, newNode);
                        }
                        else
                        {
                            nodeToReplace = memberAccess.Node;
                            editor.ReplaceNode(nodeToReplace, (current, gen) =>
                                SyntaxFactory.ParseExpression($"{httpContextAccessorMemberName}.HttpContext")
                                    .FormatterAnnotated());
                        }

                        break;
                }

            }


        }

        foreach (var assignment in actionContextAssignments)
        {
            // nodesToRemove.Add(assignment);
            editor.RemoveNode(assignment);
        }


        document = editor.GetChangedDocument();
        document = await document.RemoveUnusedImports(cancellationToken);
        return document;
    }

    /// <summary>
    /// Finds the field or property that stores the injected IActionContextAccessor.
    /// </summary>
    private ISymbol? FindStorageFieldOrProperty(
        ConstructorDeclarationSyntax constructor,
        IParameterSymbol parameterSymbol,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        var assignments = constructor.Body?.DescendantNodes()
            .OfType<AssignmentExpressionSyntax>()
            .Where(a => a.Right is IdentifierNameSyntax id && id.Identifier.Text == parameterSymbol.Name);

        if (assignments == null) return null;

        foreach (var assignment in assignments)
        {
            ISymbol? symbol = null;

            if (assignment.Left is MemberAccessExpressionSyntax memberAccess &&
                memberAccess.Expression is ThisExpressionSyntax)
            {
                symbol = semanticModel.GetSymbolInfo(memberAccess, cancellationToken).Symbol;
            }
            else if (assignment.Left is IdentifierNameSyntax identifier)
            {
                symbol = semanticModel.GetSymbolInfo(identifier, cancellationToken).Symbol;
            }

            if (symbol is IFieldSymbol or IPropertySymbol)
            {
                return symbol;
            }
        }

        return null;
    }


    /// <summary>
    /// Checks if a local declaration is an ActionContext assignment from ActionContextAccessor.
    /// </summary>
    private bool IsActionContextAssignment(
        LocalDeclarationStatementSyntax declaration,
        ISymbol? storageSymbol,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        return declaration.Declaration.Variables.Any(v =>
        {
            if (v.Initializer?.Value is MemberAccessExpressionSyntax memberAccess &&
                memberAccess.Name.Identifier.Text == "ActionContext")
            {
                var symbol = semanticModel.GetSymbolInfo(memberAccess.Expression, cancellationToken).Symbol;
                return SymbolEqualityComparer.Default.Equals(symbol, storageSymbol) ||
                       (symbol != null && symbol.Name.Contains("actionContextAccessor", StringComparison.OrdinalIgnoreCase));
            }
            return false;
        });
    }

}