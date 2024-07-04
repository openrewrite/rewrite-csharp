using Rewrite.Core;

namespace Rewrite.Test;

public interface SourceSpecs : IEnumerable<SourceSpec>;

public interface SourceSpecs<T> : SourceSpecs where T : SourceFile;