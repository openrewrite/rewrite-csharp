using FluentAssertions;
using Rewrite.Test.CSharp;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;
using static Rewrite.RewriteJava.Tree.JavaType;

namespace Rewrite.CSharp.Tests;

using static Assertions;

public class CSharpTypeAttributionTests : RewriteTest
{
    [Test]
    [KnownBug]
    // ReSharper disable once UnusedMember.Local
    public void ClosureImplicitParameterAttributed()
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
                    var c = (Cs.ClassDeclaration)cu.Members[0];
                    var md = (Cs.MethodDeclaration)c.Body!.Statements[1];
                    var m = md.Descendents().OfType<J.MethodInvocation>().First();
                    // m.Arguments.Should().
                    // Assert.Equal((decimal)3, m.Arguments.Count, 1);
                    var thirdArgument = (Cs.Argument)m.Arguments[2];
                    thirdArgument.Should().BeOfType<Cs.Argument>();

                    var it = thirdArgument.Expression!;
                    //todo: fix this to be properly type attested with c#
                //     AsFullyQualified(it.Select?.Type)?.FullyQualifiedName.Should().BeEquivalentTo("java.lang.String");
                //     it.MethodType?.Name.Should().BeEquivalentTo("substring");
                //     it.MethodType?.DeclaringType.FullyQualifiedName.Should().BeEquivalentTo("java.lang.String");
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
            AsFullyQualified(variable.VariableType?.Type)?.FullyQualifiedName.Should().Be(attributed ? "java.util.ArrayList" : "java.lang.Object");
            return variable;
        }
    }
}
