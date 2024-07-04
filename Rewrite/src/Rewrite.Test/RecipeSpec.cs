using Rewrite.Core;
using ExecutionContext = Rewrite.Core.ExecutionContext;

namespace Rewrite.Test;

public class RecipeSpec
{
    public Recipe? Recipe { get; set; }

    public PrintOutputCapture<int>.IMarkerPrinter? MarkerPrinter { get; set; }
    public int? Cycles { get; set; }
    public int? ExpectedCyclesThatMakeChanges { get; set; }
    public ExecutionContext? ExecutionContext { get; set; }
    public List<Parser.Builder> Parsers { get; set; } = [];
    public List<Action<SourceSpec>> AllSources { get; set; } = [];
    public string? RelativeTo { get; set; }
    public TypeValidation? TypeValidation { get; set; }

    public static RecipeSpec Defaults()
    {
        return new RecipeSpec();
    }
}