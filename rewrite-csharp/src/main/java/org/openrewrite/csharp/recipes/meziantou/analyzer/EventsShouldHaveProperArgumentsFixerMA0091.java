package org.openrewrite.csharp.recipes.meziantou.analyzer;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class EventsShouldHaveProperArgumentsFixerMA0091 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "MA0091";
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
        return "Sender should be 'this' for instance events";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
