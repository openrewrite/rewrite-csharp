package org.openrewrite.csharp.recipes.meziantou.analyzer;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class ArgumentExceptionShouldSpecifyArgumentNameFixerMA0043 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "MA0043";
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
        return "Use nameof operator in ArgumentException";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
