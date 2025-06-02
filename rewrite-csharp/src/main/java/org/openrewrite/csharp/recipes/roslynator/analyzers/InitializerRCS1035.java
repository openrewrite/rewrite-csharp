package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class InitializerRCS1035 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1035";
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
        return "[deprecated] Remove redundant comma in initializer";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
