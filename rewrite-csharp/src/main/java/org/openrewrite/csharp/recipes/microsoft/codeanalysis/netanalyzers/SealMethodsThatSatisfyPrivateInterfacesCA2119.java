package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class SealMethodsThatSatisfyPrivateInterfacesCA2119 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2119";
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
        return "Seal methods that satisfy private interfaces";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "An inheritable public type provides an overridable method implementation of an internal (Friend in Visual Basic) interface. To fix a violation of this rule, prevent the method from being overridden outside the assembly.";
    }
}
