package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class RemoveEmptyFinalizersFixerCA1821 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1821";
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
        return "Remove empty Finalizers";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Finalizers should be avoided where possible, to avoid the additional performance overhead involved in tracking object lifetime.";
    }
}
