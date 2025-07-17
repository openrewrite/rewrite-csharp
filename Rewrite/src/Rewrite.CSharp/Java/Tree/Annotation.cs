namespace Rewrite.RewriteJava.Tree;

partial interface J
{
    partial class Annotation
    {
        public JavaType? Type => AnnotationType.Type;
        public Annotation WithType(JavaType? type) => WithAnnotationType(AnnotationType.WithType(type));
        
        
        public String SimpleName 
        { 
            get 
            {
                if (AnnotationType is Identifier identifier) {
                    return identifier.SimpleName;
                } else if (AnnotationType is J.FieldAccess fieldAccess)
                {
                    return fieldAccess.SimpleName;
                }

                throw new InvalidOperationException("Unanticipated scenario");
            }
        }
    }
}
