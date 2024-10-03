using System.Reflection;
using Castle.DynamicProxy;

namespace Rewrite.Core;


/// <summary>
/// Allows a base visitors to act as language specific visitors.
/// For example a visitor that is based on TreeVisitor will be dynamically converted to CSharpVisitor
/// to correctly handle processing of nodes it was never designed to handle. This allows reusability of
/// "common" language tree visitors while still visiting all necessary language specific nodes
/// </summary>
internal static class TreeVisitorAdapter
{
    public static TTargetVisitor Adapt<TTargetVisitor, TNodeType, TState>(ITreeVisitor<Tree, TState> @delegate)
        where TTargetVisitor : class, ITreeVisitor<TNodeType, TState>
        where TNodeType : class, Tree
    {
        var generator = new ProxyGenerator();

        // var options = new ProxyGenerationOptions(new ProxyGenerationHook()) { Selector = new CustomSelector() };

        IInterceptor myInterceptor = new DelegatingInterceptor<TTargetVisitor>(@delegate);

        // var proxy = generator.CreateClassProxy<V>(options, myInterceptor);
        var proxy = generator.CreateClassProxy<TTargetVisitor>(myInterceptor);
        return proxy;
    }
}

internal class DelegatingInterceptor<TVisitor>(object target) : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        if (invocation.Method.DeclaringType == typeof(TVisitor))
            invocation.Proceed();
        else
            invocation.ReturnValue = invocation.Method.Invoke(target, invocation.Arguments);
    }
}
