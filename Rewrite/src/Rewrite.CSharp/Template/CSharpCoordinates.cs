using System.Collections;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp;

public class CSharpCoordinates : Coordinates
{
    public J Tree { get; }
    public Space.Location? Location { get; }
    public Mode TargetingMode { get; }
    public IComparer? Comparer { get; }

    public CSharpCoordinates(J tree, Space.Location? spaceLocation, Mode targetingMode, IComparer? comparer = null)
    {
        Tree = tree;
        Location = spaceLocation;
        TargetingMode = targetingMode;
        Comparer = comparer;
    }

    public bool IsReplacement()
    {
        return Mode.REPLACEMENT == TargetingMode;
    }

    /// <summary>
    /// Determines whether we are replacing a whole tree element, and not either
    /// (1) replacing just a piece of a method, class, or variable declaration signature or
    /// (2) inserting a new element
    /// </summary>
    public bool IsReplaceWholeCursorValue()
    {
        return IsReplacement() && Location == null;
    }

    public enum Mode
    {
        AFTER,
        BEFORE,
        REPLACEMENT
    }

}