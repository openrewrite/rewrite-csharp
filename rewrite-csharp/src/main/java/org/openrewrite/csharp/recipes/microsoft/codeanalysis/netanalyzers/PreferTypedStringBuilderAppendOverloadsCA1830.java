package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class PreferTypedStringBuilderAppendOverloadsCA1830 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1830";
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
        return "Prefer strongly-typed Append and Insert method overloads on StringBuilder";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "StringBuilder.Append and StringBuilder.Insert provide overloads for multiple types beyond System.String.  When possible, prefer the strongly-typed overloads over using ToString() and the string-based overload.";
    }
}
