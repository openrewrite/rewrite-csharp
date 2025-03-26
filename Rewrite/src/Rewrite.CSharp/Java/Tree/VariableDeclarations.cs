namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class VariableDeclarations
    {
        public JavaType? Type => TypeExpression?.Type;

        public VariableDeclarations WithType(JavaType? newType) => TypeExpression == null ? this : WithTypeExpression(TypeExpression.WithType(newType));

        partial class NamedVariable
        {
            public JavaType? Type => VariableType?.Type;

            public NamedVariable WithType(JavaType? newType) => VariableType == null ? this : WithVariableType(VariableType.WithType(newType));
        }
    }
}
