package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class PreferStringContainsOverIndexOfCA2249 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2249";
    }

    @Override
    public String getNugetPackageName() {
        return "Microsoft.CodeAnalysis.NetAnalyzers";
    }

    @Override
    public String getNugetPackageVersion() {
        return "9.0.0";
    }

    @Override
    public @NlsRewrite.DisplayName String getDisplayName() {
        return "Consider using 'string.Contains' instead of 'string.IndexOf'";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Calls to 'string.IndexOf' where the result is used to check for the presence/absence of a substring can be replaced by 'string.Contains'.";
    }
}
