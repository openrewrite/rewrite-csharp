package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class UseVarInsteadOfExplicitTypeRCS1010 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1010";
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
        return "[deprecated] Use 'var' instead of explicit type (when the type is obvious)";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
