package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpForwardCancellationTokenToInvocationsFixerCA2016 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2016";
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
        return "Forward the 'CancellationToken' parameter to methods";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Forward the 'CancellationToken' parameter to methods to ensure the operation cancellation notifications gets properly propagated, or pass in 'CancellationToken.None' explicitly to indicate intentionally not propagating the token.";
    }
}
