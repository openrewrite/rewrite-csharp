package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class InterpolatedStringCodeFixProviderRCS1217 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1217";
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
        return "Convert interpolated string to concatenation";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
