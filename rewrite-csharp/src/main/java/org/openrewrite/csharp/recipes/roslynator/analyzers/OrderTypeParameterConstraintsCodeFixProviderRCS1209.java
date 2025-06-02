package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class OrderTypeParameterConstraintsCodeFixProviderRCS1209 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1209";
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
        return "Order type parameter constraints";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
