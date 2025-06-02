package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class ParameterRCS1242 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1242";
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
        return "Do not pass non-read-only struct by read-only reference";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
