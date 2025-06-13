using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp.Format;

/// <summary>
/// Visitor that ensures space objects are unique by reference equality
/// </summary>
internal class UniqueSpaceVisitor : CSharpVisitor<int>
{
    /// <summary>Space objects seen so far, compared by reference equality</summary>
    private readonly HashSet<Space> _spaces = new(ReferenceEqualityComparer.Instance);


    /// <summary>Visit space with optional location</summary>
    /// <param name="space">Space to visit</param>
    /// <param name="loc">Optional location in source</param>
    /// <param name="p">Parameter passed through visitor</param>
    /// <returns>Unique space instance</returns>
    public override Space VisitSpace(Space space, Space.Location? loc, int p)
    {
        return VisitSpace(space, p);
    }

    /// <summary>
    /// Visit space and ensure it is unique. If an empty space is encountered, creates new space instance.
    /// If space was seen before, creates new instance with same content.
    /// </summary>
    /// <param name="space">Space to visit</param>
    /// <param name="p">Parameter passed through visitor</param>
    /// <returns>Unique space instance</returns>
    public Space VisitSpace(Space space, int p)
    {
        // Create new instance for empty space
        if (ReferenceEquals(space, Space.EMPTY))
        {
            space = new Space([], "");
        }

        // Create new instance if space was seen before
        if (_spaces.Contains(space))
        {
            space = new Space(space.Comments, space.Whitespace);
        }

        _spaces.Add(space);
        return space;
    }
}
