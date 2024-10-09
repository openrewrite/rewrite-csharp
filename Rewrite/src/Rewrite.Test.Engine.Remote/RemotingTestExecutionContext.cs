using Rewrite.Core;
using Rewrite.Remote;
using Rewrite.Test;
using ExecutionContext = Rewrite.Core.ExecutionContext;

namespace Rewrite.Test.Remote;

public class RemotingTestExecutionContext(IRemotingContext remotingContext) : ITestExecutionContext
{
    public string Print(Cursor cursor) => remotingContext.Client!.Print(cursor);

    public void Reset(ExecutionContext ctx)
    {
        remotingContext.Reset();
        remotingContext.Client!.Reset();
        RemotingExecutionContextView.View(ctx).RemotingContext = remotingContext;
    }

    public IList<SourceFile?> RunRecipe(Recipe recipe, IDictionary<string, object?> options, IList<SourceFile> sourceFiles)
    {
        IRemotingContext.RegisterRecipeFactory(recipe.GetType().FullName!, _ => recipe);
        return remotingContext.Client!.RunRecipe(recipe.GetType().FullName!, options, sourceFiles);
    }
}
