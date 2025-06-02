package org.openrewrite.csharp.recipes.roslynator.analyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class UseEmptyStringLiteralOrStringEmptyRCS1078 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "RCS1078";
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
        return "Use \"\" or 'string.Empty'";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "";
    }
}
