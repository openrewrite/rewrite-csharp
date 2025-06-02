package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class InterfaceMethodsShouldBeCallableByChildTypesCA1033 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1033";
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
        return "Interface methods should be callable by child types";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "An unsealed externally visible type provides an explicit method implementation of a public interface and does not provide an alternative externally visible method that has the same name.";
    }
}
