namespace Rewrite.RewriteCSharp.Tree;

public interface CsLeftPadded
{
    public record Location(CsSpace.Location BeforeLocation)
    {
        public static readonly Location ASSIGNMENT_OPERATION_OPERATOR = new(CsSpace.Location.ASSIGNMENT_OPERATION_OPERATOR);
        public static readonly Location BINARY_OPERATOR = new(CsSpace.Location.BINARY_OPERATOR);
        public static readonly Location USING_DIRECTIVE_STATIC = new(CsSpace.Location.USING_DIRECTIVE_STATIC);
        public static readonly Location USING_DIRECTIVE_UNSAFE = new(CsSpace.Location.USING_DIRECTIVE_UNSAFE);
    }
}