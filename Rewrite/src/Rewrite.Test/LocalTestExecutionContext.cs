﻿using Rewrite.Core;
using ExecutionContext = Rewrite.Core.ExecutionContext;

namespace Rewrite.Test;

public class LocalTestExecutionContext : ITestExecutionContext
{
    public string Print(Cursor cursor)
    {
        var tree = cursor.Value as Tree ?? throw new InvalidOperationException("Current value stored in Cursor is not a Tree");
        return tree.Print(cursor, new PrintOutputCapture<int>(0));
    }

    public void Reset(ExecutionContext ctx)
    {
        // ignore
    }

    public IList<SourceFile?> RunRecipe(Recipe recipe, IDictionary<string, object?> options, IList<SourceFile> sourceFiles)
    {
        if(recipe == Recipe.Noop())
            return sourceFiles.Cast<SourceFile?>().ToList();
        throw new NotImplementedException();
    }
}