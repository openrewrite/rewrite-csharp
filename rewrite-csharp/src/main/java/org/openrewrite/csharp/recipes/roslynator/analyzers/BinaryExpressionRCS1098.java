package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class BinaryExpressionRCS1098 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1098";
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
        return "Constant values should be placed on right side of comparisons";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
