package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class DoNotUseWhenAllOrWaitAllWithSingleArgumentFixerCA1842 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1842";
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
        return "Do not use 'WhenAll' with a single task";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Using 'WhenAll' with a single task may result in performance loss, await or return the task instead.";
    }
}
