package org.openrewrite.csharp.recipes.meziantou.analyzer;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CommaMA0007 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "MA0007";
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
        return "Add a comma after the last value";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
