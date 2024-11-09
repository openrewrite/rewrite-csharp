
namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class NewClass
    {
        public JavaType? Type => Clazz?.Type;

        public NewClass WithType(JavaType? type)
        {
            if (type == Clazz?.Type) return this;
            return new NewClass(Id, prefix, markers, _enclosing, @new, clazz?.WithType(type), _arguments, body, constructorType);
        }

    }
}
