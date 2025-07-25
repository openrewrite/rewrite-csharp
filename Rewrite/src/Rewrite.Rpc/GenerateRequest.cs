namespace Rewrite.Rpc;

public class GenerateRequest
{
    public required string Id { get; set; }
    /// <summary>
    /// An ID of the p value stored in the caller's local object cache.
    /// </summary>
    public required string P { get; set; }
}