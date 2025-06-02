package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class NamespaceDeclarationRCS1072 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1072";
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
        return "[deprecated] Remove empty namespace declaration";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
