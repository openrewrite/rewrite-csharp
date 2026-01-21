using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using Nuke.Common.IO;

namespace Rewrite.MSBuild;

public class RecipeAssemblyResolver : MetadataAssemblyResolver
{
    private PathAssemblyResolver _resolver;
    private Dictionary<string, Assembly> _loaded = new();
    public RecipeAssemblyResolver(IEnumerable<AbsolutePath> additionalAssemblies)
    {
        
        var runtimeAssemblies = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");
        var currentDirAssemblies = Directory.EnumerateFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "*.dll");
        var allAssemblies = runtimeAssemblies.Union(currentDirAssemblies).Union(additionalAssemblies.Select(x => (string)x));
        
        _resolver = new PathAssemblyResolver(allAssemblies.Distinct());
    }

    public override Assembly? Resolve(MetadataLoadContext context, AssemblyName assemblyName)
    {
        if (_loaded.TryGetValue(assemblyName.FullName, out var result))
            return result;
        //
        // var result = context.GetAssemblies().FirstOrDefault(x => x.GetName().FullName == assemblyName.FullName);
        // if (result != null)
        //     return result;
        result = AssemblyResolution.Resolve(assemblyName);
        if (result != null)
        {
            result = context.LoadFromAssemblyPath(result.Location);
        }
        else
        {
            result = _resolver.Resolve(context, assemblyName);
        }

        _loaded.Add(assemblyName.FullName, result!);
        return result;
    }
}