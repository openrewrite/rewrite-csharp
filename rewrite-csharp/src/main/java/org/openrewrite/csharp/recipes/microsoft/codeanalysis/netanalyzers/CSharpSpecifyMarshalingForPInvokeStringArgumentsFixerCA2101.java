package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpSpecifyMarshalingForPInvokeStringArgumentsFixerCA2101 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2101";
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
        return "Specify marshaling for P/Invoke string arguments";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "A platform invoke member allows partially trusted callers, has a string parameter, and does not explicitly marshal the string. This can cause a potential security vulnerability.";
    }
}
