package org.openrewrite.csharp.recipes.meziantou.analyzer;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class UseAnOverloadThatHasTimeProviderMA0166 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "MA0166";
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
        return "Forward the TimeProvider to methods that take one";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
