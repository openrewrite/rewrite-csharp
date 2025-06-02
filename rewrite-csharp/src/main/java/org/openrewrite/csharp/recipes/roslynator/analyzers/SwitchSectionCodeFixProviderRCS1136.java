package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class SwitchSectionCodeFixProviderRCS1136 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1136";
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
        return "Merge switch sections with equivalent content";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
