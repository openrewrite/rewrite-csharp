using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp;

[Flags]
public enum SyntaxType
{
    None = 0,
    Node = 1 << 0,
    Statement = 1 << 1,
    Expression = 1 << 2,
    MemberDeclaration = 1 << 3,
    Name = 1 << 4,
    List = 1 << 5,
    Pattern = 1 << 6,
    Trivia = 1 << 7,
    Directive = 1 << 8,
}

public static class SyntaxTypeExtensions
{
    public static SyntaxType GetSyntaxType(this J node)
    {
        return node switch
        {
            Cs.Binary => SyntaxType.Node | SyntaxType.Expression,
            J.Binary => SyntaxType.Node | SyntaxType.Expression,
            Cs.FileScopeNamespaceDeclaration => SyntaxType.Node | SyntaxType.MemberDeclaration,
            Cs.BlockScopeNamespaceDeclaration => SyntaxType.Node | SyntaxType.MemberDeclaration,
            Cs.ClassDeclaration => SyntaxType.Node | SyntaxType.MemberDeclaration,
            Cs.EnumDeclaration => SyntaxType.Node | SyntaxType.MemberDeclaration,
            J.Identifier => SyntaxType.Node | SyntaxType.Expression | SyntaxType.Name,
            Cs.MethodDeclaration => SyntaxType.Node | SyntaxType.MemberDeclaration,
            J.NullableType => SyntaxType.Node | SyntaxType.Expression,
            J.Block => SyntaxType.Node | SyntaxType.Statement,
            Cs.ArrayType => SyntaxType.Node | SyntaxType.Expression,
            Cs.AwaitExpression => SyntaxType.Node | SyntaxType.Expression,
            Cs.BinaryPattern => SyntaxType.Node | SyntaxType.Pattern,
            J.Break => SyntaxType.Node | SyntaxType.Statement,
            J.TypeCast => SyntaxType.Node | SyntaxType.Expression,
            Cs.CheckedExpression => SyntaxType.Node | SyntaxType.Expression,
            J.ParameterizedType => SyntaxType.Node | SyntaxType.Expression | SyntaxType.Name,
            Cs.AliasQualifiedName => SyntaxType.Node | SyntaxType.Expression | SyntaxType.Name,
            Cs.PointerType => SyntaxType.Node | SyntaxType.Expression,
            Cs.TupleType => SyntaxType.Node | SyntaxType.Expression,
            Cs.RefType => SyntaxType.Node | SyntaxType.Expression,
            Cs.TupleExpression => SyntaxType.Node | SyntaxType.Expression,
            Cs.RangeExpression => SyntaxType.Node | SyntaxType.Expression,
            Cs.ImplicitElementAccess => SyntaxType.Node | SyntaxType.Expression,
            J.Ternary => SyntaxType.Node | SyntaxType.Expression,
            J.Literal => SyntaxType.Node | SyntaxType.Expression,
            Cs.DefaultExpression => SyntaxType.Node | SyntaxType.Expression,
            J.MethodInvocation => SyntaxType.Node | SyntaxType.Expression,
            J.ArrayAccess => SyntaxType.Node | SyntaxType.Expression,
            Cs.DeclarationExpression => SyntaxType.Node | SyntaxType.Expression,
            J.MethodDeclaration => SyntaxType.Node | SyntaxType.Expression,
            Cs.RefExpression => SyntaxType.Node | SyntaxType.Expression,
            Cs.Lambda => SyntaxType.Node | SyntaxType.Expression,
            Cs.InitializerExpression => SyntaxType.Node | SyntaxType.Expression,
            Cs.NewClass => SyntaxType.Node | SyntaxType.Expression,
            J.NewArray => SyntaxType.Node | SyntaxType.Expression,
            Cs.StackAllocExpression => SyntaxType.Node | SyntaxType.Expression,
            Cs.CollectionExpression => SyntaxType.Node | SyntaxType.Expression,
            Cs.QueryExpression => SyntaxType.Node | SyntaxType.Expression,
            J.Empty => SyntaxType.Node | SyntaxType.Expression,
            Cs.InterpolatedString => SyntaxType.Node | SyntaxType.Expression,
            Cs.IsPattern => SyntaxType.Node | SyntaxType.Expression,
            Cs.StatementExpression => SyntaxType.Node | SyntaxType.Expression,
            Cs.DiscardPattern => SyntaxType.Node | SyntaxType.Pattern,
            Cs.TypePattern => SyntaxType.Node | SyntaxType.Pattern,
            Cs.VarPattern => SyntaxType.Node | SyntaxType.Pattern,
            Cs.RecursivePattern => SyntaxType.Node | SyntaxType.Pattern,
            Cs.ConstantPattern => SyntaxType.Node | SyntaxType.Pattern,
            Cs.ParenthesizedPattern => SyntaxType.Node | SyntaxType.Pattern,
            Cs.RelationalPattern => SyntaxType.Node | SyntaxType.Pattern,
            Cs.UnaryPattern => SyntaxType.Node | SyntaxType.Pattern,
            Cs.ListPattern => SyntaxType.Node | SyntaxType.Pattern,
            Cs.SlicePattern => SyntaxType.Node | SyntaxType.Pattern,
            Cs.ExpressionStatement => SyntaxType.Node | SyntaxType.Statement,
            J.Label => SyntaxType.Node | SyntaxType.Statement,
            Cs.GotoStatement => SyntaxType.Node | SyntaxType.Statement,
            J.Continue => SyntaxType.Node | SyntaxType.Statement,
            J.Return => SyntaxType.Node | SyntaxType.Statement,
            J.Throw => SyntaxType.Node | SyntaxType.Statement,
            Cs.Yield => SyntaxType.Node | SyntaxType.Statement,
            J.WhileLoop => SyntaxType.Node | SyntaxType.Statement,
            J.DoWhileLoop => SyntaxType.Node | SyntaxType.Statement,
            J.ForLoop => SyntaxType.Node | SyntaxType.Statement,
            Cs.ForEachVariableLoop => SyntaxType.Node | SyntaxType.Statement,
            Cs.UsingStatement => SyntaxType.Node | SyntaxType.Statement,
            Cs.FixedStatement => SyntaxType.Node | SyntaxType.Statement,
            Cs.CheckedStatement => SyntaxType.Node | SyntaxType.Statement,
            Cs.UnsafeStatement => SyntaxType.Node | SyntaxType.Statement,
            Cs.LockStatement => SyntaxType.Node | SyntaxType.Statement,
            J.If => SyntaxType.Node | SyntaxType.Statement,
            Cs.SwitchExpression => SyntaxType.Node | SyntaxType.Expression,
            Cs.Try => SyntaxType.Node | SyntaxType.Statement,
            Cs.DelegateDeclaration => SyntaxType.Node | SyntaxType.MemberDeclaration,
            Cs.EnumMemberDeclaration => SyntaxType.Node | SyntaxType.MemberDeclaration,
            Cs.EventDeclaration => SyntaxType.Node | SyntaxType.MemberDeclaration,
            Cs.OperatorDeclaration => SyntaxType.Node | SyntaxType.MemberDeclaration,
            Cs.PropertyDeclaration => SyntaxType.Node | SyntaxType.MemberDeclaration,
            _ => SyntaxType.Node
        };
    }
    
    public static bool IsStatement(this J j)
    {
        return j.GetSyntaxType().HasFlag(SyntaxType.Statement);
    }
    public static bool IsExpression(this J j)
    {
        return j.GetSyntaxType().HasFlag(SyntaxType.Expression);
    }
}