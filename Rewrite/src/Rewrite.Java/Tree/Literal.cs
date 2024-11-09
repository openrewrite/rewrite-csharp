namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class Literal
    {
        JavaType? TypedTree.Type => Type;
        public Literal WithType(JavaType? javaType)
        {
            if (javaType == Type)
            {
                return this;
            }

            if (javaType is JavaType.Primitive primitive)
            {
                return new J.Literal(Id, Prefix, Markers, Value, ValueSource,
                    UnicodeEscapes, primitive);
            }

            return this;
        }
    }
}
