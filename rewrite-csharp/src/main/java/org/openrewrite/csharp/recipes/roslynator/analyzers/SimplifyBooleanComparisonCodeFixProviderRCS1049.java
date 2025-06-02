package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class SimplifyBooleanComparisonCodeFixProviderRCS1049 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1049";
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
        return "Simplify boolean comparison";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
