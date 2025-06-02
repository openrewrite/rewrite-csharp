package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class AddOrRemoveTrailingCommaRCS1260 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1260";
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
        return "Add/remove trailing comma";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
