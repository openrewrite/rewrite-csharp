namespace Rewrite.RewriteJava.Tree;

public partial interface J : Rewrite.Core.Tree
{
    public partial class MethodDeclaration
    {

        public Identifier Name => _name.Identifier;
        public MethodDeclaration WithName(Identifier identifier)
        {
            return new MethodDeclaration(
                Id,
                prefix,
                markers,
                leadingAnnotations,
                modifiers,
                _typeParameters,
                returnTypeExpression,
                _name.WithIdentifier(identifier),
                _parameters,
                _throws,
                body,
                _defaultValue,
                methodType);
        }

        public JavaType? Type => null;
        public MethodDeclaration WithType(JavaType? type) => this;
    }
}
