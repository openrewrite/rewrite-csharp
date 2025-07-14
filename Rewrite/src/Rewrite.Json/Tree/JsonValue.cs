using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteJson.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public partial interface JsonValue : Json
{
}
public partial interface JsonValue<out T>
{
}
