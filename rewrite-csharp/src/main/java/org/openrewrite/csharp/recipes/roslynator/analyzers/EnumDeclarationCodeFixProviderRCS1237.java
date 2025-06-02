package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class EnumDeclarationCodeFixProviderRCS1237 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1237";
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
        return "[deprecated] Use bit shift operator";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
