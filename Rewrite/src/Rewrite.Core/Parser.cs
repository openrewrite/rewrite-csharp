using System.Text;

namespace Rewrite.Core;

public interface Parser
{
    abstract class Builder
    {
        public abstract Builder Clone();

        public Type? SourceFileType { get; set; }

        public abstract Parser Build();
    }

    class Input(string sourcePath, Func<Stream> source)
    {
        public string Path { get; set; } = sourcePath;

        public string GetRelativePath(string? relativeTo) {
            return relativeTo == null ? Path : System.IO.Path.GetRelativePath(relativeTo, Path);
        }

        public Stream GetSource(ExecutionContext ctx)
        {
            return source.Invoke();
        }
    }

    string SourcePathFromSourceText(string prefix, string sourceCode);
    IEnumerable<SourceFile> ParseInputs(IEnumerable<Input> sources, string? relativeTo, ExecutionContext ctx);

    SourceFile Parse(string source)
    {
        var input = new Core.Parser.Input(SourcePathFromSourceText("", source), () => new MemoryStream(Encoding.UTF8.GetBytes(source)));
        return ParseInputs([input], null, new InMemoryExecutionContext()).Single();
    }

    string GetCharset(ExecutionContext ctx)
    {
        // FIXME implement
        return "UTF-8";
    }
}
