using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Formatting;
using Rewrite.RoslynRecipe.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rewrite.RoslynRecipe;

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

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        // Find the parameter type node that triggered the diagnostic
        var node = root.FindNode(diagnosticSpan);
        var parameter = node.FirstAncestorOrSelf<ParameterSyntax>();
        if (parameter == null) return;

        // Check if ModelState is used (from diagnostic properties)
        bool hasModelStateUsage = diagnostic.Properties.ContainsKey("HasModelStateUsage") &&
                                   diagnostic.Properties["HasModelStateUsage"] == "true";
        
        
        if (hasModelStateUsage)
        {
            var parameterList = (ParameterListSyntax)parameter.Parent!;
            var commentTrivia = SyntaxFactory.Comment("/* todo: obsolete type */");
            var todoCommentExists = parameterList.ToFullString().Contains(commentTrivia.ToString());
            if (!todoCommentExists)
            {
                // Special case: only add TODO comment for ModelState usage
                context.RegisterCodeFix(
                    CodeAction.Create(
                        title: "Add TODO comment (manual review required for ModelState)",
                        createChangedDocument: ct => AddTodoCommentAsync(document, parameter, ct),
                        equivalenceKey: "AddTodoComment"),
                    diagnostic);
            }
        }
        else
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
    /// Adds a TODO comment to the constructor parameter when ModelState usage is detected.
    /// </summary>
    private async Task<Document> AddTodoCommentAsync(
        Document document,
        ParameterSyntax parameter,
        CancellationToken cancellationToken)
    {
        
        // Add TODO comment before the parameter
        var commentTrivia = SyntaxFactory.Comment("/* todo: obsolete type */");

        var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);

        var newParameter = parameter.WithLeadingTrivia(
            parameter.GetLeadingTrivia().Add(commentTrivia).Add(SyntaxFactory.Space));

        editor.ReplaceNode(parameter, newParameter);

        return editor.GetChangedDocument();
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

        // Find all the nodes we need to replace BEFORE making any edits
        // var replacements = new Dictionary<SyntaxNode, SyntaxNode>();
        var nodesToRemove = new HashSet<SyntaxNode>();

        // 1. Find the constructor and containing type
        var constructor = parameter.FirstAncestorOrSelf<ConstructorDeclarationSyntax>();
        var containingType = parameter.FirstAncestorOrSelf<TypeDeclarationSyntax>();
        if (constructor == null || containingType == null) return document;

        // 2. Find the field or property that stores the injected value
        var storageSymbol = FindStorageFieldOrProperty(constructor, parameterSymbol, semanticModel, cancellationToken);

        // 3. Determine the new names
        var oldParameterName = parameterSymbol.Name;
        var newParameterName = ConvertToHttpContextAccessorName(oldParameterName);
        var newFieldName = storageSymbol != null
            ? ConvertToHttpContextAccessorName(storageSymbol.Name)
            : "_httpContextAccessor";

        var httpContextAccessorType = SyntaxFactory.IdentifierName("IHttpContextAccessor");
        // 4. Collect all replacements for parameter type

        var newParameter = parameter;
        if (parameter.Type != null)
        {
            newParameter = newParameter.WithType(httpContextAccessorType.WithTriviaFrom(parameter.Type));
        }
        
        
        
        // 5. Collect all replacements for parameter name
        if (oldParameterName != newParameterName)
        {
            newParameter = newParameter.WithIdentifier(SyntaxFactory.Identifier(newParameterName)
                .WithTriviaFrom(parameter.Identifier));
            
        }
        editor.ReplaceNode(parameter, newParameter.WithAdditionalAnnotations(Formatter.Annotation));
        
        // 6. Find and collect replacements for field/property declaration
        if (storageSymbol != null)
        {
            var storageDeclaration = FindSymbolDeclarationNode(containingType, storageSymbol);
            if (storageDeclaration != null)
            {
                if (storageDeclaration is FieldDeclarationSyntax fieldDecl)
                {
                    var newType = httpContextAccessorType
                        .WithTriviaFrom(fieldDecl.Declaration.Type)
                        .WithAdditionalAnnotations(Formatter.Annotation);
        
                    var variable = fieldDecl.Declaration.Variables.First();
                    var newVariable = variable.WithIdentifier(
                        SyntaxFactory.Identifier(newFieldName).WithTriviaFrom(variable.Identifier));
        
                    var newDeclaration = fieldDecl.Declaration
                        .WithType(newType)
                        .WithVariables(SyntaxFactory.SingletonSeparatedList(newVariable));
        
                    // replacements[fieldDecl] = fieldDecl.WithDeclaration(newDeclaration)
                    //     .WithAdditionalAnnotations(Formatter.Annotation);

                    editor.ReplaceNode(fieldDecl, fieldDecl.WithDeclaration(newDeclaration).WithAdditionalAnnotations(Formatter.Annotation));
                }
                else if (storageDeclaration is PropertyDeclarationSyntax propDecl)
                {
                    var newType = httpContextAccessorType
                        .WithTriviaFrom(propDecl.Type)
                        .WithAdditionalAnnotations(Formatter.Annotation);
        
                    var newVal = propDecl
                        .WithType(newType)
                        .WithIdentifier(SyntaxFactory.Identifier(newFieldName).WithTriviaFrom(propDecl.Identifier))
                        .WithAdditionalAnnotations(Formatter.Annotation);
                    // replacements[propDecl] = newVal;
                    editor.ReplaceNode(propDecl, newVal);
                }
            }
        }
        
        // 7. Find ActionContext variable assignments to remove
        var actionContextAssignments = containingType.DescendantNodes()
            .OfType<LocalDeclarationStatementSyntax>()
            .Where(ld => IsActionContextAssignment(ld, storageSymbol, semanticModel, cancellationToken))
            .ToList();
        
        foreach (var assignment in actionContextAssignments)
        {
            nodesToRemove.Add(assignment);
            // editor.RemoveNode(assignment);
        }
        
        // 8. Find and replace ActionContext property accesses
        var memberAccesses = containingType.DescendantNodes()
            .OfType<MemberAccessExpressionSyntax>()
            .Where(m => IsActionContextPropertyAccess(m, semanticModel, cancellationToken))
            .ToList();
        
        foreach (var memberAccess in memberAccesses)
        {
            var propertyName = memberAccess.Name.Identifier.Text;
            ExpressionSyntax? replacement = null;
        
            switch (propertyName)
            {
                case "ActionDescriptor":
                    replacement = SyntaxFactory.ParseExpression($"{newFieldName}.HttpContext?.GetEndpoint()?.Metadata.GetMetadata<ActionDescriptor>()")
                        .WithAdditionalAnnotations(Formatter.Annotation)
                        .DiscardFormatting();
                    break;
        
                case "RouteData":
                    replacement = SyntaxFactory.ParseExpression($"{newFieldName}.HttpContext?.GetRouteData()")
                        .WithAdditionalAnnotations(Formatter.Annotation)
                        .DiscardFormatting();
                    break;
        
                case "HttpContext":
                    replacement = SyntaxFactory.ParseExpression($"{newFieldName}.HttpContext")
                        .WithAdditionalAnnotations(Formatter.Annotation)
                        .DiscardFormatting();
                    break;
            }
        
            if (replacement != null)
            {
                // replacements[memberAccess] = replacement;
                editor.ReplaceNode(memberAccess, replacement);
            }
        }
        
        // 9. Find and replace all identifier references
        var identifierReferences = containingType.DescendantNodes()
            .OfType<IdentifierNameSyntax>()
            .Where(id => IsReferenceToReplace(id, parameterSymbol, storageSymbol, semanticModel, cancellationToken))
            .ToList();
        var currentDoc = editor.GetChangedRoot().GetText().ToString();
        foreach (var identifier in identifierReferences)
        {
            var newName = ConvertToHttpContextAccessorName(identifier.Identifier.Text);
            if (newName != identifier.Identifier.Text)
            {
                var newValue =SyntaxFactory.IdentifierName(newName)
                    .WithTriviaFrom(identifier)
                    .WithAdditionalAnnotations(Formatter.Annotation);
                // replacements[identifier] = newValue;
                editor.ReplaceNode(identifier, newValue);
            }
        }

        // 10. Apply all replacements using DocumentEditor
        // First remove nodes
        foreach (var node in nodesToRemove)
        {
            editor.RemoveNode(node);
        }

        // Then apply replacements
        // foreach (var kvp in replacements)
        // {
        //     editor.ReplaceNode(kvp.Key, kvp.Value);
        // }

        // 11. Get the changed document and manage using directives
        var changedDocument = editor.GetChangedDocument();

        // Add required using directives
        changedDocument = await AddRequiredUsingsAsync(changedDocument, cancellationToken);

        // Remove unused using directives
        changedDocument = await RemoveUnusedUsingsAsync(changedDocument, cancellationToken);

        // Format the document
        changedDocument = await Formatter.FormatAsync(
            changedDocument,
            Formatter.Annotation,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return changedDocument;
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
    /// Finds the declaration node for a field or property symbol.
    /// </summary>
    private SyntaxNode? FindSymbolDeclarationNode(TypeDeclarationSyntax containingType, ISymbol symbol)
    {
        if (symbol is IFieldSymbol fieldSymbol)
        {
            return containingType.DescendantNodes()
                .OfType<FieldDeclarationSyntax>()
                .FirstOrDefault(f => f.Declaration.Variables.Any(v => v.Identifier.Text == fieldSymbol.Name));
        }
        else if (symbol is IPropertySymbol propertySymbol)
        {
            return containingType.DescendantNodes()
                .OfType<PropertyDeclarationSyntax>()
                .FirstOrDefault(p => p.Identifier.Text == propertySymbol.Name);
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

    /// <summary>
    /// Checks if a member access expression is accessing a property of ActionContext.
    /// </summary>
    private bool IsActionContextPropertyAccess(
        MemberAccessExpressionSyntax memberAccess,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        var propertyName = memberAccess.Name.Identifier.Text;

        if (propertyName != "ActionDescriptor" && propertyName != "RouteData" && propertyName != "HttpContext")
            return false;

        // Check if this is accessing ActionContext
        if (memberAccess.Expression is MemberAccessExpressionSyntax parentAccess &&
            parentAccess.Name.Identifier.Text == "ActionContext")
        {
            return true;
        }

        // Check if this is accessing a local variable of type ActionContext
        if (memberAccess.Expression is IdentifierNameSyntax identifier)
        {
            var symbol = semanticModel.GetSymbolInfo(identifier, cancellationToken).Symbol;
            if (symbol is ILocalSymbol localSymbol)
            {
                var typeKey = localSymbol.Type.GetDocumentationCommentId();
                return typeKey == "T:Microsoft.AspNetCore.Mvc.ActionContext";
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if an identifier is a reference that needs to be replaced.
    /// </summary>
    private bool IsReferenceToReplace(
        IdentifierNameSyntax identifier,
        IParameterSymbol parameterSymbol,
        ISymbol? storageSymbol,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        var symbol = semanticModel.GetSymbolInfo(identifier, cancellationToken).Symbol;

        return symbol != null && (
            SymbolEqualityComparer.Default.Equals(symbol, parameterSymbol) ||
            SymbolEqualityComparer.Default.Equals(symbol, storageSymbol) ||
            symbol.Name == parameterSymbol.Name ||
            (storageSymbol != null && symbol.Name == storageSymbol.Name));
    }

    /// <summary>
    /// Converts an IActionContextAccessor-related name to an IHttpContextAccessor-related name.
    /// </summary>
    private string ConvertToHttpContextAccessorName(string oldName)
    {
        return oldName
            .Replace("actionContextAccessor", "httpContextAccessor")
            .Replace("ActionContextAccessor", "HttpContextAccessor")
            .Replace("actionContext", "httpContext")
            .Replace("ActionContext", "HttpContext");
    }

    /// <summary>
    /// Adds required using directives for the refactored code.
    /// </summary>
    private async Task<Document> AddRequiredUsingsAsync(Document document, CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

        if (root == null || semanticModel == null) return document;

        // Add Microsoft.AspNetCore.Http for IHttpContextAccessor
        var (doc1, root1) = await UsingsUtil.MaybeAddUsingAsync(
            document, root, semanticModel, root,
            "Microsoft.AspNetCore.Http.IHttpContextAccessor", cancellationToken).ConfigureAwait(false);

        // Check if we need Microsoft.AspNetCore.Routing for GetRouteData
        var hasRouteDataUsage = root1.DescendantNodes()
            .OfType<InvocationExpressionSyntax>()
            .Any(i => i.Expression is MemberAccessExpressionSyntax m &&
                     m.Name.Identifier.Text == "GetRouteData");

        if (hasRouteDataUsage)
        {
            var sem1 = await doc1.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            if (sem1 != null)
            {
                var (doc2, root2) = await UsingsUtil.MaybeAddUsingAsync(
                    doc1, root1, sem1, root1,
                    "Microsoft.AspNetCore.Routing.RouteData", cancellationToken).ConfigureAwait(false);
                doc1 = doc2;
                root1 = root2;
            }
        }

        // Check if we need Microsoft.AspNetCore.Mvc.Abstractions for ActionDescriptor
        var hasActionDescriptorUsage = root1.DescendantNodes()
            .OfType<GenericNameSyntax>()
            .Any(g => g.Identifier.Text == "GetMetadata" &&
                     g.TypeArgumentList.Arguments.Count == 1 &&
                     g.TypeArgumentList.Arguments[0].ToString().Contains("ActionDescriptor"));

        if (hasActionDescriptorUsage)
        {
            var sem2 = await doc1.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            if (sem2 != null)
            {
                var (doc3, root3) = await UsingsUtil.MaybeAddUsingAsync(
                    doc1, root1, sem2, root1,
                    "Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor", cancellationToken).ConfigureAwait(false);
                doc1 = doc3;
            }
        }

        return doc1;
    }

    /// <summary>
    /// Removes unused using directives after the refactoring.
    /// </summary>
    private async Task<Document> RemoveUnusedUsingsAsync(Document document, CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

        if (root == null) return document;

        // Check if Microsoft.AspNetCore.Mvc.Infrastructure is still needed
        var hasActionContextAccessorUsage = root.DescendantNodes()
            .OfType<IdentifierNameSyntax>()
            .Any(id => id.Identifier.Text == "IActionContextAccessor");

        if (!hasActionContextAccessorUsage)
        {
            document = await UsingsUtil.MaybeRemoveUsingAsync(
                document,
                "Microsoft.AspNetCore.Mvc.Infrastructure",
                cancellationToken).ConfigureAwait(false);
        }

        return document;
    }
}