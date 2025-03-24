using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using PeterO.Cbor;
using Rewrite.Core;

namespace Rewrite.Remote;

public interface IRemotingContext
{
    internal static readonly Dictionary<string, Func<CBORObject, Recipe>> RecipeFactories = new();
    internal static readonly ThreadLocal<IRemotingContext> RemotingThreadLocal = new();
    IRemotingClient? Client { get; }
    public static IRemotingContext? Current() => RemotingThreadLocal.Value;

    public void SetCurrent() => RemotingThreadLocal.Value = this;
    void Reset();

    public static void RegisterRecipeFactory(string recipeId, Func<CBORObject, Recipe> factory) =>
        RecipeFactories[recipeId] = factory;

    static IRemotingContext Create()
    {
        // TODO use `AssemblyDependencyResolver` to load assembly?
        var type = Assembly.Load("Rewrite.Remote").GetType("Rewrite.Remote.RemotingContext");
        return (IRemotingContext)Activator.CreateInstance(type!)!;
    }

    void Connect(Socket socket);
}