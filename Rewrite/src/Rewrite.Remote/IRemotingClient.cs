using Rewrite.Core;

namespace Rewrite.Remote;

public interface IRemotingClient
{
    void Hello();
    string Print(Cursor cursor);
    void Reset();
    IList<SourceFile?> RunRecipe(string recipe, IDictionary<string, object?> options, IList<SourceFile> sourceFiles);
}