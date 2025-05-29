using System.ComponentModel;
using System.Text.RegularExpressions;
using Rewrite.Core;
using Rewrite.RewriteJson;
using Rewrite.RewriteJson.Tree;

namespace Rewrite.Recipes.Json;

[DisplayName("Indent")]
[Description("Fix JSON indentation")]
public class IndentRecipe : Recipe
{
    
    public override ITreeVisitor<Tree, IExecutionContext> GetVisitor()
    {
        return new IndentVisitor();
    }

    private class IndentVisitor : JsonVisitor<IExecutionContext>
    {
        public override Space VisitSpace(Space space, IExecutionContext ctx)
        {
            if (Cursor.Value is RewriteJson.Tree.Json.Document doc)
            {
                if (doc.Prefix == space)
                {
                    space = space.WithWhitespace("  " + space.Whitespace);
                }

                if (doc.Eof == space && space.Whitespace != null)
                {
                    space = space.WithWhitespace(Regex.Replace(space.Whitespace, @"\n(?!$)", "\n  "));
                }
            }
            else if (space.Whitespace != null)
            {
                space = space.WithWhitespace(space.Whitespace.Replace("\n", "\n  "));
            }

            space = space.WithComments(ListUtils.Map(space.Comments, comment =>
            {
                comment = comment.WithText(comment.Text.Replace("\n", "\n  "));
                comment = comment.WithSuffix(comment.Suffix.Replace("\n", "\n  "));
                return comment;
            }));

            return space;
        }
    }
}
