namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class Primitive
    {
        JavaType? TypedTree.Type => Type;

        public Primitive WithType(JavaType? newType)
        {
            if (newType == Type)
            {
                return this;
            }

            if (newType is not JavaType.Primitive primitiveType)
            {
                throw new ArgumentException("Cannot apply a non-primitive type to Primitive");
            }

            return new J.Primitive(Id, Prefix, Markers, primitiveType);
        }
    }
}
