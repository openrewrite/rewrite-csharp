package org.openrewrite.csharp.recipes.meziantou.analyzer;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class UseJSRuntimeInvokeVoidAsyncWhenReturnValueIsNotUsedFixerMA0120 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "MA0120";
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
        return "Use InvokeVoidAsync when the returned value is not used";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
