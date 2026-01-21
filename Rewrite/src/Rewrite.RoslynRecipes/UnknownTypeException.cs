namespace Rewrite.RoslynRecipes;

public class UnknownTypeException(string fullyQualifiedTypeName) : Exception($"Type {fullyQualifiedTypeName} could not be resolved from compilation");