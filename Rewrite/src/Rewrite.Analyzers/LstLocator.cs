using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rewrite.Analyzers;

public class LstLocator : ISyntaxReceiver
{
    public Dictionary<FullyQualifiedName, TypeDeclarationSyntax> LstClasses { get; } = new();
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is ClassDeclarationSyntax { Identifier.Text: "Kind" })
        {
            Console.WriteLine("");
        }
        if(syntaxNode is ClassDeclarationSyntax {Parent: TypeDeclarationSyntax} classDeclaration
           && (classDeclaration.BaseList?.Types.OfType<SimpleBaseTypeSyntax>().Any(x => x.ToString() is "Cs" or "J" or "Expression" or "Tree" or "Statement") ?? false)
           && classDeclaration.SyntaxTree.FilePath.EndsWith(".g.cs"))
        {

            LstClasses.Add(GetFullyQualifiedName(classDeclaration), classDeclaration);

        }
    }

    FullyQualifiedName GetFullyQualifiedName(ClassDeclarationSyntax classDeclarationSyntax)
    {
        var fqn = new FullyQualifiedName();
        fqn.Name = classDeclarationSyntax.Identifier.Text;
        var parent = classDeclarationSyntax.Parent;
        var parents = new Stack<TypeDeclarationSyntax>();
        while (parent != null)
        {
            if (parent is TypeDeclarationSyntax typeDeclarationSyntax)
            {
                parents.Push(typeDeclarationSyntax);
            }
            else if (parent is BaseNamespaceDeclarationSyntax namespaceDeclaration)
            {
                fqn.Namespace = namespaceDeclaration.Name.ToString();
                break;
            }
            parent = parent.Parent;
        }

        fqn.ParentTypes = parents.ToList();
        return fqn;
    }
}

public class LstInfo(ClassDeclarationSyntax classDeclarationSyntax)
{
    public ClassDeclarationSyntax Class => classDeclarationSyntax;
    public string ClassName => classDeclarationSyntax.Identifier.Text;
    public TypeDeclarationSyntax OwningType => (TypeDeclarationSyntax)Class.Parent!;
    public string OwningInterfaceName => OwningType.Identifier.Text;
}
