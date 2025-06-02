package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpDoNotUseCountWhenAnyCanBeUsedCA1827 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1827";
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
        return "Do not use Count() or LongCount() when Any() can be used";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "For non-empty collections, Count() and LongCount() enumerate the entire sequence, while Any() stops at the first item or the first item that satisfies a condition.";
    }
}
