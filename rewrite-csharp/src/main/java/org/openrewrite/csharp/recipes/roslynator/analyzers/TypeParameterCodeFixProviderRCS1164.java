package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class TypeParameterCodeFixProviderRCS1164 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1164";
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
        return "Unused type parameter";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
