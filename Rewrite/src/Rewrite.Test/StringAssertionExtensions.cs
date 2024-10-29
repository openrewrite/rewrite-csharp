using System.Text;
using System.Text.RegularExpressions;
using DiffPlex;
using DiffPlex.Chunkers;
using DiffPlex.DiffBuilder.Model;
using Socolin.ANSITerminalColor;

namespace Rewrite.Test;

public static class StringAssertionExtensions
{

    public static void ShouldBeSameAs(this string? newText, string? oldText)
    {
        var result = GetDifferences(oldText ?? "", newText ?? "");
        if (result != null)
        {
            Assert.Fail($"Expected string to be the same but are different\n{result}");
        }
    }
    public static string? GetDifferences(string oldText, string newText)
    {
        string AddVisibleCrLf(string str) =>
            Regex.Replace(str.Replace("\r", "\u240D").Replace("\n", "\u2424"), "([\u240D\u2424]+)", "$1\n");// + (AnsiColor.ColorizeText("<-EOF", AnsiColor.Foreground(Terminal256ColorCodes.Yellow1C226)));
        oldText = AddVisibleCrLf(oldText);
        newText = AddVisibleCrLf(newText);

        var differ = new Differ();
        var diff = DiffPlex.DiffBuilder.SideBySideDiffBuilder.Diff(differ, oldText, newText, ignoreWhiteSpace: false, wordChunker: new CharacterChunker());
        if (!diff.NewText.HasDifferences)
            return null;
        int padSize = diff.OldText.Lines.Count > 0 ? (int)Math.Log10(diff.OldText.Lines.Count) + 1 : 1;

        var result = new StringBuilder();
        var oldLastLineIndex = diff.OldText.Lines.Last(x => x.Type != ChangeType.Imaginary).Position;
        var newLastLineIndex = diff.NewText.Lines.Last(x => x.Type != ChangeType.Imaginary).Position;
        for (int i = 0; i < diff.OldText.Lines.Count ; i++)
        {
            var oldLine = diff.OldText.Lines[i];
            var newLine = diff.NewText.Lines[i];
            var lineChange = diff.NewText.Lines[i].Type;
            if (lineChange == ChangeType.Unchanged)
                continue;
            result.AppendLine("-------------------");
            string oldRender = AnsiColor.ColorizeText(oldLine.Text, AnsiColor.Background(Terminal256ColorCodes.BlackC0))!;
            string newRender = AnsiColor.ColorizeText(newLine.Text, AnsiColor.Background(Terminal256ColorCodes.BlackC0))!;
            if (oldLine.Text != null && newLine.Text != null)
            {
                // check if this is a pure linebreak issue. if it is, collapse diffs for this line and next, into a single diff rendering (we just need to highlight the missing line break characters)
                var oldThisAndNextLine = diff.OldText.Lines.Count > i + 1 ? oldLine.Text + diff.OldText.Lines[i+1].Text : oldLine.Text;
                var newThisAndNextLine = diff.NewText.Lines.Count > i + 1 ? newLine.Text + diff.NewText.Lines[i+1].Text : newLine.Text;
                string StripWhitespace(string str) => Regex.Replace(str,"[\u240D\u2424\n]","");
                if(StripWhitespace(oldThisAndNextLine) == StripWhitespace(newThisAndNextLine))
                {
                    var twoLineDiff = DiffPlex.DiffBuilder.SideBySideDiffBuilder.Diff(differ, oldThisAndNextLine, newThisAndNextLine, ignoreWhiteSpace: false, wordChunker: new CharacterChunker());
                    oldLine = twoLineDiff.OldText.Lines[0];
                    newLine = twoLineDiff.NewText.Lines[0];
                    i++;
                }
            }


            if (lineChange == ChangeType.Modified)
            {
                oldRender = GetDiffText(oldLine.SubPieces);
                newRender = GetDiffText(newLine.SubPieces);
            }

            var oldLineNum = oldLine.Position?.ToString();
            var newLineNum = newLine.Position?.ToString();

            oldLineNum = $"{AnsiColor.ColorizeText(oldLineNum, GetStyle(oldLine.Type))}|".PadLeft(padSize);
            newLineNum = $"{AnsiColor.ColorizeText(newLineNum, GetStyle(newLine.Type))}|".PadLeft(padSize);

            // if whole line is different, then make every character take line colour
            if (oldLine.Type is not ChangeType.Modified)
            {
                oldRender = AnsiColor.ColorizeText(oldLine.Text, GetStyle(oldLine.Type))!;
            }
            if (newLine.Type is not ChangeType.Modified)
            {
                newRender = AnsiColor.ColorizeText(newLine.Text!, GetStyle(newLine.Type))!;
            }
            // append EOF character
            if (oldLine.Position == oldLastLineIndex)
            {
                oldRender += AnsiColor.ColorizeText("\u2403", AnsiColor.Foreground(Terminal256ColorCodes.YellowC11));
            }
            if (newLine.Position == oldLastLineIndex)
            {
                newRender += AnsiColor.ColorizeText("\u2403", AnsiColor.Foreground(Terminal256ColorCodes.YellowC11));
            }

            if (oldLine.Type is not ChangeType.Imaginary)
                result.AppendLine($"{oldLineNum}{oldRender}");

            if (newLine.Type is not ChangeType.Imaginary)
                result.AppendLine($"{newLineNum}{newRender}");
        }

        return result.ToString();
    }

    static string GetDiffText(List<DiffPiece> current)
    {
        var sb = new StringBuilder();
        foreach (var charDelta in current)
        {
            var text = charDelta.Text;
            if (charDelta.Type == ChangeType.Imaginary)
            {
                text = " ";
            }
            var style = GetStyle(charDelta.Type);
            sb.Append(AnsiColor.ColorizeText(text, style));
        }

        return sb.ToString();
    }

    static AnsiColor GetStyle(ChangeType type)
    {
        return type switch
        {
            ChangeType.Deleted => AnsiColor.Composite(AnsiColor.Foreground(Terminal256ColorCodes.Red1C196), AnsiColor.Background(Terminal256ColorCodes.BlackC0)),
            ChangeType.Inserted => AnsiColor.Composite(AnsiColor.Foreground(Terminal256ColorCodes.Green1C46), AnsiColor.Background(Terminal256ColorCodes.BlackC0)),
            ChangeType.Modified => AnsiColor.Composite(AnsiColor.Background(Terminal256ColorCodes.Blue1C21)),
            ChangeType.Imaginary => AnsiColor.Reset, // AnsiColor.Background(Terminal256ColorCodes.GreyC8),
            // ChangeType.Imaginary => AnsiColor.Composite(AnsiColor.Background(Terminal256ColorCodes.GreyC8), AnsiColor.Foreground(Terminal256ColorCodes.SeaGreen3C78)),
            _ => AnsiColor.Background(Terminal256ColorCodes.BlackC0)
        };
    }
}
