package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class UseBlockBodyOrExpressionBodyRCS1016 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1016";
    }

    @Override
    public String getNugetPackageName() {
        return "Roslynator.Analyzers";
    }

    @Override
    public String getNugetPackageVersion() {
        return "4.13.1";
    }

    @Override
    public @NlsRewrite.DisplayName String getDisplayName() {
        return "Use block body or expression body";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
