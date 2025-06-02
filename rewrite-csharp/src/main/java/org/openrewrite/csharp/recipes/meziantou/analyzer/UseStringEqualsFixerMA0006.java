package org.openrewrite.csharp.recipes.meziantou.analyzer;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class UseStringEqualsFixerMA0006 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "MA0006";
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
        return "Use String.Equals instead of equality operator";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
