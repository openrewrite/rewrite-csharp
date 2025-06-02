package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpUseAsSpanInsteadOfRangeIndexerCA1831 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1831";
    }

    @Override
    public String getNugetPackageName() {
        return "Microsoft.CodeAnalysis.NetAnalyzers";
    }

    @Override
    public String getNugetPackageVersion() {
        return "9.0.0";
    }

    @Override
    public @NlsRewrite.DisplayName String getDisplayName() {
        return "Use AsSpan or AsMemory instead of Range-based indexers when appropriate";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "The Range-based indexer on string values produces a copy of requested portion of the string. This copy is usually unnecessary when it is implicitly used as a ReadOnlySpan or ReadOnlyMemory value. Use the AsSpan method to avoid the unnecessary copy.";
    }
}
