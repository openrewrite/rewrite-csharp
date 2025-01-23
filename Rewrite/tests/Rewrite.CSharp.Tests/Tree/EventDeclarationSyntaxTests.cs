using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class EventDeclarationSyntaxTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    public void BasicEvent()
    {
        RewriteRun(
            CSharp(
                """
                public class TestClass
                {
                    public event EventHandler OnTest;
                }
                """
            )
        );
    }

    [Fact]
    public void EventWithExplicitInterface()
    {
        RewriteRun(
            CSharp(
                """
                public class TestClass
                {
                    public event EventHandler MyInterface.OnTest;
                }
                """
            )
        );
    }

    [Fact]
    public void CustomDelegateEvent()
    {
        RewriteRun(
            CSharp(
                """
                public class TestClass
                {
                    public delegate void CustomEventHandler(object sender, string message);
                    public event CustomEventHandler OnCustomEvent;
                }
                """
            )
        );
    }

    [Fact]
    public void StaticEvent()
    {
        RewriteRun(
            CSharp(
                """
                public class TestClass
                {
                    public static event EventHandler OnStaticTest;
                }
                """
            )
        );
    }

    [Fact]
    public void EventWithAccessor()
    {
        RewriteRun(
            CSharp(
                """
                public class TestClass
                {
                    private EventHandler test;
                    public event EventHandler OnTest
                    {
                        add { test += value; }
                        remove { test -= value; }
                    }
                }
                """
            )
        );
    }

    [Fact]
    public void GenericEvent()
    {
        RewriteRun(
            CSharp(
                """
                public class TestClass
                {
                    public event EventHandler<string> OnGenericTest;
                }
                """
            )
        );
    }

    [Fact]
    public void PrivateProtectedEvent()
    {
        RewriteRun(
            CSharp(
                """
                public class TestClass
                {
                    private protected event EventHandler OnProtectedTest;
                }
                """
            )
        );
    }

    [Fact]
    public void AbstractEvent()
    {
        RewriteRun(
            CSharp(
                """
                public abstract class TestClass
                {
                    public abstract event EventHandler OnAbstractTest;
                }
                """
            )
        );
    }

    [Fact]
    public void VirtualEvent()
    {
        RewriteRun(
            CSharp(
                """
                public class TestClass
                {
                    public virtual event EventHandler OnVirtualTest;
                }
                """
            )
        );
    }

    [Fact]
    public void EventWithAttributes()
    {
        RewriteRun(
            CSharp(
                """
                public class TestClass
                {
                    [Obsolete]
                    public event EventHandler OnDeprecatedTest;
                }
                """
            )
        );
    }

    [Fact]
    public void EventWithDocumentation()
    {
        RewriteRun(
            CSharp(
                """
                public class TestClass
                {
                    /// <summary>
                    /// Test event with documentation
                    /// </summary>
                    public event EventHandler OnDocumentedTest;
                }
                """
            )
        );
    }
}
