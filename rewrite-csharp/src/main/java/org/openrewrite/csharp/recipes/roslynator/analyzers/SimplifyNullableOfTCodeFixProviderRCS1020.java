package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class SimplifyNullableOfTCodeFixProviderRCS1020 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1020";
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
        return "Simplify Nullable<T> to T?";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
