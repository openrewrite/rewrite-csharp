package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class WhileStatementCodeFixProviderRCS1065 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1065";
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
        return "[deprecated] Avoid usage of while statement to create an infinite loop";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
