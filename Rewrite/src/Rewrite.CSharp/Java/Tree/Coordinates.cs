// using System.Collections;
//
// namespace Rewrite.RewriteCSharp.Tree;
//
// public class Coordinates
// {
//     private J tree;
//     private Space.Location? spaceLocation;
//     private Mode mode;
//
//     private IComparer comparator;
//
//     public Coordinates(J tree, Space.Location? spaceLocation, Mode mode, IComparer? comparator = null)
//     {
//
//         this.tree = tree;
//         this.spaceLocation = spaceLocation;
//         this.mode = mode;
//         this.comparator = comparator ?? Comparer<J>.Default;
//     }
//
//     public bool IsReplacement()
//     {
//         return Mode.REPLACEMENT == mode;
//     }
//
//     /// <summary>
//     /// Determines whether we are replacing a whole tree element, and not either
//     /// (1) replacing just a piece of a method, class, or variable declaration signature or
//     /// (2) inserting a new element
//     /// </summary>
//     public bool IsReplaceWholeCursorValue()
//     {
//         return IsReplacement() && spaceLocation == null;
//     }
//
//     public enum Mode
//     {
//         AFTER,
//         BEFORE,
//         REPLACEMENT
//     }
//
//     /// <summary>
//     /// Gets the comparator.
//     /// </summary>
//     public IComparer<J2> GetComparator<J2>() where J2 : J
//     {
//         //noinspection unchecked
//         return (IComparer<J2>)comparator;
//     }
// }
