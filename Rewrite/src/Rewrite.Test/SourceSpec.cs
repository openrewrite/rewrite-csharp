using System.Collections;
using System.Text;
using Rewrite.Core;
using Rewrite.Core.Marker;

namespace Rewrite.Test;

public delegate SourceFile ValidateSource(SourceFile sourceFile, TypeValidation typeValidation);

public abstract class SourceSpec : IParser.Input
{
    public Type SourceFileType { get; }
    public IParser.Builder Parser { get; }
    public string? Before { get; }
    public string? After { get; set; }
    public ValidateSource? ValidateSource { get; set; }
    public Action<IExecutionContext>? CustomizeExecutionContext { get; }
    public Markers Markers { get; }
    public string? SourcePath { get; set; }
    public string Dir { get; }
    public bool Skip { get; }
    public bool NoTrim { get; set; }

    protected SourceSpec(Type sourceFileType,
        IParser.Builder parser,
        string? before,
        ValidateSource? validateSource,
        Action<IExecutionContext>? customizeExecutionContext,
        Markers markers,
        string? sourcePath = null,
        string dir = "",
        bool skip = false,
        bool noTrim = true) : base(GetAbsolutePath(dir, sourcePath, GetBefore(before, out var trimmed, !noTrim ), parser), () => new MemoryStream(Encoding.UTF8.GetBytes(trimmed)))
    {
        this.SourceFileType = sourceFileType;
        this.Parser = parser;
        this.Before = before;
        this.ValidateSource = validateSource;
        this.CustomizeExecutionContext = customizeExecutionContext;
        this.Markers = markers;
        this.SourcePath = sourcePath;
        this.Dir = dir;
        this.Skip = skip;
        this.NoTrim = noTrim;
    }

    static string GetBefore(string? original, out string trimmed, bool shouldTrim)
    {
        trimmed = (!shouldTrim ? original : TrimIndentPreserveCrLf(original)) ?? "";
        return trimmed;
    }
    
    private static string? TrimIndentPreserveCrLf(string? text)
    {
        if (text == null) return null;

        text = text.EndsWith("\r\n") ? text.Substring(0, text.Length - 2) : text;
        text = text.Replace('\r', '⏎');
        text = text.Replace('⏎', '\r');
        // FIXME implement `StringUtils.TrimIndent()` as required
        return text;
    }
    private static string GetAbsolutePath(string dir, string? path, string? before, IParser.Builder parser)
    {
        var sourcePath = path != null
            ? System.IO.Path.Combine(dir, path)
            : parser.Build().SourcePathFromSourceText(dir, before ?? "");
        return sourcePath;
    }

    public abstract SourceFile BeforeRecipe(SourceFile sourceFile);
}

public class SourceSpec<T> : SourceSpec where T : SourceFile
{
    private readonly Func<T, T> _beforeRecipeFunc;
    public Action<T> AfterRecipe { get; set; }

    public SourceSpec(Guid id,
        IParser.Builder parser,
        string? before,
        Func<T, T> beforeRecipeFunc,
        ValidateSource? validate,
        Action<T> afterRecipe,
        Action<IExecutionContext> customizeExecutionContext,
        string? sourcePath = null) : base(typeof(T), parser, before, validate, customizeExecutionContext, Markers.EMPTY, sourcePath: sourcePath)
    {
        _beforeRecipeFunc = beforeRecipeFunc;
        this.AfterRecipe = afterRecipe;
    }

    public override SourceFile BeforeRecipe(SourceFile sourceFile)
    {
        return _beforeRecipeFunc((T)sourceFile);
    }
}
