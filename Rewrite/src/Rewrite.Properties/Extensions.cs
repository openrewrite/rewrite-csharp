using Rewrite.RewriteProperties.Tree;

namespace Rewrite.RewriteProperties;

public static class DelimiterExtensions
{
    public static char GetCharacter(this Properties.Entry.Delimiter delimiter)
    {
        return delimiter switch
        {
            Properties.Entry.Delimiter.COLON => ':',
            Properties.Entry.Delimiter.EQUALS => '=',
            Properties.Entry.Delimiter.NONE => '\0',
            _ => throw new ArgumentException(delimiter.ToString())
        };
    }
    
    public static Properties.Entry.Delimiter GetDelimiter(string value) {
        return "=".Equals(value.Trim()) ? Properties.Entry.Delimiter.EQUALS :
            ":".Equals(value.Trim()) ? Properties.Entry.Delimiter.COLON :
            "".Equals(value.Trim()) ? Properties.Entry.Delimiter.NONE :
            Properties.Entry.Delimiter.EQUALS;
    }

    public static char GetCharacter(this Properties.Comment.Delimiter delimiter)
    {
        return delimiter switch
        {
            Properties.Comment.Delimiter.HASH_TAG => '#',
            Properties.Comment.Delimiter.EXCLAMATION_MARK => '!',
            _ => throw new ArgumentException(delimiter.ToString())
        };
    }
    
}
