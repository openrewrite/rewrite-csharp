using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.CSharp;

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

public static class JExtensions
{
    public static SyntaxType GetSyntaxType(this J node)
    {
        /*
         * InvocationExpressionSyntax -> J.MethodInvocation
         *                               Cs.AliasQualifiedName
         * 
         */
        throw new NotImplementedException();
    }
    
    static SyntaxType Qualify(Type type)
    {
        if(!type.IsAssignableTo(typeof(SyntaxNode)))
            throw new InvalidOperationException($"Type is not a {nameof(SyntaxNode)}");
        SyntaxType result = SyntaxType.Node;
        if(type.IsAssignableTo(typeof(StatementSyntax)))
            result |= SyntaxType.Statement;
        if (type.IsAssignableTo(typeof(ExpressionSyntax)))
            result |= SyntaxType.Expression;
        if (type.IsAssignableTo(typeof(BaseListSyntax)))
            result |= SyntaxType.Statement;
        if (type.IsAssignableTo(typeof(NameSyntax)))
            result |= SyntaxType.Name;
        if (type.IsAssignableTo(typeof(MemberDeclarationSyntax)))
            result |= SyntaxType.MemberDeclaration;
        if (type.IsAssignableTo(typeof(PatternSyntax)))
            result |= SyntaxType.Pattern;
        if (type.IsAssignableTo(typeof(StructuredTriviaSyntax)))
            result |= SyntaxType.Trivia;
        if (type.IsAssignableTo(typeof(DirectiveTriviaSyntax)))
            result |= SyntaxType.Directive;
        return result;
    }
}