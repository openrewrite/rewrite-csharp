using Rewrite.Core;

namespace Rewrite.Test;

public interface ITestExecutionContext
{
    internal static ITestExecutionContext? TestContextThreadLocal;
    public static ITestExecutionContext? Current() => TestContextThreadLocal;

    public static void SetCurrent(ITestExecutionContext executionContext) => TestContextThreadLocal = executionContext;

    string Print(Cursor cursor);
    void Reset(IExecutionContext ctx);
    IList<SourceFile?> RunRecipe(Recipe recipe, IDictionary<string, object?> options, IList<SourceFile> sourceFiles);
}
