package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class UseEnvironmentMembersCA1840 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1840";
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
        return "Use 'Environment.CurrentManagedThreadId'";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "'Environment.CurrentManagedThreadId' is simpler and faster than 'Thread.CurrentThread.ManagedThreadId'.";
    }
}
