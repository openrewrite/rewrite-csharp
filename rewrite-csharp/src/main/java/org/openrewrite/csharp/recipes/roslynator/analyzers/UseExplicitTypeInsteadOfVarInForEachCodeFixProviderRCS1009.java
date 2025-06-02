package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class UseExplicitTypeInsteadOfVarInForEachCodeFixProviderRCS1009 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1009";
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
        return "[deprecated] Use explicit type instead of 'var' (foreach variable)";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
