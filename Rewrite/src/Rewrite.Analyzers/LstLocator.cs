using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rewrite.Analyzers;

public class LstLocator : ISyntaxReceiver
{
    public Dictionary<string, LstInfo> LstClasses { get; } = new();
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is ClassDeclarationSyntax c && c.Identifier.Text == "Kind")
        {
            Console.WriteLine("");
        }
        if(syntaxNode is ClassDeclarationSyntax {Parent: TypeDeclarationSyntax parent} classDeclaration
           && (classDeclaration.BaseList?.Types.OfType<SimpleBaseTypeSyntax>().Any(x => x.ToString() is "Cs" or "J" or "Expression" or "Tree" or "Statement") ?? false)
           && classDeclaration.SyntaxTree.FilePath.EndsWith(".g.cs"))
        // if (syntaxNode is ClassDeclarationSyntax
        //     {
        //         BaseList.Types: [SimpleBaseTypeSyntax
        //         {
        //             Type: IdentifierNameSyntax
        //             {
        //                 Identifier.Text: "Cs" or "J" or "Expression" or "Tree" or "Statement"
        //             }
        //         }],
        //         Parent: TypeDeclarationSyntax parent
        //         // ,
        //         // Parent: InterfaceDeclarationSyntax
        //         // {
        //         //     Identifier.Text: "Cs" or "J" ,
        //         // } parent,
        //     } classDeclaration && classDeclaration.SyntaxTree.FilePath.EndsWith(".g.cs"))
        {
            LstClasses.Add($"{parent.Identifier.Text}.{classDeclaration.Identifier.Text}", new LstInfo(classDeclaration));

        }
    }
}

public class LstInfo(ClassDeclarationSyntax classDeclarationSyntax)
{
    public ClassDeclarationSyntax Class => classDeclarationSyntax;
    public string ClassName => classDeclarationSyntax.Identifier.Text;
    public TypeDeclarationSyntax OwningType => (TypeDeclarationSyntax)Class.Parent!;
    public string OwningInterfaceName => OwningType.Identifier.Text;
}
