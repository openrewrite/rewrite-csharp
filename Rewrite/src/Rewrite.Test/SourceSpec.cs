using System.Collections;
using Rewrite.Core;
using Rewrite.Core.Marker;
using ExecutionContext = Rewrite.Core.ExecutionContext;

namespace Rewrite.Test;

using ExecutionContext = ExecutionContext;

public delegate SourceFile ValidateSource(SourceFile sourceFile, TypeValidation typeValidation);

public abstract class SourceSpec
{
    public Type SourceFileType { get; }
    public Parser.Builder Parser { get; }
    public string? Before { get; }
    public string? After { get; set; }
    public ValidateSource? ValidateSource { get; set; }
    public Action<ExecutionContext>? CustomizeExecutionContext { get; }
    public Markers Markers { get; }
    public string? SourcePath { get; set; }
    public string Dir { get; }
    public bool Skip { get; }
    public bool NoTrim { get; set; }

    protected SourceSpec(Type sourceFileType,
        Parser.Builder parser,
        string? before,
        ValidateSource? validateSource,
        Action<ExecutionContext>? customizeExecutionContext,
        Markers markers,
        string? sourcePath = null,
        string dir = "",
        bool skip = false,
        bool noTrim = true)
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



    public abstract SourceFile BeforeRecipe(SourceFile sourceFile);
}

public class SourceSpec<T> : SourceSpec where T : SourceFile
{
    private readonly Func<T, T> _beforeRecipeFunc;
    public Action<T> AfterRecipe { get; set; }

    public SourceSpec(Guid Id,
        Parser.Builder Parser,
        string? Before,
        Func<T, T> BeforeRecipeFunc,
        ValidateSource? Validate,
        Action<T> AfterRecipe,
        Action<ExecutionContext> CustomizeExecutionContext,
        string? SourcePath = null) : base(typeof(T), Parser, Before, Validate, CustomizeExecutionContext, Markers.EMPTY, sourcePath: SourcePath)
    {
        _beforeRecipeFunc = BeforeRecipeFunc;
        this.AfterRecipe = AfterRecipe;
    }

    public override SourceFile BeforeRecipe(SourceFile sourceFile)
    {
        return _beforeRecipeFunc((T)sourceFile);
    }
}
