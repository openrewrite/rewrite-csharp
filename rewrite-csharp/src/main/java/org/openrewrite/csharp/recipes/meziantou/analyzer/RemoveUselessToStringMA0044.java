package org.openrewrite.csharp.recipes.meziantou.analyzer;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class RemoveUselessToStringMA0044 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "MA0044";
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
        return "Remove useless ToString call";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
