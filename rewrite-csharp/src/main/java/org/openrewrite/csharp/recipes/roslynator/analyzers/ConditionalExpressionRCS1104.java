package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class ConditionalExpressionRCS1104 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1104";
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
        return "Simplify conditional expression";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
