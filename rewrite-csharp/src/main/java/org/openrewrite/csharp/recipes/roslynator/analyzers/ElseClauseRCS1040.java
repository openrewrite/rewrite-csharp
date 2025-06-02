package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class ElseClauseRCS1040 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1040";
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
        return "[deprecated] Remove empty 'else' clause";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
