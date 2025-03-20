
namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class NewClass
    {
        public JavaType? Type => Clazz?.Type;

        public NewClass WithType(JavaType? type)
        {
            if (type == Clazz?.Type) return this;
            return new NewClass(Id, Prefix, Markers, _enclosing, New, Clazz?.WithType(type), _arguments, Body, ConstructorType);
        }

    }
}
