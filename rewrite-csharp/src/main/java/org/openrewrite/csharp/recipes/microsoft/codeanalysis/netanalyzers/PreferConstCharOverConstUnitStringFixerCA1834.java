package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class PreferConstCharOverConstUnitStringFixerCA1834 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1834";
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
        return "Consider using 'StringBuilder.Append(char)' when applicable";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "'StringBuilder.Append(char)' is more efficient than 'StringBuilder.Append(string)' when the string is a single character. When calling 'Append' with a constant, prefer using a constant char rather than a constant string containing one character.";
    }
}
