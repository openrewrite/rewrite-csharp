namespace Rewrite.Core.Config;

#if Analyzer
internal
#else
[PublicAPI]
public 
#endif
class TagsAttribute(params string[] tags) : Attribute
{
    public string[] Tags { get; } = tags;
}