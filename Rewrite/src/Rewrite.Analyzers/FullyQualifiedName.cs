using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rewrite.Analyzers;

public class FullyQualifiedName
{
    protected bool Equals(FullyQualifiedName other)
    {
        return Namespace == other.Namespace && ParentTypes.Equals(other.ParentTypes) && Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((FullyQualifiedName)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Namespace.GetHashCode();
            foreach (var val in ParentTypes)
            {
                hashCode = (hashCode * 397) ^ val.GetHashCode();
            }
            hashCode = (hashCode * 397) ^ Name.GetHashCode();
            return hashCode;
        }
    }

    public string Namespace { get; set; } = "";
    public List<TypeDeclarationSyntax> ParentTypes { get; set; } = new ();
    public string Name { get; set; } = "";

    public override string ToString()
    {
        var parents = string.Join("", ParentTypes.Select(x => $"{x.Identifier.Text}+"));
        return $"{Namespace}.{parents}{Name}";
    }

}
