package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class DuplicateWordInCommentCodeFixProviderRCS1243 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1243";
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
        return "Duplicate word in a comment";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
