using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyInjection;
using Rewrite.Core;

namespace Rewrite.MSBuild;

public class RecipeFactory
{
    private readonly IServiceProvider _serviceProvider;

    public RecipeFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Recipe Create(Type type)
    {
        return (Recipe)ActivatorUtilities.CreateInstance(_serviceProvider, type);
    }
    // public Recipe Create(string typeName, AssemblyLoadContext assemblyLoadContext)
    // {
    //     var typeNameParts = TypeName.Parse(typeName);
    //     if (typeNameParts.AssemblyName == null)
    //     {
    //         throw new ArgumentException($"{typeName} must be fully qualified with assembly name.");
    //     }
    //
    //     var assembly = assemblyLoadContext.LoadFromAssemblyName(typeNameParts.AssemblyName.ToAssemblyName());
    //     var type = assembly.GetType(typeNameParts.FullName);
    //     if (type == null)
    //     {
    //         throw new ArgumentException($"{typeName} not found");
    //     }
    //
    //     if (!type.IsAssignableTo(typeof(Recipe)))
    //     {
    //         throw new ArgumentException($"{type} does not inherit from {typeof(Recipe).FullName}");
    //     }
    //     var recipie = (Recipe)ActivatorUtilities.CreateInstance(_serviceProvider, type);
    //     return recipie;
    // }
}