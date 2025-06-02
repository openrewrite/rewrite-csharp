package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class BaseArgumentListCodeFixProviderRCS1205 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1205";
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
        return "Order named arguments according to the order of parameters";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
