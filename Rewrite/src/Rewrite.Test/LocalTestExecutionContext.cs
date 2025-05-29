using Rewrite.Core;

namespace Rewrite.Test;

public class LocalTestExecutionContext : ITestExecutionContext
{
    public string Print(Cursor cursor)
    {
        var tree = cursor.Value as Tree ?? throw new InvalidOperationException("Current value stored in Cursor is not a Tree");
        return tree.Print(cursor, new PrintOutputCapture<int>(0));
    }

    public void Reset(IExecutionContext ctx)
    {
        // ignore
    }

    public IList<SourceFile?> RunRecipe(Recipe recipe, IDictionary<string, object?> options, IList<SourceFile> sourceFiles)
    {
        return sourceFiles.Select(x => (SourceFile?)recipe.GetVisitor().Visit(x, new InMemoryExecutionContext())).ToList();
    }


}
