using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class DelegateDeclarationTests : RewriteTest
{
    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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
