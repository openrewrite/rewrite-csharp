using Rewrite.Core;
using Rewrite.RewriteCSharp.Tree;

namespace Rewrite.Test;

public static class SourceSpecExtensions
{
    public static T Parse<T>(this SourceSpec sourceSpec) where T : SourceFile
    {
        var sourceText = sourceSpec.Before ?? "";
        return (T)sourceSpec.Parser.Build().Parse(sourceText);

    }

    public static IEnumerable<T> Parse<T>(this SourceSpecs sourceSpec) where T : SourceFile
    {
        foreach (var spec in sourceSpec)
        {
            yield return spec.Parse<T>();
        }
    }
}
