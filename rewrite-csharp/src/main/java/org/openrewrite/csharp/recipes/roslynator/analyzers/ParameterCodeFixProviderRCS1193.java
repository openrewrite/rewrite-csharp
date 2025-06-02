package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class ParameterCodeFixProviderRCS1193 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1193";
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
        return "Overriding member should not change 'params' modifier";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
