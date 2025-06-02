package org.openrewrite.csharp.recipes.meziantou.analyzer;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class ValidateArgumentsCorrectlyFixerMA0050 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "MA0050";
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
        return "Validate arguments correctly in iterator methods";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
