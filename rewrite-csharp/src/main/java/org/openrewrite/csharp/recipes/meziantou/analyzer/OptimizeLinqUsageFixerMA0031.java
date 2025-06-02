package org.openrewrite.csharp.recipes.meziantou.analyzer;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class OptimizeLinqUsageFixerMA0031 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "MA0031";
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
        return "Optimize Enumerable.Count() usage";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
