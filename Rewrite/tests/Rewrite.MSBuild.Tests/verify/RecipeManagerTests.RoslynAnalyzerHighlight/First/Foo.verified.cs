namespace First;

public class Foo
{
    string Test() => string.Join(" ", /* >> CA1861 */ new[] { "Hello", "world!" });
}