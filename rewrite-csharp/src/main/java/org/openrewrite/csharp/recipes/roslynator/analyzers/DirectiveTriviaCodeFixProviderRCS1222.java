package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class DirectiveTriviaCodeFixProviderRCS1222 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1222";
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
        return "Merge preprocessor directives";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
