package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class BinaryExpressionCodeFixProviderRCS1156 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1156";
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
        return "Use string.Length instead of comparison with empty string";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
