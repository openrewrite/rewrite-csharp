namespace Rewrite.Core;

public class ExampleAttribute(string example) : Attribute
{
    public string Example { get; } = example;
}