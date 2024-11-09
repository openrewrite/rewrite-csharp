using FluentAssertions;
using Rewrite.Test.Engine.Remote;
using Rewrite.Test.CSharp;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;
using static Rewrite.RewriteJava.Tree.JavaType;

namespace Rewrite.CSharp.Tests;

using static Assertions;

public class CSharpTypeAttributionTests(ITestOutputHelper output) : RewriteTest(output)
{
    [Fact]
    [KnownBug]
    void ClosureImplicitParameterAttributed()
    {
        RewriteRun(
            CSharp(
                """
                public class Program
                {
                    public T? register<T>(String name, Type type, Action<T> configurationAction)
                    {
                        return default;
                    }

                    public void Main()
                    {
                        register("test", typeof(Program), (string t) => { });
                    }
                }
                """,
                spec => spec.AfterRecipe = cu =>
                {
                    var c = (J.ClassDeclaration)cu.Members[0];
                    var md = (J.MethodDeclaration)c.Body.Statements[1];
                    var m = (J.MethodInvocation)md.Body!.Statements[0];
                    // m.Arguments.Should().
                    Assert.Equal((decimal)3, m.Arguments.Count, 1);
                    m.Arguments[2].Should().BeOfType<J.Lambda>();
                    var it =
                        (J.MethodInvocation)((J.Return)((J.Block)((J.Lambda)m.Arguments[2]).Body).Statements[0])
                        .Expression!;
                    AsFullyQualified(it.Select?.Type)?.FullyQualifiedName.Should().BeEquivalentTo("java.lang.String");
                    it.MethodType?.Name.Should().BeEquivalentTo("substring");
                    it.MethodType?.DeclaringType.FullyQualifiedName.Should().BeEquivalentTo("java.lang.String");
                })
        );
    }

    private Action<SourceSpec<Cs.CompilationUnit>> IsAttributed(bool attributed)
    {
        var testJavaVisitor = new TestJavaVisitor(attributed);
        return spec => spec.AfterRecipe = cu => testJavaVisitor.Visit(cu, 0);
    }

    private static FullyQualified? AsFullyQualified(JavaType? type)
    {
        return type is FullyQualified fullyQualified and not Unknown ? fullyQualified : null;
    }

    class TestJavaVisitor(bool attributed) : JavaVisitor<int>
    {
        public override J? VisitVariable(J.VariableDeclarations.NamedVariable variable, int p)
        {
            Assert.Equal(AsFullyQualified(variable.VariableType?.Type)?.FullyQualifiedName,
                attributed ? "java.util.ArrayList" : "java.lang.Object");
            return variable;
        }
    }
}
