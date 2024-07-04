using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using FileAttributes = Rewrite.Core.FileAttributes;

namespace Rewrite.RewriteYaml.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public interface YamlKey : Yaml
{
}
