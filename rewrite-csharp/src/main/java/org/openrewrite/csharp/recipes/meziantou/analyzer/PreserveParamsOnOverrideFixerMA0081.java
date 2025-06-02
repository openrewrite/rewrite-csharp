package org.openrewrite.csharp.recipes.meziantou.analyzer;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class PreserveParamsOnOverrideFixerMA0081 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "MA0081";
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
        return "Method overrides should not omit params keyword";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
