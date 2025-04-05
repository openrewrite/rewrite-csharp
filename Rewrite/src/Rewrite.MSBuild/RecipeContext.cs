using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.Loader;

namespace Rewrite.MSBuild;

/// <summary>
/// Provides context for loading and executing recipe assembly and its dependencies in a way that is separate from primary the host app
/// </summary>
public class RecipeIsolationContext : AssemblyLoadContext
{
    protected override Assembly? Load(AssemblyName assemblyName)
    {
        return base.Load(assemblyName);
    }
}

public class RecipeContext
{

}