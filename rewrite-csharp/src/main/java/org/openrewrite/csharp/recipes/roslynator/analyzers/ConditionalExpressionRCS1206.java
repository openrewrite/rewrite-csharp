package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class ConditionalExpressionRCS1206 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1206";
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
        return "Use conditional access instead of conditional expression";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
