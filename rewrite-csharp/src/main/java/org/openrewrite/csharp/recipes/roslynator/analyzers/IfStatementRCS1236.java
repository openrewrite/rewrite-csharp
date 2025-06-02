package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class IfStatementRCS1236 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1236";
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
        return "Use exception filter";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
