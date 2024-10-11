namespace Rewrite.CSharp.Tests.RoslynTestCases;

public record SourceTestCase(string Name, string SourceText)
{
    public override string ToString() => Name;
}
