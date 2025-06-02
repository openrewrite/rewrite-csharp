package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class SwitchSectionRCS1111 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1111";
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
        return "Add braces to switch section with multiple statements";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
