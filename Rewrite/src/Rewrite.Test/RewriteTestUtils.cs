using Rewrite.Core;

namespace Rewrite.Test;

internal static class RewriteTestUtils
{
    public static bool GroupSourceSpecsByParser(List<IParser.Builder> parserBuilders, Dictionary<IParser.Builder, List<SourceSpec>> sourceSpecsByParser, SourceSpec sourceSpec)
    {
        foreach (var entry in sourceSpecsByParser)
        {
            if (entry.Key.SourceFileType == sourceSpec.SourceFileType &&
                sourceSpec.Parser.GetType().IsInstanceOfType(entry.Key))
            {
                entry.Value.Add(sourceSpec);
                return true;
            }
        }

        foreach (var parser in parserBuilders)
        {
            if (parser.SourceFileType?.Equals(sourceSpec.SourceFileType) ?? false)
            {
                if (!sourceSpecsByParser.ContainsKey(parser))
                    sourceSpecsByParser[parser] = [];
                sourceSpecsByParser[parser].Add(sourceSpec);
                return true;
            }
        }

        return false;
    }
}
