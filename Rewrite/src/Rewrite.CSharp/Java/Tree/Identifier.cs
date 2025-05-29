namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class Identifier
    {
        public static bool operator ==(J.Identifier? left, string? right)
        {
            if (ReferenceEquals(left, null)) return right == null;
            return left.SimpleName == right;
        }
        public static bool operator !=(J.Identifier? left, string? right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj)
        {
            if (obj is J.Identifier otherIdentifier)
                return this.Equals(otherIdentifier);
            if (obj is string str)
            {
                return this == str;
            }

            return base.Equals(obj);
        }

        public List<Identifier> Names => [this];
    }
}
