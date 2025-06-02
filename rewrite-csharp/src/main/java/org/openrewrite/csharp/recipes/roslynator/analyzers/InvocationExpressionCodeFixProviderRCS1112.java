package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class InvocationExpressionCodeFixProviderRCS1112 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1112";
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
        return "Combine 'Enumerable.Where' method chain";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
