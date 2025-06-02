package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class RemovePartialModifierFromTypeWithSinglePartRCS1043 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1043";
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
        return "Remove 'partial' modifier from type with a single part";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
