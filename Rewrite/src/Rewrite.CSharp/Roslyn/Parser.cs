// using Microsoft.CodeAnalysis;
// using Microsoft.CodeAnalysis.CSharp;
// using Microsoft.CodeAnalysis.CSharp.Syntax;
// using Rewrite.RewriteCSharp.Tree;
//
// namespace Rewrite.RewriteCSharp.Roslyn;
//
// public class Parser
// {
//     public SyntaxNode Parse(Core.Tree tree)
//     {
//         Core.Tree.ToString(tree)
//         var code = tree.ToString() ?? throw new InvalidOperationException("Tree ToString() returned null. Is the local printer configured?");
//
//         var syntaxNode = tree switch
//         {
//             Cs.CompilationUnit => SyntaxFactory.ParseCompilationUnit(code),
//             Cs.ClassDeclaration => SyntaxFactory.ParseMemberDeclaration(code),
//         }
//     }
// }
