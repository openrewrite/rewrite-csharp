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
                Prefix,
                Markers,
                LeadingAnnotations,
                Modifiers,
                _typeParameters,
                ReturnTypeExpression,
                _name.WithIdentifier(identifier),
                _parameters,
                _throws,
                Body,
                _defaultValue,
                MethodType);
        }

        public JavaType? Type => null;
        public MethodDeclaration WithType(JavaType? type) => this;
    }
}
