using System.Reflection.Metadata;

namespace Rewrite.Refasmer.Filters;

public interface IImportFilter
{
    public bool OmitNonApiMembers { get; }
        
    bool AllowImport(TypeDefinition type, MetadataReader reader);
    bool AllowImport( MethodDefinition method, MetadataReader reader );
    bool AllowImport( FieldDefinition field, MetadataReader reader );
}