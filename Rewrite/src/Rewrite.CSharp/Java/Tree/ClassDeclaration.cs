namespace Rewrite.RewriteJava.Tree;

public partial interface J : Rewrite.Core.Tree
{
    public partial class ClassDeclaration
    {
        public Kind.Types DeclarationKind => _declarationKind.KindType;
        JavaType? TypedTree.Type => Type;

        public ClassDeclaration WithType(JavaType? newType)
        {
            return WithType(newType as JavaType.FullyQualified);
        }
    }
}
