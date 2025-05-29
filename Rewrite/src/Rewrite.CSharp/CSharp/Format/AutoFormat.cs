using System.ComponentModel;

namespace Rewrite.RewriteCSharp.Format;

[DisplayName("C# Auto Format")]
[Description("Formats the code for standard whitespace and indentation")]
public class AutoFormatRecipe : Recipe
{
    public override ITreeVisitor<Core.Tree, IExecutionContext> GetVisitor() => new AutoFormatVisitor<IExecutionContext>();
}
