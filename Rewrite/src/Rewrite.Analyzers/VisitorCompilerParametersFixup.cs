// using System;
// using System.Linq;
// using System.Globalization;
// using System.Collections.Generic;
// using System.Collections.Immutable;
// using System.Composition;
// using System.Threading;
// using System.Threading.Tasks;
// using Microsoft.CodeAnalysis;
// using Microsoft.CodeAnalysis.CodeActions;
// using Microsoft.CodeAnalysis.CodeFixes;
// using Microsoft.CodeAnalysis.Editing;
// using Microsoft.CodeAnalysis.Operations;
//
// namespace Rewrite.Analyzers;
//
// [ExportCodeFixProvider(LanguageNames.CSharp, LanguageNames.VisualBasic), Shared]
// public sealed class AvoidConstArraysFixer : CodeFixProvider
// {
//     public sealed override ImmutableArray<string> FixableDiagnosticIds { get; } = ImmutableArray.Create(AvoidConstArraysAnalyzer.RuleId);
//
//     private static readonly ImmutableArray<string> s_collectionMemberEndings = ImmutableArray.Create("array", "collection", "enumerable", "list");
//
//     // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
//     public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;
//
//     public override async Task RegisterCodeFixesAsync(CodeFixContext context)
//     {
//         Document document = context.Document;
//         SyntaxNode root = await document.GetRequiredSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
//         SyntaxNode node = root.FindNode(context.Span);
//
//         context.RegisterCodeFix(CodeAction.Create(
//                 MicrosoftNetCoreAnalyzersResources.AvoidConstArraysCodeFixTitle,
//                 async ct => await ExtractConstArrayAsync(document, root, node, context.Diagnostics[0].Properties, ct).ConfigureAwait(false),
//                 equivalenceKey: nameof(MicrosoftNetCoreAnalyzersResources.AvoidConstArraysCodeFixTitle)),
//             context.Diagnostics);
//     }
//
//     private static async Task<Document> ExtractConstArrayAsync(
//         Document document,
//         SyntaxNode root,
//         SyntaxNode node,
//         ImmutableDictionary<string, string?> properties,
//         CancellationToken cancellationToken)
//     {
//         SemanticModel model = await document.GetRequiredSemanticModelAsync(cancellationToken).ConfigureAwait(false);
//         DocumentEditor editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
//         SyntaxGenerator generator = editor.Generator;
//         IArrayCreationOperation arrayArgument = GetArrayCreationOperation(node, model, cancellationToken, out bool isInvoked);
//         INamedTypeSymbol containingType = model.GetEnclosingSymbol(node.SpanStart, cancellationToken)!.ContainingType;
//
//         // Get a valid member name for the extracted constant
//         string newMemberName = GetExtractedMemberName(containingType.MemberNames, properties["paramName"] ?? GetMemberNameFromType(arrayArgument));
//
//         // Get method containing the symbol that is being diagnosed
//         IOperation? methodContext = arrayArgument.GetAncestor<IMethodBodyOperation>(OperationKind.MethodBody);
//         methodContext ??= arrayArgument.GetAncestor<IBlockOperation>(OperationKind.Block); // VB methods have a different structure than CS methods
//
//         // Create the new member
//         SyntaxNode newMember = generator.FieldDeclaration(
//             newMemberName,
//             generator.TypeExpression(arrayArgument.Type),
//             GetAccessibility(methodContext is null ? null : model.GetEnclosingSymbol(methodContext.Syntax.SpanStart, cancellationToken)),
//             DeclarationModifiers.Static | DeclarationModifiers.ReadOnly,
//             arrayArgument.Syntax.WithoutTrailingTrivia() // don't include extra trivia before the end of the declaration
//         );
//
//         // Add any additional formatting
//         if (methodContext is not null)
//         {
//             newMember = newMember.FormatForExtraction(methodContext.Syntax);
//         }
//
//         ISymbol? lastFieldOrPropertySymbol = containingType.GetMembers().LastOrDefault(x => x is IFieldSymbol or IPropertySymbol);
//         if (lastFieldOrPropertySymbol is not null)
//         {
//             var span = lastFieldOrPropertySymbol.Locations.First().SourceSpan;
//             if (root.FullSpan.Contains(span))
//             {
//                 // Insert after fields or properties
//                 SyntaxNode lastFieldOrPropertyNode = root.FindNode(span);
//                 editor.InsertAfter(generator.GetDeclaration(lastFieldOrPropertyNode)!, newMember);
//             }
//             else if (methodContext != null)
//             {
//                 // Span not found
//                 editor.InsertBefore(methodContext.Syntax, newMember);
//             }
//         }
//         else if (methodContext != null)
//         {
//             // No fields or properties, insert right before the containing method for simplicity
//             editor.InsertBefore(methodContext.Syntax, newMember);
//         }
//
//         // Replace argument with a reference to our new member
//         SyntaxNode identifier = generator.IdentifierName(newMemberName);
//         if (isInvoked)
//         {
//             editor.ReplaceNode(node, generator.WithExpression(identifier, node));
//         }
//         else
//         {
//             // add any extra trivia that was after the original argument
//             editor.ReplaceNode(node, generator.Argument(identifier).WithTriviaFrom(arrayArgument.Syntax));
//         }
//
//         // Return changed document
//         return editor.GetChangedDocument();
//     }
//
// }
