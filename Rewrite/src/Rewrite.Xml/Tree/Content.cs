using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteXml.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public interface Content : Xml
{
}
