package org.openrewrite.csharp.recipes.meziantou.analyzer;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class MakeMethodStaticFixerMA0038 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "MA0038";
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
        return "Make method static (deprecated, use CA1822 instead)";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
