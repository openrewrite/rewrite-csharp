// using Microsoft.CodeAnalysis;
// using Microsoft.CodeAnalysis.CSharp;
// using Microsoft.CodeAnalysis.CSharp.Syntax;
// using Microsoft.CodeAnalysis.Text;
// using System.Collections.Immutable;
// using System.Text;
//
// [Generator]
// public class AsyncAcceptMethodGenerator : IIncrementalGenerator
// {
//     public void Initialize(IncrementalGeneratorInitializationContext context)
//     {
//         var classDeclarations = context.SyntaxProvider
//             .CreateSyntaxProvider(
//                 predicate: static (node, _) => node is ClassDeclarationSyntax,
//                 transform: static (ctx, _) => (ClassDeclarationSyntax)ctx.Node)
//             .Where(static c => c is not null);
//
//         var compilationAndClasses = context.CompilationProvider.Combine(classDeclarations.Collect());
//
//         context.RegisterSourceOutput(compilationAndClasses, (spc, source) =>
//         {
//             var (compilation, classNodes) = source;
//             foreach (var classNode in classNodes)
//             {
//                 var model = compilation.GetSemanticModel(classNode.SyntaxTree);
//                 var symbol = model.GetDeclaredSymbol(classNode);
//                 if (symbol == null || symbol.IsAbstract)
//                     continue;
//
//                 var acceptMethod = classNode.Members
//                     .OfType<MethodDeclarationSyntax>()
//                     .FirstOrDefault(m =>
//                         m.Identifier.Text == "AcceptCSharp"
//                         && m.ParameterList.Parameters.Count == 2
//                         && m.Body != null
//                         && m.Body.Statements.Count == 1
//                         && m.Body.Statements[0] is ReturnStatementSyntax ret
//                         && ret.Expression is InvocationExpressionSyntax invoc
//                         && invoc.Expression is MemberAccessExpressionSyntax member
//                         && member.Expression is IdentifierNameSyntax visitorVar
//                         && visitorVar.Identifier.Text == m.ParameterList.Parameters[0].Identifier.Text);
//
//                 if (acceptMethod == null)
//                     continue;
//
//                 var invocation = (InvocationExpressionSyntax)((ReturnStatementSyntax)acceptMethod.Body!.Statements[0]).Expression!;
//                 var memberAccess = (MemberAccessExpressionSyntax)invocation.Expression;
//                 var visitMethodName = memberAccess.Name.Identifier.Text;
//
//                 var sb = new StringBuilder();
//                 sb.AppendLine("#nullable enable");
//
//                 // Copy all using directives from the original syntax tree
//                 var root = classNode.SyntaxTree.GetRoot();
//                 var usings = root.DescendantNodes().OfType<UsingDirectiveSyntax>();
//                 foreach (var usingDirective in usings)
//                 {
//                     sb.AppendLine(usingDirective.ToFullString().Trim());
//                 }
//
//                 sb.AppendLine("using System.Threading.Tasks;");
//
//                 // Emit only namespace declarations (file-scoped or block)
//                 var fileScopedNs = root.DescendantNodesAndSelf().OfType<FileScopedNamespaceDeclarationSyntax>().FirstOrDefault();
//                 
//                 sb.AppendLine();
//                 sb.AppendLine($"namespace {fileScopedNs.Name.ToFullString().Trim()};");
//                 
//
//                 // Reconstruct nested type containers
//                 var containers = new Stack<INamedTypeSymbol>();
//                 var container = symbol.ContainingType;
//                 while (container != null)
//                 {
//                     containers.Push(container);
//                     container = container.ContainingType;
//                 }
//
//                 var closingBraces = containers.Count;
//                 while (containers.Count > 0)
//                 {
//                     var c = containers.Pop();
//                     var keyword = c.TypeKind switch
//                     {
//                         TypeKind.Class => "class",
//                         TypeKind.Struct => "struct",
//                         TypeKind.Interface => "interface",
//                         TypeKind.Enum => "enum",
//                         TypeKind.Delegate => "delegate",
//                         // TypeKind.Record => c.IsRecord ? "record" : "class",
//                         _ => "class"
//                     };
//                     sb.AppendLine($"partial {keyword} {c.Name} {{");
//                 }
//
//                 sb.AppendLine($"    public partial class {symbol.Name}");
//                 sb.AppendLine("    {");
//                 sb.AppendLine("        public async Task<J?> AcceptCSharp<P>(CSharpVisitorAsync<P> v, P p)");
//                 sb.AppendLine("        {");
//                 sb.AppendLine($"            return await v.{visitMethodName}(this, p);");
//                 sb.AppendLine("        }");
//                 sb.AppendLine("    }");
//
//                 for (int i = 0; i < closingBraces; i++)
//                     sb.AppendLine("}");
//
//                
//                 var sourceCode = sb.ToString();
//                 spc.AddSource(symbol.Name + "_AcceptCSharpAsync.g.cs", SourceText.From(sourceCode, Encoding.UTF8));
//             }
//         });
//     }
// }
