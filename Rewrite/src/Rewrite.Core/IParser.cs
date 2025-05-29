using System.Text;

namespace Rewrite.Core;

public interface IParser
{
    abstract class Builder
    {
        public abstract Builder Clone();

        public Type? SourceFileType { get; set; }

        public abstract IParser Build();
    }

    class Input(string sourcePath, Func<Stream> source)
    {
        public string Path { get; set; } = sourcePath;

        public string GetRelativePath(string? relativeTo) => relativeTo == null ? Path : System.IO.Path.GetRelativePath(relativeTo, Path);

        public Stream GetSource(IExecutionContext ctx) => source.Invoke();
    }

    string SourcePathFromSourceText(string prefix, string sourceCode);
    IEnumerable<SourceFile> ParseInputs(IEnumerable<Input> sources, string? relativeTo, IExecutionContext ctx);

    SourceFile Parse(string source)
    {
        var input = new Core.IParser.Input(SourcePathFromSourceText("", source), () => new MemoryStream(Encoding.UTF8.GetBytes(source)));
        return ParseInputs([input], null, new InMemoryExecutionContext()).Single();
    }

    string GetCharset(IExecutionContext ctx)
    {
        // FIXME implement
        return "UTF-8";
    }
}
