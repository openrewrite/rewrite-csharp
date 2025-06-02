package org.openrewrite.csharp.recipes.meziantou.analyzer;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class MakeInterpolatedStringMA0165 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "MA0165";
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
        return "Make interpolated string";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
