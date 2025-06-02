package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class UseCancellationTokenThrowIfCancellationRequestedCA2250 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2250";
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
        return "Use 'ThrowIfCancellationRequested'";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "'ThrowIfCancellationRequested' automatically checks whether the token has been canceled, and throws an 'OperationCanceledException' if it has.";
    }
}
