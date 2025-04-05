using System.ComponentModel;
using ExecutionContext = Rewrite.Core.ExecutionContext;

namespace Rewrite.RewriteCSharp.Format;

[DisplayName("C# Auto Format")]
[Description("Formats the code for standard whitespace and indentation")]
public class AutoFormatRecipe : Recipe
{
    public override ITreeVisitor<Core.Tree, ExecutionContext> GetVisitor() => new AutoFormatVisitor<ExecutionContext>();
}
