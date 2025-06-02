package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpDoNotPassNonNullableValueToArgumentNullExceptionThrowIfNullCA1871 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1871";
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
        return "Do not pass a nullable struct to 'ArgumentNullException.ThrowIfNull'";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "'ArgumentNullException.ThrowIfNull' accepts an 'object', so passing a nullable struct may cause the value to be boxed.";
    }
}
