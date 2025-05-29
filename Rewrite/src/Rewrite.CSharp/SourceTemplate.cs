namespace Rewrite.RewriteCSharp;

/// <summary>
/// C# equivalent of the Java SourceTemplate interface.
/// </summary>
/// <typeparam name="T">A type that extends Tree</typeparam>
/// <typeparam name="C">A type that extends Coordinates</typeparam>
public interface SourceTemplate<T, C> 
    where T : Core.Tree 
    where C : Coordinates 
{
    /// <summary>
    /// Applies the template within the given scope at the specified coordinates.
    /// </summary>
    /// <typeparam name="T2">A type that extends T</typeparam>
    /// <param name="scope">The cursor representing the scope</param>
    /// <param name="coordinates">The coordinates where the template will be applied</param>
    /// <param name="parameters">Optional parameters for the template application</param>
    /// <returns>The resulting tree after applying the template</returns>
    T2 Apply<T2>(Cursor scope, C coordinates, params object[] parameters) where T2 : T;
}