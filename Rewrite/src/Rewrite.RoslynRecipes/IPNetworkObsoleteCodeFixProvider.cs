using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Rewrite.RoslynRecipes.Helpers;

namespace Rewrite.RoslynRecipes;

/// <summary>
/// Provides code fixes for migrating from the obsolete Microsoft.AspNetCore.HttpOverrides.IPNetwork
/// to System.Net.IPNetwork and from KnownNetworks to KnownIPNetworks.
/// </summary>
/// <remarks>
/// This code fix provider handles two migration scenarios:
/// 1. Replacing Microsoft.AspNetCore.HttpOverrides.IPNetwork type references with System.Net.IPNetwork
/// 2. Replacing ForwardedHeadersOptions.KnownNetworks property accesses with KnownIPNetworks
/// See: https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/10/ipnetwork-knownnetworks-obsolete
/// </remarks>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(IPNetworkObsoleteCodeFixProvider)), Shared]
public sealed class IPNetworkObsoleteCodeFixProvider : CodeFixProvider
{
    private const string ReplaceIPNetworkTypeTitle = "Replace with System.Net.IPNetwork";
    private const string ReplaceKnownNetworksTitle = "Replace KnownNetworks with KnownIPNetworks";

    /// <summary>
    /// Gets the diagnostic IDs that this code fix provider can handle.
    /// </summary>
    public sealed override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(
            IPNetworkObsoleteAnalyzer.IPNetworkTypeDiagnosticId,
            IPNetworkObsoleteAnalyzer.KnownNetworksDiagnosticId);

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
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root == null) return;

        foreach (var diagnostic in context.Diagnostics)
        {
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var node = root.FindNode(diagnosticSpan);

            if (diagnostic.Id == IPNetworkObsoleteAnalyzer.IPNetworkTypeDiagnosticId)
            {
                context.RegisterCodeFix(
                    CodeAction.Create(
                        title: ReplaceIPNetworkTypeTitle,
                        createChangedDocument: ct => ReplaceIPNetworkTypeAsync(context.Document, node, ct),
                        equivalenceKey: ReplaceIPNetworkTypeTitle),
                    diagnostic);
            }
            else if (diagnostic.Id == IPNetworkObsoleteAnalyzer.KnownNetworksDiagnosticId)
            {
                context.RegisterCodeFix(
                    CodeAction.Create(
                        title: ReplaceKnownNetworksTitle,
                        createChangedDocument: ct => ReplaceKnownNetworksAsync(context.Document, node, ct),
                        equivalenceKey: ReplaceKnownNetworksTitle),
                    diagnostic);
            }
        }
    }

    /// <summary>
    /// Replaces the obsolete Microsoft.AspNetCore.HttpOverrides.IPNetwork type with System.Net.IPNetwork.
    /// </summary>
    /// <param name="document">The document containing the code to fix.</param>
    /// <param name="node">The syntax node representing the IPNetwork type reference.</param>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    /// <returns>A task that returns the updated document with the type replaced.</returns>
    private async Task<Document> ReplaceIPNetworkTypeAsync(
        Document document,
        SyntaxNode node,
        CancellationToken cancellationToken)
    {
        var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
        var semanticModel = editor.SemanticModel;

        // Get the old and new types
        var oldType = semanticModel.Compilation.GetTypeByMetadataNameOrThrow("Microsoft.AspNetCore.HttpOverrides.IPNetwork");
        var newType = semanticModel.Compilation.GetTypeByMetadataNameOrThrow("System.Net.IPNetwork");


        editor.ReplaceType(oldType, newType, node.FirstAncestorOrSelf<CompilationUnitSyntax>());

        document = editor.GetChangedDocument();
        document = await document.RemoveUnusedImports(cancellationToken);
        return document;
    }

    /// <summary>
    /// Replaces the obsolete KnownNetworks property access with KnownIPNetworks.
    /// </summary>
    /// <param name="document">The document containing the code to fix.</param>
    /// <param name="node">The syntax node representing the KnownNetworks property access.</param>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    /// <returns>A task that returns the updated document with the property replaced.</returns>
    private async Task<Document> ReplaceKnownNetworksAsync(
        Document document,
        SyntaxNode node,
        CancellationToken cancellationToken)
    {
        var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
        
        // The node should be the "KnownNetworks" identifier within the member access
        var newIdentifier = SyntaxFactory.IdentifierName("KnownIPNetworks")
            .FormatterAnnotated();

        editor.ReplaceNode(node, (current, gen) => newIdentifier.WithTriviaFrom(current));
        
        return editor.GetChangedDocument();
    }

    /// <summary>
    /// Removes the Microsoft.AspNetCore.HttpOverrides using directive if it's no longer needed.
    /// </summary>
    /// <param name="editor">The document editor to use for modifications.</param>
    private async Task MaybeRemoveOldUsing(DocumentEditor editor, CancellationToken cancellationToken)
    {
        var newDocument = editor.GetChangedDocument();

        // Check if Microsoft.AspNetCore.HttpOverrides.IPNetwork is still used anywhere
        var compilation = await newDocument.Project.GetCompilationAsync(cancellationToken) ?? throw new InvalidOperationException("Cannot obtain compilation");
        var unusedImports = compilation.GetDiagnostics(cancellationToken)
            .Where(x => x.Id == "CS8019") // unused imports
            .ToList();
        var nodesToRemove = unusedImports.Select(x => editor.OriginalRoot.FindNode(x.Location.SourceSpan)).ToList();
        foreach (var node in nodesToRemove)
        {
            editor.RemoveNode(node, SyntaxRemoveOptions.KeepDirectives | SyntaxRemoveOptions.KeepExteriorTrivia);
        }
        
    }
}
