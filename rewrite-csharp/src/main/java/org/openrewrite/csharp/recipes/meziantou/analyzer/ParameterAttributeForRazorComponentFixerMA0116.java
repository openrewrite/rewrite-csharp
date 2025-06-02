package org.openrewrite.csharp.recipes.meziantou.analyzer;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class ParameterAttributeForRazorComponentFixerMA0116 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "MA0116";
    }

    @Override
    public String getNugetPackageName() {
        return "Meziantou.Analyzer";
    }

    @Override
    public String getNugetPackageVersion() {
        return "2.0.201";
    }

    @Override
    public @NlsRewrite.DisplayName String getDisplayName() {
        return "Parameters with [SupplyParameterFromQuery] attributes should also be marked as [Parameter]";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
