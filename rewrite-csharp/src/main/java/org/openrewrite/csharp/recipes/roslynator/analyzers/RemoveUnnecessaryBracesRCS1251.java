package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class RemoveUnnecessaryBracesRCS1251 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1251";
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
        return "Remove unnecessary braces from record declaration";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
