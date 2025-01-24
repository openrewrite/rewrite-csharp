using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Rewrite.Core;
using Rewrite.RewriteCSharp.Internal;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.RewriteJava;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp.Format;

/// <summary>
/// Ensures that whitespace is on the outermost AST element possible.
/// </summary>
public class NormalizeFormatVisitor<P> : CSharpIsoVisitor<P>
{
    private readonly Core.Tree? stopAfter;

    [JsonConstructor]
    public NormalizeFormatVisitor(Core.Tree? stopAfter)
    {
        this.stopAfter = stopAfter;
    }

    public NormalizeFormatVisitor() : this(null) { }

    public override J.ClassDeclaration VisitClassDeclaration(J.ClassDeclaration classDecl, P p)
    {
        var c = base.VisitClassDeclaration(classDecl, p);

        if (c.LeadingAnnotations.Any())
        {
            c = ConcatenatePrefix(c, Space.FirstPrefix(c.LeadingAnnotations));
            c = c.WithLeadingAnnotations(Space.FormatFirstPrefix(c.LeadingAnnotations, Space.EMPTY));
            return c;
        }

        if (c.Modifiers.Any())
        {
            c = ConcatenatePrefix(c, Space.FirstPrefix(c.Modifiers));
            c = c.WithModifiers(Space.FormatFirstPrefix(c.Modifiers, Space.EMPTY));
            return c;
        }

        if (!c.Padding.DeclarationKind.Prefix.IsEmpty)
        {
            c = ConcatenatePrefix(c, c.Padding.DeclarationKind.Prefix);
            c = c.Padding.WithDeclarationKind(c.Padding.DeclarationKind.WithPrefix(Space.EMPTY));
            return c;
        }

        var typeParameters = c.Padding.TypeParameters;
        if (typeParameters != null && typeParameters.Elements.Any())
        {
            c = ConcatenatePrefix(c, typeParameters.Before);
            c = c.Padding.WithTypeParameters(typeParameters.WithBefore(Space.EMPTY));
            return c;
        }

        return c.WithName(c.Name.WithPrefix(c.Name.Prefix.WithWhitespace(" ")));
    }


    [SuppressMessage("ReSharper", "ConstantConditionWarning")]
    public override J.MethodDeclaration VisitMethodDeclaration(J.MethodDeclaration method, P p)
    {
        var m = base.VisitMethodDeclaration(method, p);

        if (m.LeadingAnnotations.Any())
        {
            m = ConcatenatePrefix(m, Space.FirstPrefix(m.LeadingAnnotations));
            m = m.WithLeadingAnnotations(Space.FormatFirstPrefix(m.LeadingAnnotations, Space.EMPTY));
            return m;
        }

        if (m.Modifiers.Any())
        {
            m = ConcatenatePrefix(m, Space.FirstPrefix(m.Modifiers));
            m = m.WithModifiers(Space.FormatFirstPrefix(m.Modifiers, Space.EMPTY));
            return m;
        }

        if (m.Annotations.TypeParameters != null)
        {
            if (m.Annotations.TypeParameters.Parameters.Any())
            {
                m = ConcatenatePrefix(m, m.Annotations.TypeParameters.Prefix);
                m = m.Annotations.WithTypeParameters(m.Annotations.TypeParameters!.WithPrefix(Space.EMPTY));
            }
            return m;
        }

        if (m.ReturnTypeExpression != null)
        {
            if (!string.IsNullOrEmpty(m.ReturnTypeExpression.Prefix.Whitespace))
            {
                m = ConcatenatePrefix(m, m.ReturnTypeExpression.Prefix);
                m = m.WithReturnTypeExpression(m.ReturnTypeExpression!.WithPrefix(Space.EMPTY) as TypeTree);
            }
            return m;
        }

        m = ConcatenatePrefix(m, m.Name.Prefix);
        m = m.WithName(m.Name.WithPrefix(Space.EMPTY));
        return m;
    }

    [SuppressMessage("ReSharper", "ConstantConditionWarning")]
    public override Cs.MethodDeclaration VisitMethodDeclaration(Cs.MethodDeclaration method, P p)
    {
        var m = base.VisitMethodDeclaration(method, p);

        if (m.Attributes.Any())
        {
            m = ConcatenatePrefix(m, Space.FirstPrefix(m.Attributes));
            m = m.WithAttributes(Space.FormatFirstPrefix(m.Attributes, Space.EMPTY));
            return m;
        }

        if (m.Modifiers.Any())
        {
            m = ConcatenatePrefix(m, Space.FirstPrefix(m.Modifiers));
            m = m.WithModifiers(Space.FormatFirstPrefix(m.Modifiers, Space.EMPTY));
            return m;
        }


        if (!string.IsNullOrEmpty(m.ReturnTypeExpression.Prefix.Whitespace))
        {
            m = ConcatenatePrefix(m, m.ReturnTypeExpression.Prefix);
            m = m.WithReturnTypeExpression((TypeTree)m.ReturnTypeExpression!.WithPrefix(Space.EMPTY));
        }
        return m;

    }

    [SuppressMessage("ReSharper", "ConstantConditionWarning")]
    public override J.VariableDeclarations VisitVariableDeclarations(J.VariableDeclarations multiVariable, P p)
    {
        var v = base.VisitVariableDeclarations(multiVariable, p);

        if (v.LeadingAnnotations.Any())
        {
            v = ConcatenatePrefix(v, Space.FirstPrefix(v.LeadingAnnotations));
            v = v.WithLeadingAnnotations(Space.FormatFirstPrefix(v.LeadingAnnotations, Space.EMPTY));
            return v;
        }

        if (v.Modifiers.Any())
        {
            v = ConcatenatePrefix(v, Space.FirstPrefix(v.Modifiers));
            v = v.WithModifiers(Space.FormatFirstPrefix(v.Modifiers, Space.EMPTY));
            return v;
        }

        if (v.TypeExpression != null)
        {
            v = ConcatenatePrefix(v, v.TypeExpression.Prefix);
            v = v.WithTypeExpression(v.TypeExpression!.WithPrefix(Space.EMPTY) as TypeTree);
            return v;
        }

        return v;
    }

    private J2 ConcatenatePrefix<J2>(J2 j, Space prefix) where J2 : J<J2>
    {
        string? shift = StringUtils.CommonMargin(null, j.Prefix.Whitespace);

        var comments = ListUtils.ConcatAll(
            j.Prefix.Comments,
            prefix.Comments.Map(comment =>
            {
                Comment c = comment;
                if (string.IsNullOrEmpty(shift))
                {
                    return c;
                }

                if (comment is TextComment textComment)
                {
                    c = textComment.WithText(textComment.Text.Replace("\n", "\n" + shift));
                }


                if (c.Suffix.Contains("\n"))
                {
                    c = c.WithSuffix(c.Suffix.Replace("\n", "\n" + shift));
                }

                return c;
            })
        );

        return j.WithPrefix(j.Prefix
            .WithWhitespace(j.Prefix.Whitespace + prefix.Whitespace)
            .WithComments(comments));
    }

    public override J? PostVisit(Core.Tree tree, P p)
    {
        if (stopAfter != null && stopAfter.IsScope(tree))
        {
            Cursor.PutMessageOnFirstEnclosing<JavaSourceFile>("stop", true);
        }
        return base.PostVisit(tree, p);
    }


    public override J? Visit(Core.Tree? tree, P p)
    {
        if (Cursor.GetNearestMessage<J>("stop") != null)
        {
            return (J)tree!;
        }
        return base.Visit(tree, p);
    }
}
