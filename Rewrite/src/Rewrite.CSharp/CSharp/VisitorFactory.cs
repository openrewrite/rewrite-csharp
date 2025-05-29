// using Rewrite.RewriteCSharp;
// using Rewrite.RewriteCSharp.Tree;
//
// namespace Rewrite.RewriteCSharp.Tree;
//
// public class AnonymousVisitor
// {
//     private CSharpVisitor _visitor;
//     public delegate J? VisitDelegate<in TArg>(TArg node, AnonymousVisitor @this, CSharpVisitor @base);
//     public AnonymousVisitor()
//     {
//         _visitor = new CSharpVisitor();
//         // VisitArgument = (argument, @base, @this) => _visitor.VisitArgument(argument);
//     }
//
//     
//     public VisitDelegate<Cs.Argument>? VisitArgument { get; init; }
//     
//     public void Visit(J j)
//     {
//         new AnonymousVisitor()
//         {
//             VisitArgument = (node, @this, @base) => 
//         };
//         // _visitor.Visit(j);
//     }
//
//     private class CSharpVisitorImpl(AnonymousVisitor parent) : CSharpVisitor
//     {
//         private readonly AnonymousVisitor _parent = parent;
//
//         public override J? VisitArgument(Cs.Argument node)
//         {
//             return _parent.VisitArgument != null ? _parent.VisitArgument(node, base) : base.VisitArgument(node);
//         }
//     }
// }
//
// public partial class CSharpVisitor
// {
//     public AnonymousVisitor Create()
//     {
//         return new AnonymousVisitor
//         {
//             VisitArgument = argument => argument
//         };
//     }
// }
//
//
