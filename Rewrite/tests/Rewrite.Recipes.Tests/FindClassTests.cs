using FluentAssertions;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.Recipes;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;
using Rewrite.Test.CSharp;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.Java.Tests;

public class FindClassTests : RewriteTest
{
    [Test]
    public void Test1()
    {
        var source = Assertions.CSharp("class Test{}").Parse();
       
        var after = (Cs.CompilationUnit)new FindClass().GetVisitor().Visit(source, new InMemoryExecutionContext())!;
        after.Should().NotBeSameAs(source);
        after.Members.OfType<Cs.ClassDeclaration>().First().Markers.MarkerList.Should().Contain(e => e is SearchResult);
    }
}
