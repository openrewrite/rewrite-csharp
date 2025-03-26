namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class MethodInvocation
    {
        public JavaType? Type => MethodType?.ReturnType;

        public MethodInvocation WithType(JavaType? type)
        {
            if (type == MethodType?.ReturnType || type is not JavaType.Method method) return this;
            return new J.MethodInvocation(Id, Prefix, Markers, Padding.Select, Padding.TypeParameters, Name, Padding.Arguments, method);
        }

    }
}
