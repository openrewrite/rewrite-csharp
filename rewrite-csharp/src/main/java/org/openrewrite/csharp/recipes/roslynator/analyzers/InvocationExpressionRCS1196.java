package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class InvocationExpressionRCS1196 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1196";
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
        return "Call extension method as instance method";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
