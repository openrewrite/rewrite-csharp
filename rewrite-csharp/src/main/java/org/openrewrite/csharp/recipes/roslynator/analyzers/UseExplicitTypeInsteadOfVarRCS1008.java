package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class UseExplicitTypeInsteadOfVarRCS1008 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1008";
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
        return "[deprecated] Use explicit type instead of 'var' (when the type is not obvious)";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
