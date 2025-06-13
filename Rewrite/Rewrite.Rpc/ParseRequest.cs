namespace Rewrite.Rpc;

public class ParseRequest
{
    public required string Parser { get; set; }
    public required List<Input> Inputs { get; set; }
    public string? RelativeTo { get; set; }


    public class Input
    {
        public string? Text { get; set; }
        public required string SourcePath { get; set; }
    }
}