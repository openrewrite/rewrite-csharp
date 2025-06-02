package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class IfStatementRCS1061 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1061";
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
        return "Merge 'if' with nested 'if'";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
