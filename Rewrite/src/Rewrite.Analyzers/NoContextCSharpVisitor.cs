using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
namespace Rewrite.Analyzers;

[Generator]
public class NoContextVisitor : IIncrementalGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new LstLocator());
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        
        var candidateNodes = context.SyntaxProvider.CreateSyntaxProvider(
            predicate: (node, _) => node is ClassDeclarationSyntax
            {
                Identifier.ValueText: "CSharpVisitor",
                TypeParameterList.Parameters: [_] // generic with 1 type argument
            },
            transform: (syntaxContext, cancellationToken) => (syntaxContext.Node, syntaxContext.SemanticModel))
            .Combine(context.CompilationProvider);


        context.RegisterSourceOutput(candidateNodes, (spc, pair) =>
        {
            try
            {
                var classDeclaration = pair.Left.Node;
                var visitorSemanticModel = pair.Left.SemanticModel;
                var classSemanticModel = (INamedTypeSymbol)visitorSemanticModel.GetDeclaredSymbol(classDeclaration)!;
                var compilation = pair.Right;

                var result = GetEffectiveVirtualMethods(classSemanticModel)
                    .Where(x => x.Name.StartsWith("Visit"))
                    .Select(methodSymbol =>
                    {

                        if (methodSymbol.ContainingType.Name.Contains("Tree") || methodSymbol.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() is not MethodDeclarationSyntax methodSyntax)
                            return null;
                        // methodSyntax = methodSyntax.WithBody(null).WithExpressionBody(null);
                        var semanticModel = methodSyntax.SyntaxTree == classDeclaration.SyntaxTree ? visitorSemanticModel : compilation.GetSemanticModel(methodSyntax.SyntaxTree);
                        // var result = new FullyQualifiedNameRewriter(semanticModel).Visit(methodSyntax);
                        var result = new StatelessVisitorRewriter(semanticModel).Visit(methodSyntax);
                        return result;
                    })
                    .Where(x => x != null)
                    .ToList();
                var template = $$"""
                                 #nullable enable
                                 using System.Diagnostics;
                                 using System.Diagnostics.CodeAnalysis;
                                 using Rewrite.Core;
                                 using Rewrite.RewriteCSharp.Tree;
                                 using Rewrite.RewriteJava;
                                 using Rewrite.RewriteJava.Tree;

                                 namespace Rewrite.RewriteCSharp.Tree;
                                 public partial class CSharpVisitor
                                 {
                                     private global::Rewrite.RewriteCSharp.CSharpVisitor<object> _visitor = new();
                                     private static object _stub = new();
                                     public global::Rewrite.Core.Cursor Cursor { get => _visitor.Cursor; set => _visitor.Cursor = value; }
                                     public virtual global::Rewrite.RewriteJava.Tree.J? Visit(global::Rewrite.Core.Tree? node, global::Rewrite.Core.Cursor cursor) => _visitor.Visit(node, _stub, cursor);
                                     public virtual global::Rewrite.RewriteJava.Tree.J? Visit(global::Rewrite.Core.Tree? node) => _visitor.Visit(node, _stub);
                                     public virtual global::Rewrite.Core.Marker.Markers VisitMarkers(global::Rewrite.Core.Marker.Markers? node) => _visitor.VisitMarkers(node, _stub);
                                     public virtual global::Rewrite.Core.Marker.Marker VisitMarker(global::Rewrite.Core.Marker.Marker node) => _visitor.VisitMarker(node, _stub);
                                     public virtual global::Rewrite.RewriteJava.Tree.J? PreVisit(global::Rewrite.RewriteJava.Tree.J? node) => _visitor.PreVisit(node, _stub);
                                     public virtual global::Rewrite.RewriteJava.Tree.J? PostVisit(global::Rewrite.RewriteJava.Tree.J node) => _visitor.PostVisit(node, _stub);
                                     public virtual global::Rewrite.RewriteJava.Tree.J? DefaultValue(global::Rewrite.RewriteJava.Tree.J node) => _visitor.DefaultValue(node, _stub);
                                 {{result.Render(x => $$"""
                                                            {{x}}

                                                        """)}}
                                 }
                                 """;
                spc.AddSource("Rewrite.RewriteCSharp.CSharpVisitor.g.cs", template);
            }
            catch (Exception e)
            {
                spc.AddSource($"{GetType().FullName}.g.cs", SourceText.From(e.ToString(), Encoding.UTF8));
            }
//             {{result.Render(method => $$"""
//                                  public virtual J? {{method.MethodName}}{{method.TypeParameters}}({{method.ParameterType}}{{method.TypeParameters}} node) => _visitor.{{method.MethodName}}{{method.TypeParameters}}(node, _stub);
//                              
//                              """)}}
        });
        
        
        // var methods = context.SyntaxProvider.CreateSyntaxProvider(
        //     predicate: (node, _) => node is ClassDeclarationSyntax
        //     {
        //         Identifier.ValueText: "CSharpVisitor",
        //         TypeParameterList.Parameters: [_] // generic with 1 type argument
        //     },
        //     transform: (syntaxContext, cancellationToken) =>
        //     {
        //         var model = syntaxContext.SemanticModel;
        //         var node = syntaxContext.Node;
        //         var declaration = (INamedTypeSymbol)syntaxContext.SemanticModel.GetDeclaredSymbol(node)!;
        //         var methods = GetEffectiveVirtualMethods(declaration)
        //             .Where(x => x.Name.StartsWith("Visit"))
        //             .Select(methodSymbol =>
        //             {
        //                 if (methodSymbol.DeclaringSyntaxReferences.Length == 0)
        //                     return null;
        //
        //                 var method = (MethodDeclarationSyntax)methodSymbol.DeclaringSyntaxReferences.First().GetSyntax();
        //
        //                 var parameters = method.ParameterList.Parameters;
        //                 if (parameters.Count == 0)
        //                     return method; // or throw
        //
        //                 // --- 1. Fix return type if needed ---
        //                 var originalReturnType = method.ReturnType;
        //
        //                 bool isTType = originalReturnType is IdentifierNameSyntax idName && idName.Identifier.Text == "T";
        //                 bool isNullableTType = originalReturnType is NullableTypeSyntax nullableType &&
        //                                        nullableType.ElementType is IdentifierNameSyntax idName2 &&
        //                                        idName2.Identifier.Text == "T";
        //
        //                 TypeSyntax newReturnType = originalReturnType;
        //
        //                 if (isTType || isNullableTType)
        //                 {
        //                     newReturnType = NullableType(IdentifierName("J"));
        //                 }
        //
        //                 // --- 2. Fix parameters ---
        //                 var parametersToPreserve = parameters.Take(parameters.Count - 1).ToList();
        //
        //                 var newParameters = new List<ParameterSyntax>();
        //                 for (int i = 0; i < parametersToPreserve.Count; i++)
        //                 {
        //                     var originalParam = parametersToPreserve[i];
        //                     TypeSyntax? updatedType;
        //                     // Update type if it is "Tree"
        //                     
        //                     var parameterType = methodSymbol.Parameters[i].Type;
        //                     updatedType = ParseTypeName(parameterType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
        //
        //                     // if (originalParam.Type?.ToString() == "Tree?")
        //                     // {
        //                     //     updatedType = ParseTypeName("Rewrite.Core.Tree");
        //                     // }
        //                     // else
        //                     // {
        //                     //     updatedType = originalParam.Type;
        //                     // }
        //
        //                     if (i == 0)
        //                     {
        //                         // First parameter: rename to 'node', updated type
        //                         newParameters.Add(
        //                             Parameter(Identifier("node"))
        //                                 .WithType(updatedType)
        //                         );
        //                     }
        //                     else
        //                     {
        //                         // Other parameters: keep name, but updated type
        //                         newParameters.Add(
        //                             originalParam.WithType(updatedType)
        //                         );
        //                     }
        //                 }
        //
        //
        //                 if (methodSymbol.TypeParameters.SelectMany(x => x.ConstraintTypes).Count() > 0)
        //                 {
        //                     Debugger.Break();
        //                     var constraintClauses = method.ConstraintClauses.Join(methodSymbol.TypeParameters, x => x.Name.ToString(), x => x.Name, (syntax, symbol) => (Syntax: syntax, Symbol: symbol));
        //                     var typeConstraints = constraintClauses.Select((x, i) => x.Syntax.Constraints.Select((syntax, i) => (Syntax: syntax, Symbol: x.Symbol.ConstraintTypes[i]))).ToList();
        //                 }
        //                 // methodSymbol.TypeParameters.First().ConstraintTypes[0].de
        //
        //                 for (int constraintClauseIndex = 0; constraintClauseIndex < method.ConstraintClauses.Count; constraintClauseIndex++)
        //                 {
        //                     var clause = method.ConstraintClauses[constraintClauseIndex];
        //                     for (int constraintIndex = 0; constraintIndex < clause.Constraints.Count; constraintIndex++)
        //                     {
        //                         var constraint = clause.Constraints[constraintIndex];
        //                         
        //                     }
        //                 }
        //                 var newTypeParameterList = method.TypeParameterList?.Parameters.ToList() ?? [];
        //                 for (int i = 0; i < newTypeParameterList.Count; i++)
        //                 {
        //                     var originalTypeParameter = newTypeParameterList[i];
        //                     
        //                 }
        //
        //                 var newParameterList = ParameterList(SeparatedList(newParameters));
        //
        //                 // --- 3. Build invocation ---
        //                 var invocationArguments = new List<ArgumentSyntax>();
        //
        //                 for (int i = 0; i < parametersToPreserve.Count; i++)
        //                 {
        //                     if (i == 0)
        //                         invocationArguments.Add(Argument(IdentifierName("node")));
        //                     else
        //                         invocationArguments.Add(Argument(IdentifierName(parametersToPreserve[i].Identifier.Text)));
        //                 }
        //
        //                 invocationArguments.Add(Argument(IdentifierName("_stub"))); // _stub replaces last param
        //
        //                 var invocation = InvocationExpression(
        //                     MemberAccessExpression(
        //                         SyntaxKind.SimpleMemberAccessExpression,
        //                         IdentifierName("_visitor"),
        //                         IdentifierName(method.Identifier.Text)
        //                     ),
        //                     ArgumentList(SeparatedList(invocationArguments))
        //                 );
        //
        //                 var arrowExpression = ArrowExpressionClause(invocation);
        //
        //                 // --- 4. Build final method ---
        //                 var newMethod = method
        //                     .WithReturnType(newReturnType)
        //                     .WithParameterList(newParameterList)
        //                     .WithoutTrivia()
        //                     .WithAttributeLists([])
        //                     .WithBody(null)
        //                     .WithExpressionBody(arrowExpression)
        //                     .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
        //                     .NormalizeWhitespace();
        //
        //                 return newMethod;
        //                 // var arg0 = x.Parameters[0].Type;
        //                 // var typeParams = x.TypeParameters.IsEmpty ? "" : $"<{string.Join(",", x.TypeParameters.Select(x => x.Name))}>";
        //                 // List<string> path = new();
        //                 // var containtingType = arg0.ContainingType;
        //                 // path.Add(arg0.Name);
        //                 // while (containtingType != null)
        //                 // {
        //                 //     path.Add(containtingType.Name);
        //                 //     containtingType = containtingType.ContainingType;
        //                 // }
        //                 //
        //                 // path.Reverse();
        //                 //
        //                 // return new
        //                 // {
        //                 //     MethodName = x.Name,
        //                 //     TypeParameters = typeParams,
        //                 //     ParameterType = string.Join(".", path)
        //                 // };
        //             })
        //             .ToList();
        //         return methods;
        //        
        //     })
        //     .Where(x => x is not null);
        
    }
    
    public static IEnumerable<IMethodSymbol> GetEffectiveVirtualMethods(INamedTypeSymbol typeSymbol)
    {
        var seenBaseMethods = new HashSet<IMethodSymbol>(SymbolEqualityComparer.Default);

        var current = typeSymbol;
        while (current != null)
        {
            foreach (var member in current.GetMembers())
            {
                if (member is IMethodSymbol method &&
                    method.MethodKind == MethodKind.Ordinary &&
                    !method.IsStatic &&
                    (method.IsVirtual || method.IsOverride))
                {
                    var overridden = method.OverriddenMethod;
                    if (overridden != null)
                    {
                        seenBaseMethods.Add(overridden.OriginalDefinition);
                    }

                    if (!seenBaseMethods.Contains(method.OriginalDefinition))
                    {
                        yield return method;
                    }
                }
            }

            current = current.BaseType;
        }
    }

}

class FullyQualifiedNameRewriter(SemanticModel semanticModel) : CSharpSyntaxRewriter
{
    private readonly SemanticModel _semanticModel = semanticModel;

    public override SyntaxNode? VisitQualifiedName(QualifiedNameSyntax node)
    {
        return GetFullyQualified(node) ?? base.VisitQualifiedName(node);
    }

    public override SyntaxNode? VisitIdentifierName(IdentifierNameSyntax node)
    {
        if (node.Parent is QualifiedNameSyntax)
        {
            return base.VisitIdentifierName(node);
        }

        return GetFullyQualified(node) ?? base.VisitIdentifierName(node);
    }

    QualifiedNameSyntax? GetFullyQualified(SyntaxNode node)
    {
        var symbol = _semanticModel.GetSymbolInfo(node).Symbol;

        if (symbol is INamedTypeSymbol typeSymbol)
        {
            var fullyQualifiedName = (QualifiedNameSyntax)ParseName(typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
            return fullyQualifiedName.WithTriviaFrom(node);
        }
        return default;
    }
}

class StatelessVisitorRewriter(SemanticModel semanticModel) : FullyQualifiedNameRewriter(semanticModel)
{
    private readonly SemanticModel _semanticModel = semanticModel;
    public override  SyntaxNode? VisitIdentifierName(IdentifierNameSyntax node)
    {
        var symbol = _semanticModel.GetSymbolInfo(node).Symbol;
        if (symbol == null)
            return base.VisitIdentifierName(node);

        if (symbol is ITypeParameterSymbol typeParameter)
        {
            if (typeParameter.Name == "T")
            {
                return node.WithIdentifier(Identifier("J"));
            }
        }

        return base.VisitIdentifierName(node);
    }


    public override SyntaxNode? VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        node = node.Update(
            VisitList(node.AttributeLists), 
            VisitList(node.Modifiers), 
            (TypeSyntax?)Visit(node.ReturnType) ?? throw new ArgumentNullException("returnType"), 
            (ExplicitInterfaceSpecifierSyntax?)Visit(node.ExplicitInterfaceSpecifier), 
            VisitToken(node.Identifier), 
            (TypeParameterListSyntax?)Visit(node.TypeParameterList), 
            (ParameterListSyntax?)Visit(node.ParameterList) ?? throw new ArgumentNullException("parameterList"), 
            VisitList(node.ConstraintClauses),
            null, 
            null, 
            VisitToken(node.SemicolonToken));
        // rename first parameter to "node" and drop last parameter
        var parameters = node.ParameterList.Parameters;
        var firstParam = parameters[0].WithIdentifier(Identifier("node"));
        var newParameters = ParameterList(SeparatedList(parameters.Skip(1).Take(parameters.Count - 2).Prepend(firstParam)));
        

        var invocationArgs = newParameters.Parameters
            .Select(p => p.Identifier.Text)
            .Append("_stub")
            .Select(x => Argument(IdentifierName(x)))
            .ToList();
        var invocation = InvocationExpression(
            MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                IdentifierName("_visitor"),
                IdentifierName(node.Identifier.Text)
            ),
            ArgumentList(SeparatedList(invocationArgs))
        );
        var arrowExpression = ArrowExpressionClause(invocation);
        node = node
            .WithParameterList(newParameters)
            .WithoutTrivia()
            .WithAttributeLists([])
            .WithBody(null)
            .WithExpressionBody(arrowExpression)
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
            .NormalizeWhitespace();

        return node;
    }
}