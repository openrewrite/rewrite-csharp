using Rewrite.Core.Marker;

namespace Rewrite.RewriteXml.Tree;

public static class Extensions
{
    public static Xml.Document WithEof(this Xml.Document document, string eof)
    {
        // FIXME implement based on Java code
        return eof == document.Eof ? document : new Xml.Document(document.Id, document.SourcePath, document.Prefix, document.Markers, document.CharsetName, document.CharsetBomMarked, document.Checksum, document.FileAttributes, document.Prolog, document.Root, eof);
    }

    public static Xml.Tag WithContent(this Xml.Tag tag, IList<Content>? content)
    {
        if (ReferenceEquals(tag.Content, content)) {
            return tag;
        }

        content ??= [];

        tag = new Xml.Tag(tag.Id, tag.Prefix, tag.Markers, tag.Name, tag.Attributes, content, tag.ClosingTag, tag.BeforeTagDelimiterPrefix);

        if (tag.ClosingTag == null) {
            if (tag.Content != null && tag.Content.Count > 0) {
                // TODO test this
                string indentedClosingTagPrefix = tag.Prefix.Substring(Math.Max(0, tag.Prefix.LastIndexOf('\n')));

                if (tag.Content[0] is Xml.CharData) {
                    return tag.WithClosingTag(new Xml.Tag.Closing(Core.Tree.RandomId(),
                        tag.Content[0].GetPrefix().Contains('\n') ?
                            indentedClosingTagPrefix : "",
                        Markers.EMPTY,
                        tag.Name, ""));
                } else {
                    return tag.WithClosingTag(new Xml.Tag.Closing(Core.Tree.RandomId(),
                        indentedClosingTagPrefix, Markers.EMPTY,
                        tag.Name, ""));
                }
            }
        }

        return tag;
    }

    public static Xml.Tag WithName(this Xml.Tag tag, string name)
    {
        return name == tag.Name ? tag : new Xml.Tag(tag.Id, tag.Prefix, tag.Markers, name, tag.Attributes, tag.Content, tag.ClosingTag?.WithName(name), tag.BeforeTagDelimiterPrefix);
    }

    public static Xml.JspDirective WithType(this Xml.JspDirective directive, string type)
    {
        // FIXME implement based on Java code
        return new Xml.JspDirective(directive.Id, directive.Prefix, directive.Markers, directive.BeforeTypePrefix, type, directive.Attributes, directive.BeforeDirectiveEndPrefix);
    }

    public static string GetPrefix(this Xml xml)
    {
        return xml switch
        {
            Xml.Document document => document.Prefix,
            Xml.Prolog prolog => prolog.Prefix,
            Xml.XmlDecl decl => decl.Prefix,
            Xml.ProcessingInstruction pi => pi.Prefix,
            Xml.Tag tag => tag.Prefix,
            Xml.Tag.Closing closing => closing.Prefix,
            Xml.Attribute attribute => attribute.Prefix,
            Xml.Attribute.Value value => value.Prefix,
            Xml.CharData charData => charData.Prefix,
            Xml.DocTypeDecl typeDecl => typeDecl.Prefix,
            Xml.DocTypeDecl.ExternalSubsets subsets => subsets.Prefix,
            Xml.Element element => element.Prefix,
            Xml.Ident ident => ident.Prefix,
            Xml.JspDirective directive => directive.Prefix,
            Xml.Comment comment => comment.Prefix,
            _ => throw new NotImplementedException()
        };
    }

    public static string GetPrefix(this Xml.Ident ident)
    {
        return ident.Prefix;
    }
}
