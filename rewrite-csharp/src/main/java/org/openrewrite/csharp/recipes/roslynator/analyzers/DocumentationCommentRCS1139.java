package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class DocumentationCommentRCS1139 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1139";
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
        return "Add summary element to documentation comment";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
