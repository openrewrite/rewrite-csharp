package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class UseEnvironmentMembersFixerCA1839 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1839";
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
        return "Use 'Environment.ProcessPath'";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "'Environment.ProcessPath' is simpler and faster than 'Process.GetCurrentProcess().MainModule.FileName'.";
    }
}
