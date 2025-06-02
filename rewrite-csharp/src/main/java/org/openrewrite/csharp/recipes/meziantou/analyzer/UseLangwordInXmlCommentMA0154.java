package org.openrewrite.csharp.recipes.meziantou.analyzer;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class UseLangwordInXmlCommentMA0154 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "MA0154";
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
        return "Use langword in XML comment";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
