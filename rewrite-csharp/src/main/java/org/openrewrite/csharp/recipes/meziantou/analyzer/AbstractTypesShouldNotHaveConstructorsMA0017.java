package org.openrewrite.csharp.recipes.meziantou.analyzer;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class AbstractTypesShouldNotHaveConstructorsMA0017 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "MA0017";
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
        return "Abstract types should not have public or internal constructors";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
