package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class UnnecessaryEnumFlagRCS1258 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1258";
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
        return "Unnecessary enum flag";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
