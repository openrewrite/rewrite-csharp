package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class MemberDeclarationCodeFixProviderRCS1224 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1224";
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
        return "Make method an extension method";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
