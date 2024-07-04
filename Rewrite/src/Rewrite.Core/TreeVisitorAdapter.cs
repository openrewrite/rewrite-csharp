using System.Reflection;
using Castle.DynamicProxy;

namespace Rewrite.Core;

internal static class TreeVisitorAdapter
{
    public static V Adapt<V, R, P>(ITreeVisitor<Tree, P> @delegate) where V : class, ITreeVisitor<R, P> where R : class, Tree
    {
        var generator = new ProxyGenerator();

        // var options = new ProxyGenerationOptions(new ProxyGenerationHook()) { Selector = new CustomSelector() };

        IInterceptor myInterceptor = new DelegatingInterceptor<V>(@delegate);

        // var proxy = generator.CreateClassProxy<V>(options, myInterceptor);
        var proxy = generator.CreateClassProxy<V>(myInterceptor);
        return proxy;
    }
}

internal class DelegatingInterceptor<V>(object target) : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        if (invocation.Method.DeclaringType == typeof(V))
            invocation.Proceed();
        else
            invocation.ReturnValue = invocation.Method.Invoke(target, invocation.Arguments);
    }
}