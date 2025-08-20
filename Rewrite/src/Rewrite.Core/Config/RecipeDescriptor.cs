
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
    public required string Id { get; init; }
    public required TypeName TypeName { get; init; }
    public required RecipeKind Kind { get; init; }
    public required string DisplayName { get; init; }
    public required string Description { get; init; }

    public IReadOnlyList<string> Tags { get; init; } = new List<string>();
    public TimeSpan? EstimatedEffortPerOccurrence { get; init; } = TimeSpan.FromMinutes(5);
    public IReadOnlyList<OptionDescriptor> Options { get; init; } = new List<OptionDescriptor>();
    public IReadOnlyList<RecipeDescriptor> RecipeList { get; init; } = new List<RecipeDescriptor>();
    
    public override string ToString() => $"[{Id}] {DisplayName}";

#if !Analyzer
    
#endif

}
#if Analyzer
internal
#else
public 
#endif 
    enum RecipeKind
{
    OpenRewrite,
    RoslynAnalyzer,
    RoslynFixer
}


