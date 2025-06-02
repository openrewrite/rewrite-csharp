package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class UsePatternMatchingInsteadOfAsAndNullCheckCodeFixProviderRCS1221 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1221";
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
        return "Use pattern matching instead of combination of 'as' operator and null check";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
