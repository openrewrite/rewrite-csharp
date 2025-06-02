package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class SwitchSectionRCS1099 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1099";
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
        return "Default label should be the last label in a switch section";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
