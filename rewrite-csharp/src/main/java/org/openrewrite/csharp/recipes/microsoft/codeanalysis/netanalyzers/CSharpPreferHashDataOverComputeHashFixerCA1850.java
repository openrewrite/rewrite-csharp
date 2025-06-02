package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpPreferHashDataOverComputeHashFixerCA1850 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1850";
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
        return "Prefer static 'HashData' method over 'ComputeHash'";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "It is more efficient to use the static 'HashData' method over creating and managing a HashAlgorithm instance to call 'ComputeHash'.";
    }
}
