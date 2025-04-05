namespace Rewrite.Core.Config;

#if Analyzer
internal
#else
[PublicAPI]
public 
#endif
class OptionDescriptor
{
    public required string Name { get; init; }
    public required string Type { get; init; }
    public string? DisplayName { get;init; }
    public string? Description { get;init; }
    public string? Example { get;init; }
    public bool Required { get;init; }

}