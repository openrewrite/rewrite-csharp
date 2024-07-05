namespace Rewrite.RewriteCSharp.Tree;

public interface CsContainer
{
    public record Location(CsSpace.Location BeforeLocation, CsRightPadded.Location ElementLocation)
    {
        public static readonly Location ARRAY_RANK_SPECIFIER_SIZES = new(CsSpace.Location.ARRAY_RANK_SPECIFIER_SIZES, CsRightPadded.Location.ARRAY_RANK_SPECIFIER_SIZE);
        public static readonly Location PROPERTY_DECLARATION_ACCESSORS = new(CsSpace.Location.PROPERTY_DECLARATION_ACCESSORS, CsRightPadded.Location.PROPERTY_DECLARATION_ACCESSORS);
    }
}