package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpDoNotPassNonNullableValueToArgumentNullExceptionThrowIfNullCA2264 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2264";
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
        return "Do not pass a non-nullable value to 'ArgumentNullException.ThrowIfNull'";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "'ArgumentNullException.ThrowIfNull' throws when the passed argument is 'null'. Certain constructs like non-nullable structs, 'nameof()' and 'new' expressions are known to never be null, so 'ArgumentNullException.ThrowIfNull' will never throw.";
    }
}
