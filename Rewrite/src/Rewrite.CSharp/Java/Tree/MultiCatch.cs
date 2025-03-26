
namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class MultiCatch
    {
        public JavaType Type => new JavaType.MultiCatch(Alternatives
            .Select(alt => alt.Type)
            .Where(x => x != null)
            .Cast<JavaType>()
            .ToList());

        public MultiCatch WithType(JavaType? type) => this;

    }
}
