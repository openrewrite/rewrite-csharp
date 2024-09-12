using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.RewriteCSharp.Marker;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava;
using Rewrite.RewriteJava.Marker;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp;

public class CSharpPrinter<P> : CSharpVisitor<PrintOutputCapture<P>>
{
    private readonly CSharpJavaPrinter _delegate;

    public CSharpPrinter()
    {
        _delegate = new CSharpJavaPrinter(this);
    }

    public override J? Visit(Core.Tree? tree, PrintOutputCapture<P> p) {
        if (!(tree is Cs)) {
            // re-route printing to the java printer
            return _delegate.Visit(tree, p);
        } else {
            return base.Visit(tree, p);
        }
    }

    public override Cs VisitCompilationUnit(Cs.CompilationUnit compilationUnit, PrintOutputCapture<P> p)
    {
        BeforeSyntax(compilationUnit, Space.Location.COMPILATION_UNIT_PREFIX, p);
        Visit(compilationUnit.Members, p);
        AfterSyntax(compilationUnit, p);
        VisitSpace(compilationUnit.Eof, Space.Location.COMPILATION_UNIT_EOF, p);
        return compilationUnit;
    }

    public override Cs VisitBlockScopeNamespaceDeclaration(Cs.BlockScopeNamespaceDeclaration namespaceDeclaration,
        PrintOutputCapture<P> p)
    {
        BeforeSyntax(namespaceDeclaration, CsSpace.Location.BLOCK_SCOPE_NAMESPACE_DECLARATION_PREFIX, p);
        p.Append("namespace");
        Visit(namespaceDeclaration.Name, p);
        Visit(namespaceDeclaration.Members, p);
        AfterSyntax(namespaceDeclaration, p);
        return namespaceDeclaration;
    }

    public override Cs VisitFileScopeNamespaceDeclaration(Cs.FileScopeNamespaceDeclaration namespaceDeclaration,
        PrintOutputCapture<P> p)
    {
        BeforeSyntax(namespaceDeclaration, CsSpace.Location.FILE_SCOPE_NAMESPACE_DECLARATION_PREFIX, p);
        p.Append("namespace");
        Visit(namespaceDeclaration.Name, p);
        Visit(namespaceDeclaration.Members, p);
        AfterSyntax(namespaceDeclaration, p);
        return namespaceDeclaration;
    }

    public override J VisitIdentifier(J.Identifier identifier, PrintOutputCapture<P> p)
    {
        BeforeSyntax(identifier, Space.Location.IDENTIFIER_PREFIX, p);
        p.Append(identifier.SimpleName);
        AfterSyntax(identifier, p);
        return identifier;
    }

    protected void VisitContainer<J2>(string? before, JContainer<J2>? container, CsContainer.Location loc,
        string suffixBetween, string? after,
        PrintOutputCapture<P> p) where J2 : J
    {
        if (container != null)
        {
            VisitSpace(container.Before, loc.BeforeLocation, p);
            p.Append(before);
            VisitRightPadded(container.Padding.Elements, loc.ElementLocation, suffixBetween, p);
            AfterSyntax(container.Markers, p);
            p.Append(after ?? "");
        }
    }

    protected void VisitRightPadded<J2>(IList<JRightPadded<J2>> nodes, CsRightPadded.Location location,
        string suffixBetween, PrintOutputCapture<P> p)
        where J2 : J
    {
        for (var i = 0; i < nodes.Count; ++i)
        {
            var node = nodes[i];
            Visit(node.Element, p);
            VisitSpace(node.After, location.AfterLocation, p);
            VisitMarkers(node.Markers, p);
            if (i < nodes.Count - 1)
            {
                p.Append(suffixBetween);
            }
        }
    }

    private static readonly Func<string, string> MARKER_WRAPPER =
        o => "/*~~" + o + (o.Length == 0 ? "" : "~~") + ">*/";

    protected void AfterSyntax(J j, PrintOutputCapture<P> p)
    {
        AfterSyntax(j.Markers, p);
    }

    public override Markers VisitMarkers(Markers? markers, PrintOutputCapture<P> p)
    {
        if (markers != null)
        {
            foreach (var marker in markers.MarkerList)
            {
                if (marker is Semicolon)
                    p.Append(";");
            }
        }

        return Markers.EMPTY;
    }

    //todo: AS: review if base method can be virtual and this one should just be override
    protected override Space VisitSpace(Space space, CsSpace.Location loc, PrintOutputCapture<P> p)
    {
        return VisitSpace(space, Space.Location.LANGUAGE_EXTENSION, p);
    }

    public override Space VisitSpace(Space? space, Space.Location? loc, PrintOutputCapture<P> p)
    {
        if (space == null)
            return Space.EMPTY;
        p.Append(space.Whitespace);
        foreach (var comment in space.Comments)
        {
            VisitMarkers(comment.Markers, p);
            comment.PrintComment(Cursor, p);
            p.Append(comment.Suffix);
        }

        return space;
    }

    private void BeforeSyntax(Cs cs, Space.Location loc, PrintOutputCapture<P> p)
    {
        BeforeSyntax(cs.Prefix, cs.Markers, loc, p);
    }

    private void BeforeSyntax(Cs cs, CsSpace.Location loc, PrintOutputCapture<P> p)
    {
        BeforeSyntax(cs.Prefix, cs.Markers, loc, p);
    }

    private void BeforeSyntax(Space prefix, Markers markers, CsSpace.Location? loc, PrintOutputCapture<P> p)
    {
        foreach (var marker in markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.BeforePrefix(marker, new Cursor(Cursor, marker), MARKER_WRAPPER));
        }

        if (loc != null)
        {
            VisitSpace(prefix, loc, p);
        }

        VisitMarkers(markers, p);
        foreach (var marker in markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.BeforeSyntax(marker, new Cursor(Cursor, marker), MARKER_WRAPPER));
        }
    }

    private void BeforeSyntax(J j, Space.Location loc, PrintOutputCapture<P> p)
    {
        BeforeSyntax(j.Prefix, j.Markers, loc, p);
    }

    private void BeforeSyntax(Space prefix, Markers markers, Space.Location loc, PrintOutputCapture<P> p)
    {
        foreach (var marker in markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.BeforePrefix(marker, new Cursor(Cursor, marker), MARKER_WRAPPER));
        }

        VisitSpace(prefix, loc, p);
        VisitMarkers(markers, p);
        foreach (var marker in markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.BeforeSyntax(marker, new Cursor(Cursor, marker), MARKER_WRAPPER));
        }
    }

    private void AfterSyntax(Cs cs, PrintOutputCapture<P> p)
    {
        AfterSyntax(cs.Markers, p);
    }

    private void AfterSyntax(Markers markers, PrintOutputCapture<P> p)
    {
        foreach (var marker in markers.MarkerList)
        {
            p.Append(p.MarkerPrinter.AfterSyntax(marker, new Cursor(Cursor, marker), MARKER_WRAPPER));
        }
    }

    class CSharpJavaPrinter(CSharpPrinter<P> cSharpPrinter) : JavaPrinter<P>
    {
        public override J? Visit(Core.Tree? tree, PrintOutputCapture<P> p) {
            if (tree is Cs) {
                return cSharpPrinter.Visit(tree, p);
            } else {
                return base.Visit(tree, p);
            }
        }

        public override J VisitBlock(J.Block block, PrintOutputCapture<P> p)
        {
            BeforeSyntax(block, Space.Location.BLOCK_PREFIX, p);

            if (block.Static)
            {
                p.Append("static");
                VisitRightPadded(block.Padding.Static, JRightPadded.Location.STATIC_INIT, p);
            }

            if (block.Markers.FindFirst<OmitBraces>() == null)
            {
                p.Append('{');
                VisitStatements(block.Padding.Statements, JRightPadded.Location.BLOCK_STATEMENT, p);
                VisitSpace(block.End, Space.Location.BLOCK_END, p);
                p.Append('}');
            }

            AfterSyntax(block, p);
            return block;
        }

        public override J VisitClassDeclaration(J.ClassDeclaration classDecl, PrintOutputCapture<P> p)
        {
            string kind = "";
            switch (classDecl.Padding.DeclarationKind.KindType)
            {
                case J.ClassDeclaration.Kind.Type.Class:
                    kind = "class";
                    break;
                case J.ClassDeclaration.Kind.Type.Enum:
                    kind = "enum";
                    break;
                case J.ClassDeclaration.Kind.Type.Interface:
                    kind = "interface";
                    break;
                case J.ClassDeclaration.Kind.Type.Annotation:
                    kind = "@interface";
                    break;
                case J.ClassDeclaration.Kind.Type.Record:
                    kind = "record";
                    break;
            }

            BeforeSyntax(classDecl, Space.Location.CLASS_DECLARATION_PREFIX, p);
            VisitSpace(Space.EMPTY, Space.Location.ANNOTATIONS, p);
            Visit(classDecl.LeadingAnnotations, p);
            foreach (J.Modifier m in classDecl.Modifiers)
            {
                VisitModifier(m, p);
            }
            Visit(classDecl.Padding.DeclarationKind.Annotations, p);
            VisitSpace(classDecl.Padding.DeclarationKind.Prefix, Space.Location.CLASS_KIND, p);
            p.Append(kind);
            Visit(classDecl.Name, p);
            VisitContainer("<", classDecl.Padding.TypeParameters, JContainer.Location.TYPE_PARAMETERS, ",", ">", p);
            VisitContainer("(", classDecl.Padding.PrimaryConstructor, JContainer.Location.RECORD_STATE_VECTOR, ",", ")", p);
            VisitLeftPadded(":", classDecl.Padding.Extends, JLeftPadded.Location.EXTENDS, p);
            VisitContainer(classDecl.Padding.Extends == null ? ":" : ",",
                classDecl.Padding.Implements, JContainer.Location.IMPLEMENTS, ",", null, p);
            VisitContainer("permits", classDecl.Padding.Permits, JContainer.Location.PERMITS, ",", null, p);
            Visit(classDecl.Body, p);
            AfterSyntax(classDecl, p);
            return classDecl;
        }
    }
}
