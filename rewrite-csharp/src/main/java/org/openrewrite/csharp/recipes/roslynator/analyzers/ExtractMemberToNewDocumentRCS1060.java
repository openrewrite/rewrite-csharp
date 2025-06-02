package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class ExtractMemberToNewDocumentRCS1060 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1060";
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
        return "Declare each type in separate file";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
