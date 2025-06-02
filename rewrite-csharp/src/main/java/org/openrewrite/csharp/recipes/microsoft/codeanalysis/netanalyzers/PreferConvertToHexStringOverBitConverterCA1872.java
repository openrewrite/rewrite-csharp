package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class PreferConvertToHexStringOverBitConverterCA1872 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA1872";
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
        return "Prefer 'Convert.ToHexString' and 'Convert.ToHexStringLower' over call chains based on 'BitConverter.ToString'";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "Use 'Convert.ToHexString' or 'Convert.ToHexStringLower' when encoding bytes to a hexadecimal string representation. These methods are more efficient and allocation-friendly than using 'BitConverter.ToString' in combination with 'String.Replace' to replace dashes and 'String.ToLower'.";
    }
}
