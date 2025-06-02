package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class OverloadOperatorEqualsOnOverridingValueTypeEqualsCA2231 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2231";
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
        return "Overload operator equals on overriding value type Equals";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "In most programming languages there is no default implementation of the equality operator (==) for value types. If your programming language supports operator overloads, you should consider implementing the equality operator. Its behavior should be identical to that of Equals.";
    }
}
