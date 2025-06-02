package org.openrewrite.csharp.recipes.meziantou.analyzer;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class StringShouldNotContainsNonDeterministicEndOfLineMA0101 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "MA0101";
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
        return "String contains an implicit end of line character";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
