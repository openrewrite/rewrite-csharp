using System.Text.RegularExpressions;
using Rewrite.Core;
using Rewrite.RewriteJson;
using Rewrite.RewriteJson.Tree;
using ExecutionContext = Rewrite.Core.ExecutionContext;

namespace Rewrite.Recipes.Json;

using ExecutionContext = ExecutionContext;

public class Indent : Recipe
{
    public override string DisplayName => "Indent";
    
    public override string Description => "Fix JSON indentation"; 
    
    public override ITreeVisitor<Tree, ExecutionContext> GetVisitor()
    {
        return new IndentVisitor();
    }

    private class IndentVisitor : JsonVisitor<ExecutionContext>
    {
        public override Space VisitSpace(Space space, ExecutionContext ctx)
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
