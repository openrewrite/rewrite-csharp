package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class OperatorsShouldHaveSymmetricalOverloadsFixerCA2226 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2226";
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
        return "Operators should have symmetrical overloads";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "A type implements the equality or inequality operator and does not implement the opposite operator.";
    }
}
