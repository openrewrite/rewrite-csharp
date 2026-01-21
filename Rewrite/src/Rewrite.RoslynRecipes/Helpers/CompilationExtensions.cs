using Microsoft.CodeAnalysis;

namespace Rewrite.RoslynRecipes.Helpers;

public static class CompilationExtensions
{
    extension(Compilation compilation)
    {
        /// <summary>
        /// Gets the type within the compilation's assembly and all referenced assemblies (other than
        /// those that can only be referenced via an extern alias) using its canonical CLR metadata name.
        /// If the type cannot be resolved, throws <see cref="Rewrite.RoslynRecipes.UnknownTypeException"/>
        /// This lookup follows the following order:
        /// <list type="number">
        /// <item><description>If the type is found in the compilation's assembly, that type is returned.</description></item>
        /// <item><description>
        /// Next, the core library (the library that defines <c>System.Object</c> and has no assembly references) is searched.
        /// If the type is found there, that type is returned.
        /// </description></item>
        /// <item><description>
        /// Finally, all remaining referenced non-extern assemblies are searched. If one and only one type matching the provided metadata name is found, that
        /// single type is returned. Accessibility is ignored for this check.
        /// </description></item>
        /// </list>
        /// </summary>
        /// <returns>Null if the type can't be found or there was an ambiguity during lookup.</returns>
        /// <remarks>
        /// <para>
        /// Since VB does not have the concept of extern aliases, it considers all referenced assemblies.
        /// </para>
        /// <para>
        /// In C#, if the core library is referenced as an extern assembly, it will be searched. All other extern-aliased assemblies will not be searched.
        /// </para>
        /// <para>
        /// Because accessibility to the current assembly is ignored when searching for types that match the provided metadata name, if multiple referenced
        /// assemblies define the same type symbol (as often happens when users copy well-known types from the BCL or other sources) then this API will return null,
        /// even if all but one of those symbols would be otherwise inaccessible to user-written code in the current assembly. For fine-grained control over ambiguity
        /// resolution, consider using <see cref=" Microsoft.CodeAnalysis.Compilation.GetTypesByMetadataName(string)" /> instead and filtering the results for the symbol required.
        /// </para>
        /// <para>
        /// Assemblies can contain multiple modules. Within each assembly, the search is performed based on module's position in the module list of that assembly. When
        /// a match is found in one module in an assembly, no further modules within that assembly are searched.
        /// </para>
        /// <para>Type forwarders are ignored, and not considered part of the assembly where the TypeForwardAttribute is written.</para>
        /// <para>
        /// Ambiguities are detected on each nested level. For example, if <c>A+B</c> is requested, and there are multiple <c>A</c>s but only one of them has a <c>B</c> nested
        /// type, the lookup will be considered ambiguous and null will be returned.
        /// </para>
        /// </remarks>
        /// <exception cref="Rewrite.RoslynRecipes.UnknownTypeException"></exception>
        public INamedTypeSymbol GetTypeByMetadataNameOrThrow(string fullyQualifiedMetadataName)
        {
            return compilation.GetTypeByMetadataName(fullyQualifiedMetadataName) ?? throw new UnknownTypeException(fullyQualifiedMetadataName);
        }
    }
}