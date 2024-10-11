namespace Rewrite.CSharp.Tests.RoslynTestCases;

public record CSharpSyntaxFragment(string Name, string Content)
{
    public override string ToString() => Name;
}
