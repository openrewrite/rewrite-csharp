using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp.Template2;

[InterpolatedStringHandler]
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicConstructors)]
public struct InterpolatedTemplate
{
    internal const string ParameterPlaceholder = "__p";
    private readonly StringBuilder _template;
    int _placeholderCount = 0;
    // private Dictionary<string,Placeholder> _placeholders ;

    public InterpolatedTemplate(int literalLength, int formattedCount)
    {
        _template = new StringBuilder((ParameterPlaceholder.Length + 2) * formattedCount + literalLength);
        // _placeholders = new();
        // Placeholders = _placeholders.AsReadOnly();
    }

    // public IReadOnlyDictionary<string, Placeholder> Placeholders { get; }

    public void AppendLiteral(string? s)
    {
        if(s is not null)
            _template.Append(s);
    }

    public void AppendFormatted<T>(T t)
    {
        switch (t)
        {
            case Placeholder p:
                AddPlaceholder(p);
                return;

            case J j:
                AddPlaceholder(new Placeholder(j, null));
                return;

            default:
                AppendLiteral(t?.ToString());
                return;
        }
    }
    private void AddPlaceholder(Placeholder placeholder)
    {
        // var placeholderVariable = $"__p{_placeholderCount}";
        // _placeholders.Add(placeholderVariable, placeholder);
        _template.Append(placeholder.Node);
        _placeholderCount++;
    }
    // private void AddPlaceholder2(Placeholder placeholder)
    // {
    //     var placeholderVariable = $"__p{_placeholderCount}";
    //     _placeholders.Add(placeholderVariable, placeholder);
    //     _template.Append(placeholderVariable);
    //     _placeholderCount++;
    // }

    public override string ToString() => _template.ToString();
}