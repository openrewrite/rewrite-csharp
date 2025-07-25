using Newtonsoft.Json.Linq;

namespace Rewrite.Rpc;

public class VisitRequest
{
    public required string Visitor { get; set; }
    public JObject? VisitorOptions { get; set; }
    public required string TreeId { get; set; }
    /// <summary>
    /// An ID of the p value stored in the caller's local object cache.
    /// </summary>
    public required string P { get; set; }
    /// <summary>
    /// A list of IDs representing the cursor whose objects are stored in the caller's local object cache.
    /// </summary>
    public List<string>? Cursor { get; set; }
}