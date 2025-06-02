package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class AvoidUnreliableStreamReadFixerCA2022 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2022";
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
        return "Avoid inexact read with 'Stream.Read'";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "A call to 'Stream.Read' may return fewer bytes than requested, resulting in unreliable code if the return value is not checked.";
    }
}
