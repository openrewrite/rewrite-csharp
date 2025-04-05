using System.Reflection;
using System.Text.RegularExpressions;
using Rewrite.Core;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava.Tree;
using Rewrite.RewriteJson;
using Rewrite.RewriteJson.Tree;
using Rewrite.RewriteProperties.Tree;
using Rewrite.RewriteXml.Tree;
using Rewrite.RewriteYaml.Tree;
using Serilog;

namespace Rewrite.MSBuild;

public static class AssemblyResolution
{
    public static Assembly[] CoreRewriteAssemblies => new[]
        {
            typeof(Tree),
            typeof(Cs),
            typeof(Json),
            typeof(Properties),
            typeof(Xml),
            typeof(Yaml)
        }
        .Select(x => x.Assembly)
        .ToArray();

    static readonly ILogger _log = Log.ForContext(typeof(AssemblyResolution));

    public static void LoadOpenRewrite()
    {
        // this should cause assemblies to be loaded on startup preventing any plugins from replacing them with "old crap"
        
        AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        
    }

    private static Assembly? OnAssemblyResolve(object? sender, ResolveEventArgs args)
    {
        var assemblyName = AssemblyName.GetAssemblyName(args.Name);
        return Resolve(assemblyName);
    }

    public static Assembly? Resolve(AssemblyName requestedAssemblyName)
    {
        var resolvedAssembly = CoreRewriteAssemblies.FirstOrDefault(x => x.GetName().Name == requestedAssemblyName.Name);
        if (resolvedAssembly != null && resolvedAssembly.GetName().Version != requestedAssemblyName.Version)
        {
            _log.Debug("Substituted assembly Assembly {RequestedAssembly} with {RedirectedAssembly}",requestedAssemblyName, resolvedAssembly.FullName);

        }
        return resolvedAssembly;
    }
}