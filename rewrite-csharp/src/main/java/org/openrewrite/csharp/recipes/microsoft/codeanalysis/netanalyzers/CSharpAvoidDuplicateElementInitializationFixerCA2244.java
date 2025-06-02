package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpAvoidDuplicateElementInitializationFixerCA2244 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2244";
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
        return "Do not duplicate indexed element initializations";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Indexed elements in objects initializers must initialize unique elements. A duplicate index might overwrite a previous element initialization.";
    }
}
