package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class OptimizeLinqMethodCallCodeFixProviderRCS1246 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1246";
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
        return "Use element access";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
