using System.Text;
using DiffPlex;
using DiffPlex.Chunkers;
using DiffPlex.DiffBuilder.Model;
using Socolin.ANSITerminalColor;

namespace Rewrite.CSharp.Tests;

public static class StringAssertionExtensions
{

    public static void ShouldBeSameAs(this string newText, string oldText)
    {
        var result = GetDifferences(oldText, newText);
        if (result != null)
        {
            Assert.Fail($"Expected string to be the same but are different\n{result}");
        }
    }
    public static string? GetDifferences(string oldText, string newText)
    {

        var differ = new Differ();
        var diff = DiffPlex.DiffBuilder.SideBySideDiffBuilder.Diff(differ, oldText, newText, wordChunker: new CharacterChunker());
        if (!diff.NewText.HasDifferences)
            return null;
        int padSize = diff.OldText.Lines.Count > 0 ? (int)Math.Log10(diff.OldText.Lines.Count) : 0;

        var result = new StringBuilder();

        for (int i = 0; i < diff.OldText.Lines.Count ; i++)
        {
            var leftLine = diff.OldText.Lines[i];
            var rightLine = diff.NewText.Lines[i];
            var lineChange = diff.NewText.Lines[i].Type;
            if (lineChange == ChangeType.Unchanged)
                continue;
            result.AppendLine("-------------------");
            string leftRender = AnsiColor.ColorizeText(leftLine.Text, AnsiColor.Background(Terminal256ColorCodes.BlackC0))!;
            string rightRender = AnsiColor.ColorizeText(rightLine.Text, AnsiColor.Background(Terminal256ColorCodes.BlackC0))!;
            if (lineChange == ChangeType.Modified)
            {
                var leftChanges = diff.OldText.Lines[i].SubPieces;
                var rightChanges = diff.NewText.Lines[i].SubPieces;
                leftRender = GetDiffText(leftChanges);
                rightRender = GetDiffText(rightChanges);
            }

            var lineNum = i + 1;
            // var right = GetDiffText);
            var leftLineNum = $"{lineNum.ToString()!.PadRight(padSize)}" ;
            var rightLineNum = leftLineNum;

            leftLineNum = AnsiColor.ColorizeText(leftLineNum, GetStyle(leftLine.Type)) + "|";
            rightLineNum = AnsiColor.ColorizeText(rightLineNum, GetStyle(rightLine.Type)) + "|";


            result.AppendLine($"{leftLineNum}{leftRender}");
            result.AppendLine($"{rightLineNum}{rightRender}");
        }

        return result.ToString();
    }

    static string GetDiffText(List<DiffPiece> current)
    {
        var sb = new StringBuilder();
        foreach (var charDelta in current)
        {
            var text = charDelta.Text;
            var style = AnsiColor.Composite(GetStyle(charDelta.Type),
                AnsiColor.Background(Terminal256ColorCodes.BlackC0));
            sb.Append(AnsiColor.ColorizeText(text, style));
        }

        return sb.ToString();
    }

    static AnsiColor GetStyle(ChangeType type)
    {
        return type switch
        {
            ChangeType.Deleted => AnsiColor.Foreground(Terminal256ColorCodes.Red1C196),
            ChangeType.Inserted => AnsiColor.Foreground(Terminal256ColorCodes.Green1C46),
            ChangeType.Modified => AnsiColor.Background(Terminal256ColorCodes.Blue1C21),
            ChangeType.Imaginary => AnsiColor.Composite(AnsiColor.Background(Terminal256ColorCodes.GreyC8), AnsiColor.Foreground(Terminal256ColorCodes.SeaGreen3C78)),
            _ => AnsiColor.Reset
        };
    }
}
