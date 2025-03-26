using FluentAssertions;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.Recipes;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;
using Xunit;
using Xunit.Abstractions;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.Java.Tests;

public class FindClassTests(ITestOutputHelper output) : RewriteTest(output)
{

    [Fact]
    public void Test1()
    {
        var source = new J.CompilationUnit(
            Tree.RandomId(),
            Space.EMPTY,
            Markers.EMPTY, "Foo.java", new FileAttributes(), null, false, null, null, [],
            [
                new J.ClassDeclaration(
                    Tree.RandomId(),
                    Space.EMPTY,
                    Markers.EMPTY,
                    [],
                    [],
                    new J.ClassDeclaration.Kind(Tree.RandomId(), Space.EMPTY, Markers.EMPTY, [],
                        J.ClassDeclaration.Kind.Types.Class),
                    new J.Identifier(Tree.RandomId(), Space.EMPTY, Markers.EMPTY, [], "Foo", null, null),
                    null,
                    null,
                    null,
                    null,
                    null,
                    new J.Block(Tree.RandomId(), Space.EMPTY, Markers.EMPTY,
                        new JRightPadded<bool>(false, Space.EMPTY, Markers.EMPTY), [], Space.EMPTY),
                    null
                )
            ],
            Space.EMPTY
        );
        var after = (J.CompilationUnit)new FindClass().GetVisitor().Visit(source, new InMemoryExecutionContext())!;
        after.Should().NotBeSameAs(source);
        after.Classes[0].Markers.MarkerList.Should().Contain(e => e is SearchResult);
    }
}
