package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class DeclareEnumMemberWithZeroValueRCS1135 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1135";
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
        return "Declare enum member with zero value (when enum has FlagsAttribute)";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
