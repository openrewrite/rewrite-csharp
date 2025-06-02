package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class SingleLineDocumentationCommentTriviaRCS1100 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1100";
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
        return "[deprecated] Format documentation summary on a single line";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
