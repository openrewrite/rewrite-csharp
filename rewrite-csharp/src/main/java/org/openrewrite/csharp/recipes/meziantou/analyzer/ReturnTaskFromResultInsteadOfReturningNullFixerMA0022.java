package org.openrewrite.csharp.recipes.meziantou.analyzer;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class ReturnTaskFromResultInsteadOfReturningNullFixerMA0022 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "MA0022";
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
        return "Return Task.FromResult instead of returning null";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
