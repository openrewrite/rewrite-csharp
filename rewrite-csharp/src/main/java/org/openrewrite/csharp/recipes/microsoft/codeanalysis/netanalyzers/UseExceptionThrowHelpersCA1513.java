package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class UseExceptionThrowHelpersCA1513 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1513";
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
        return "Use ObjectDisposedException throw helper";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Throw helpers are simpler and more efficient than an if block constructing a new exception instance.";
    }
}
