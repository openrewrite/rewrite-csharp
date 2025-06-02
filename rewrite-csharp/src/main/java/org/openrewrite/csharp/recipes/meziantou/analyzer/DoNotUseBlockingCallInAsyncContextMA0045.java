package org.openrewrite.csharp.recipes.meziantou.analyzer;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class DoNotUseBlockingCallInAsyncContextMA0045 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "MA0045";
    }

    @Override
    public String getNugetPackageName() {
        return "Meziantou.Analyzer";
    }

    @Override
    public String getNugetPackageVersion() {
        return "2.0.201";
    }

    @Override
    public @NlsRewrite.DisplayName String getDisplayName() {
        return "Do not use blocking calls in a sync method (need to make calling method async)";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
