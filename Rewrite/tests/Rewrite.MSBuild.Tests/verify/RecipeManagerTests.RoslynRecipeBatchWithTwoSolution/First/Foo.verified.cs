namespace First;

public class Foo
{
    private static readonly string[] value = new[] { "Hello", "world!" };

    string Test() => string.Join(" ", value);
}