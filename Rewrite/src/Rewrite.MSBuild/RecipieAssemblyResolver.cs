using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

namespace Rewrite.MSBuild;

public class RecipieAssemblyResolver : MetadataAssemblyResolver
{
    private PathAssemblyResolver _resolver;
    public RecipieAssemblyResolver()
    {
        
        var runtimeAssemblies = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");
        var currentDirAssemblies = Directory.EnumerateFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "*.dll");
        var allAssemblies = runtimeAssemblies.Union(currentDirAssemblies);
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