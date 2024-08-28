using Rewrite.Core;
using ExecutionContext = Rewrite.Core.ExecutionContext;

namespace Rewrite.Test;

public interface ITestExecutionContext
{
    internal static readonly ThreadLocal<ITestExecutionContext> TestContextThreadLocal = new();
    public static ITestExecutionContext? Current() => TestContextThreadLocal.Value;
    
    public static void SetCurrent(ITestExecutionContext executionContext) => TestContextThreadLocal.Value = executionContext;
    
    string Print(Cursor cursor);
    void Reset(ExecutionContext ctx);
    IList<SourceFile?> RunRecipe(Recipe recipe, IDictionary<string, object?> options, IList<SourceFile> sourceFiles);
}