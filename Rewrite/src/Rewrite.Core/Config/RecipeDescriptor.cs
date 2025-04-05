using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;
using TypeName = string;
namespace Rewrite.Core.Config;

using System;
using System.Collections.Generic;


#if Analyzer
internal
#else
public 
#endif
record RecipeDescriptor
{
    // Properties with public getters and private setters to maintain immutability
    public required TypeName TypeName { get; set; }

    public required string DisplayName { get; init; }
    public required string Description { get; init; }

    public IReadOnlyList<string> Tags { get; init; } = new List<string>();
    public TimeSpan? EstimatedEffortPerOccurrence { get; init; } = TimeSpan.FromMinutes(5);
    public IReadOnlyList<OptionDescriptor> Options { get; init; } = new List<OptionDescriptor>();
    public IReadOnlyList<RecipeDescriptor> RecipeList { get; init; } = new List<RecipeDescriptor>();

    public override string ToString()
    {
        return $"[{TypeName}] {DisplayName}";
    }

#if !Analyzer
    public RecipeStartInfo CreateRecipeStartInfo()
    {
        var startInfo = new RecipeStartInfo(Options);
        return startInfo;
    }
#endif

}

