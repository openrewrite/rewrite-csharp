package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class DoNotUseWhenAllOrWaitAllWithSingleArgumentFixerCA1843 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1843";
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
        return "Do not use 'WaitAll' with a single task";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Using 'WaitAll' with a single task may result in performance loss, await or return the task instead.";
    }
}
