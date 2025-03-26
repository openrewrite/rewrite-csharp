using Rewrite.Test.CSharp;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class EventDeclarationSyntaxTests : RewriteTest
{
    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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
