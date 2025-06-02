package org.openrewrite.csharp.recipes.meziantou.analyzer;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class NotPatternShouldBeParenthesizedCodeMA0164 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "MA0164";
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
        return "Use parentheses to make not pattern clearer";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
