package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpMakeTypesInternalFixerCA1515 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1515";
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
        return "Consider making public types internal";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Unlike a class library, an application's API isn't typically referenced publicly, so types can be marked internal.";
    }
}
