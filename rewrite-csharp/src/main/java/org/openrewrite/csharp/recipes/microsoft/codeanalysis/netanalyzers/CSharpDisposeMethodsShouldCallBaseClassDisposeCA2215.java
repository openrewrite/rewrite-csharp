package org.openrewrite.csharp.recipes.microsoft.codeanalysis.netanalyzers;

import org.openrewrite.NlsRewrite;
import org.openrewrite.csharp.RoslynRecipe;

public class CSharpDisposeMethodsShouldCallBaseClassDisposeCA2215 extends RoslynRecipe {

    @Override
    public String getRecipeId() {
        return "CA2215";
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
        return "Dispose methods should call base class dispose";
    }

    @Override
    public @NlsRewrite.Description String getDescription() {
        return "A type that implements System.IDisposable inherits from a type that also implements IDisposable. The Dispose method of the inheriting type does not call the Dispose method of the parent type. To fix a violation of this rule, call base.Dispose in your Dispose method.";
    }
}
