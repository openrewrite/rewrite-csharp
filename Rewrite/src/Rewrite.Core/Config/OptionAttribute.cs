namespace Rewrite.Core;

[AttributeUsage(AttributeTargets.Parameter)]
public sealed class OptionAttribute([LanguageInjection("markdown")] string displayName = "", [LanguageInjection("markdown")] string description = "", string example = "", params string[] valid) : Attribute
{
    public string DisplayName => displayName;

    public string Description => description;

    public string Example => example;

    public string[] Valid => valid;
}
