package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class AvoidUnusedPrivateFieldsFixerCA1823 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1823";
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
        return "Avoid unused private fields";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Private fields were detected that do not appear to be accessed in the assembly.";
    }
}
