package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpUseSearchValuesFixerCA1870 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1870";
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
        return "Use a cached 'SearchValues' instance";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Using a cached 'SearchValues' instance is more efficient than passing values to 'IndexOfAny'/'ContainsAny' directly.";
    }
}
