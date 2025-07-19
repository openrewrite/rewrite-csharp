using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using NMica.Utils.IO;

namespace Rewrite.MSBuild;

public class RecipeAssemblyResolver : MetadataAssemblyResolver
{
    private PathAssemblyResolver _resolver;
    public RecipeAssemblyResolver(IEnumerable<AbsolutePath> additionalAssemblies)
    {
        
        var runtimeAssemblies = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");
        var currentDirAssemblies = Directory.EnumerateFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "*.dll");
        var allAssemblies = runtimeAssemblies.Union(currentDirAssemblies).Union(additionalAssemblies.Select(x => (string)x));
        
        _resolver = new PathAssemblyResolver(allAssemblies);
    }

    public override Assembly? Resolve(MetadataLoadContext context, AssemblyName assemblyName)
    {
        var result = AssemblyResolution.Resolve(assemblyName);
        if (result != null)
        {
            result = context.LoadFromAssemblyPath(result.Location);
        }
        else
        {
            result = _resolver.Resolve(context, assemblyName);
        }

        return result;
    }
}