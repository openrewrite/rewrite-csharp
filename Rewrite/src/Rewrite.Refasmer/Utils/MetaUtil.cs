using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace Rewrite.Refasmer.Utils;

public static class MetaUtil
{
    public static string? GetFullname( this MetadataReader reader, EntityHandle entityHandle )
    {
        if (entityHandle.IsNil)
            return null;

        switch (entityHandle.Kind)
        {
            case HandleKind.TypeDefinition:
                var typeDef = reader.GetTypeDefinition((TypeDefinitionHandle) entityHandle);
                return $"{reader.GetString(typeDef.Namespace)}::{reader.GetString(typeDef.Name)}";
            case HandleKind.TypeReference:
                var typeRef = reader.GetTypeReference((TypeReferenceHandle) entityHandle);
                return $"{reader.GetString(typeRef.Namespace)}::{reader.GetString(typeRef.Name)}";
            default:
                return null;
        }
    }

    public static EntityHandle GetCustomAttrClass( this MetadataReader reader, CustomAttributeHandle attrHandle ) =>
        reader.GetCustomAttrClass(reader.GetCustomAttribute(attrHandle));

    public static EntityHandle GetCustomAttrClass( this MetadataReader reader, CustomAttribute attr ) =>
        reader.GetMethodClass(attr.Constructor);

    public static IEnumerable<TypeSpecificationHandle> TypeSpecifications( this MetadataReader reader )
    {
        for (var n = 1; n <= reader.GetTableRowCount(TableIndex.TypeSpec); n++)
            yield return MetadataTokens.TypeSpecificationHandle(n);
    }

    public static EntityHandle GetGenericType( this MetadataReader reader, TypeSpecificationHandle typeSpecHandle ) =>
        reader.GetGenericType(reader.GetTypeSpecification(typeSpecHandle));

    public static EntityHandle GetGenericType( this MetadataReader reader, TypeSpecification typeSpec )
    {
        var blobReader = reader.GetBlobReader(typeSpec.Signature);

        var typeCode = blobReader.ReadCompressedInteger();

        if (typeCode != (int) SignatureTypeCode.GenericTypeInstance)
            return default;

        typeCode = blobReader.ReadCompressedInteger();

        if (typeCode != (int) SignatureTypeKind.Class && typeCode != (int) SignatureTypeKind.ValueType)
            return default;

        return blobReader.ReadTypeHandle();
    }

    public static EntityHandle GetMethodClass( this MetadataReader reader, EntityHandle methodHandle )
    {
        switch (methodHandle.Kind)
        {
            case HandleKind.MemberReference:
                return reader.GetMemberReference((MemberReferenceHandle) methodHandle).Parent;
            case HandleKind.MethodDefinition:
                return reader.GetMethodDefinition((MethodDefinitionHandle) methodHandle).GetDeclaringType();
            default:
                return default;
        }
    }

    public static int? RowId(object definition) =>
        (int?)definition.GetType()
            .GetProperty("RowId", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
            ?.GetMethod?.Invoke(definition, Array.Empty<object>())
        ?? (int?)definition.GetType()
            .GetField("_rowId", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
            ?.GetValue(definition);
}
