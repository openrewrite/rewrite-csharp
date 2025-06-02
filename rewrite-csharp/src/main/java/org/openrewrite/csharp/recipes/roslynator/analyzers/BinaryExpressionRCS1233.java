package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class BinaryExpressionRCS1233 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1233";
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
        return "Use short-circuiting operator";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
