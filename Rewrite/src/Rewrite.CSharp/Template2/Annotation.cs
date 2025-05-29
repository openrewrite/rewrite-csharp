using Microsoft.CodeAnalysis;

namespace Rewrite.RewriteCSharp.Template2;

public struct Annotation
{

    public static SyntaxAnnotation Placeholder { get;} = new SyntaxAnnotation("TypeHint");
}