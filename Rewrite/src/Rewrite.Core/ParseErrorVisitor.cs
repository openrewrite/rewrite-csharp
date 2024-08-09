namespace Rewrite.Core;

public class ParseErrorVisitor<P> : TreeVisitor<Tree, P> {
    
    public override bool IsAcceptable(SourceFile sourceFile, P p) {
        return sourceFile is ParseError;
    }

    public virtual ParseError VisitParseError(ParseError e, P p) {
        return e.WithMarkers(VisitMarkers(e.Markers, p));
    }
}