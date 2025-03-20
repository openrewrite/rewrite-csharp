using ExecutionContext = Rewrite.Core.ExecutionContext;

namespace Rewrite.RewriteCSharp.Format;

public class AutoFormatRecipe : Recipe
{
    public override string DisplayName => "C# Auto Format";
    public override string Description => "Formats the code for standard whitespace and indentation";
    public override ITreeVisitor<Core.Tree, ExecutionContext> GetVisitor() => new AutoFormatVisitor<ExecutionContext>();
}
