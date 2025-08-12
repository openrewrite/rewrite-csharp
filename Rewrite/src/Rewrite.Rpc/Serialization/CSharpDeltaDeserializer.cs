using System.Runtime.CompilerServices;
using Rewrite.Core;
using Rewrite.Core.Marker;
using Rewrite.RewriteCSharp;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava.Tree;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.Rpc.Serialization;

public class CSharpDeltaDeserializer : CSharpVisitor<DeserializationContext>
{
    // public override J? PreVisit(Tree? node, DeserializationContext p, 
    //     [CallerMemberName] string callingMethodName = "", 
    //     [CallerArgumentExpression(nameof(node))] string callingArgumentExpression = "")
    // {
    //     // This handles the common properties for all Tree nodes
    //     var ctx = p.As((J?)node);
    //     
    //     var id = ctx.DeserializeProperty(x => x?.Id) ?? (node?.Id ?? Tree.RandomId());
    //     var prefix = ctx.DeserializeProperty(x => x?.Prefix, Visit) ?? Space.EMPTY;
    //     var markers = ctx.DeserializeProperty(x => x?.Markers, (markers, context) => Visit(markers, context)) ?? Markers.EMPTY;
    //     
    //     // For ADD operations, we need to create a new instance
    //     // The specific tree type will be handled by the appropriate Visit method
    //     return (J?)node;
    // }
    
    public override J? VisitCompilationUnit(Cs.CompilationUnit? node, DeserializationContext p)
    {
        var ctx = p.As(node);
        var (id, prefix, markers) = DeserializeJHeader(node, p);
        
        var charsetBomMarked = ctx.DeserializeProperty(x => x.CharsetBomMarked);
        var charsetName = ctx.DeserializeProperty(x => x.CharsetName);
        var checksum = ctx.DeserializeProperty(x => x.Checksum);
        var sourcePath = ctx.DeserializeProperty(x => x.SourcePath)!;
        var externs = ctx.DeserializeList(x => x.Padding.Externs, (x, context) => Visit(x, context));
        var usings = ctx.DeserializeList(x => x.Padding.Usings, x => x.Element.Id, (x, context) => Visit(x, context));
        var attributeLists = ctx.DeserializeList(x => x.AttributeLists, (x, context) => (Cs.AttributeList?)Visit(x, context));
        var members = ctx.DeserializeList(x => x.Padding.Members, (x, context) => Visit(x, context));
        var eof = ctx.DeserializeProperty(x => x.Eof, (x, context) => Visit(x, context))!;
        
        return new Cs.CompilationUnit(
            id,
            prefix,
            markers,
            sourcePath,
            new FileAttributes(),
            charsetName,
            charsetBomMarked,
            checksum,
            externs,
            usings,
            attributeLists,
            members,
            eof
        );
    }
    
    public override J? VisitClassDeclaration(Cs.ClassDeclaration? node, DeserializationContext p)
    {
        var ctx = p.As(node);
        var (id, prefix, markers) = DeserializeJHeader(node, p);

        var attributeList = ctx.DeserializeList(x => x.AttributeList, (x, context) => (Cs.AttributeList?)Visit(x, context));
        var modifiers = ctx.DeserializeList(x => x.Modifiers, (x, context) => (J.Modifier?)Visit(x, context));
        var kind = ctx.DeserializeProperty(x => x.Padding.Kind, (x, context) => Visit(x, context))!;
        var name = ctx.DeserializeProperty(x => x.Name, (x, context) => (J.Identifier?)Visit(x, context))!;
        var typeParameters = ctx.DeserializeProperty(x => x.Padding.TypeParameters, (x, context) => Visit(x, context));
        var primaryConstructor = ctx.DeserializeProperty(x => x.Padding.PrimaryConstructor, (x, context) => Visit(x, context));
        var extendings = ctx.DeserializeProperty(x => x.Padding.Extendings, (x, context) => Visit(x, context));
        var implementings = ctx.DeserializeProperty(x => x.Padding.Implementings, (x, context) => Visit(x, context));
        var body = ctx.DeserializeProperty(x => x.Body, (x, context) => (J.Block?)Visit(x, context))!;
        var typeParameterConstraintClauses = ctx.DeserializeProperty(x => x.Padding.TypeParameterConstraintClauses, (x, context) => Visit(x, context));

        throw new NotImplementedException();
        // return new Cs.ClassDeclaration(
        //     id, 
        //     prefix, 
        //     markers, 
        //     attributeList, 
        //     modifiers,
        //     kind, 
        //     name, 
        //     typeParameters,
        //     primaryConstructor,
        //     extendings,
        //     implementings,
        //     body,
        //     typeParameterConstraintClauses,
        //     new J.ClassDeclaration.Kind.Type(J.ClassDeclaration.Kind.Type.Class)
        // );
    }

    public override J? VisitClassDeclarationKind(J.ClassDeclaration.Kind? node, DeserializationContext p)
    {
        var ctx = p.As(node);
        var (id, prefix, markers) = DeserializeJHeader(node, p);
        var type = ctx.DeserializeProperty(x => x.KindType)!;
        return new J.ClassDeclaration.Kind(id, prefix, markers, new List<J.Annotation>(), type);
    }

    private (Guid, Space, Markers) DeserializeJHeader(J? node, DeserializationContext p)
    {
        var ctx = p.As(node);
        var id = ctx.DeserializeProperty(x => x.Id);
        var prefix = ctx.DeserializeProperty(x => x.Prefix, Visit) ?? Space.EMPTY;
        var markers = ctx.DeserializeProperty(x => x.Markers, (markers, context) => Visit(markers, context)) ?? Markers.EMPTY;
        return (id, prefix, markers);
    }
    
    public override J? VisitModifier(J.Modifier? node, DeserializationContext p)
    {
        var ctx = p.As(node);
        
        var (id, prefix, markers) = DeserializeJHeader(node, p);
        
        var modifierType = ctx.DeserializeProperty(x => x?.ModifierType) ?? throw new InvalidOperationException("Modifier must have a type");
        var keyword = ctx.DeserializeProperty(x => x?.Keyword);
        
        return new J.Modifier(
            id,
            prefix,
            markers,
            keyword,
            modifierType,
            new List<J.Annotation>() // annotations - not shown in serializer
        );
    }
    
    public override J? VisitIdentifier(J.Identifier? node, DeserializationContext p)
    {
        var ctx = p.As(node);
        
        var (id, prefix, markers) = DeserializeJHeader(node, p);
        
        var simpleName = ctx.DeserializeProperty(x => x.SimpleName)!;
        
        return new J.Identifier(
            id,
            prefix,
            markers,
            new List<J.Annotation>(), // annotations - not shown in serializer
            simpleName,
            null, // type - not shown in serializer
            null  // fieldType - not shown in serializer
        );
    }
    
    // Helper visit methods that match the serializer
    private Space? Visit(Space? space, DeserializationContext context)
    {
        // if (space == null) return null;
        
        var ctx = context.As(space);
        var whitespace = ctx.DeserializeProperty(x => x.Whitespace) ;
        var comments = ctx.DeserializeList(x => x.Comments);
        
        return new Space(comments, whitespace);
    }
    
    public Markers? Visit(Markers? node, DeserializationContext context)
    {
        var ctx = context.As(node);
        var id = ctx.DeserializeProperty(x => x.Id);
        var markerList = ctx.DeserializeList(x => x.MarkerList);
        
        return new Markers(id, markerList);
    }
    
    public Marker? Visit(Marker? node, DeserializationContext context)
    {
        // The serializer doesn't show Marker implementation, 
        // so this is a placeholder that would need to be expanded
        return node;
    }
    
    public JLeftPadded<T>? Visit<T>(JLeftPadded<T>? node, DeserializationContext context) where T : class
    {
        var ctx = context.As(node);
        var before = ctx.DeserializeProperty(x => x.Before, (x, diffContext) => Visit(x, diffContext)) ?? Space.EMPTY;
        var element = ctx.DeserializeProperty(x => x.Element, (x, diffContext) => Visit((Tree?)x, diffContext));
        var markers = ctx.DeserializeProperty(x => x?.Markers, (x, diffContext) => Visit(x, diffContext)) ?? Markers.EMPTY;
        
        return new JLeftPadded<T>(before, (T)element!, markers);
    }
    
    public JContainer<T>? Visit<T>(JContainer<T>? node, DeserializationContext context) where T : Tree
    {
        var ctx = context.As(node);
        var before = ctx.DeserializeProperty(x => x.Before, (x, diffContext) => Visit(x, diffContext)) ?? Space.EMPTY;
        var elements = ctx.DeserializeList(x => x.Elements, x => x.Element.Id, (x, diffContext) => Visit(x, diffContext));
        var markers = ctx.DeserializeProperty(x => x?.Markers, (x, diffContext) => Visit(x, diffContext)) ?? Markers.EMPTY;
        
        return JContainer<T>.Build(before, elements, markers);
    }
    
    public JRightPadded<T>? Visit<T>(JRightPadded<T>? node, DeserializationContext context) where T : Tree
    {
        var ctx = context.As(node);
        var element = ctx.DeserializeProperty(x => x.Element, (x, diffContext) =>  Visit(x, diffContext)!);
        var after = ctx.DeserializeProperty(x => x.After, (x, diffContext) => Visit(x, diffContext)) ?? Space.EMPTY;
        var markers = ctx.DeserializeProperty(x => x?.Markers, (x, diffContext) => Visit(x, diffContext)) ?? Markers.EMPTY;
        
        return new JRightPadded<T>((T)element!, after, markers);
    }

    public override J? Visit(Tree? tree, DeserializationContext p, string callingMethodName = "", string callingArgumentExpression = "")
    {
        if (tree is null)
        {
            var type = Type.GetType(p.Current.ValueType!);

            if (type == typeof(Cs.ClassDeclaration))
            {
                return VisitClassDeclaration(null, p);
            }
            else if (type == typeof(Cs.Modifier))
            {
                return VisitModifier(null, p);
            }
        }
        return base.Visit(tree, p, callingMethodName, callingArgumentExpression);
    }

    public override Markers VisitMarkers(Markers? node, DeserializationContext p)
    {
        return Visit(node, p) ?? Markers.EMPTY;
    }
}