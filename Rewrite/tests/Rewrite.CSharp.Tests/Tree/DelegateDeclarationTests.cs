using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class DelegateDeclarationTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    public void SimpleDelegateDeclaration()
    {
        RewriteRun(
            CSharp(
                """
                public delegate void SimpleDelegate();
                """
            )
        );
    }

    [Fact]
    public void DelegateWithParameters()
    {
        RewriteRun(
            CSharp(
                """
                public delegate int CalculateDelegate(int x, int y);
                """
            )
        );
    }

    [Fact]
    public void GenericDelegate()
    {
        RewriteRun(
            CSharp(
                """
                public delegate T GenericDelegate<T>(T input);
                """
            )
        );
    }

    [Fact]
    public void DelegateWithMultipleTypeParameters()
    {
        RewriteRun(
            CSharp(
                """
                public delegate TResult TransformDelegate<TInput, TResult>(TInput input);
                """
            )
        );
    }

    [Fact]
    public void DelegateWithConstraints()
    {
        RewriteRun(
            CSharp(
                """
                public delegate T ConstrainedDelegate<T>() where T : class, new();
                """
            )
        );
    }

    [Fact]
    public void DelegateWithNullableParameter()
    {
        RewriteRun(
            CSharp(
                """
                public delegate void NullableDelegate(string? text);
                """
            )
        );
    }

    [Fact]
    public void DelegateWithRefParameter()
    {
        RewriteRun(
            CSharp(
                """
                public delegate void RefDelegate(ref int number);
                """
            )
        );
    }

    [Fact]
    public void DelegateWithOutParameter()
    {
        RewriteRun(
            CSharp(
                """
                public delegate bool TryParseDelegate(string input, out int result);
                """
            )
        );
    }

    [Fact]
    public void NestedDelegate()
    {
        RewriteRun(
            CSharp(
                """
                public class Container
                {
                    private delegate void NestedDelegate(int x);
                }
                """
            )
        );
    }
}
