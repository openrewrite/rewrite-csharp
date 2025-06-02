package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class ClassDeclarationRCS1102 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1102";
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
        return "Make class static";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
