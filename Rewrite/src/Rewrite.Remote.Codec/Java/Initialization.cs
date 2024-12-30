using System.Formats.Cbor;
using Rewrite.RewriteJava;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.Remote.Codec.Java;

public static class Initialization
{
    public static void Initialize()
    {
        SenderContext.Register(typeof(J), () => new JavaSender());
        ReceiverContext.Register(typeof(J), () => new JavaReceiver());

        RegisterValueSerializers();
        RegisterValueDeserializers();
    }

    private static void RegisterValueDeserializers()
    {
        RemotingContext.RegisterValueDeserializer<JavaType.Primitive>((type, reader, context) =>
        {
            var kind = (JavaType.Primitive.PrimitiveType)reader.ReadInt64();
            reader.ReadEndArray();
            return new JavaType.Primitive(kind);
        });
        RemotingContext.RegisterValueDeserializer<JavaType.Unknown>((type, reader, context) =>
        {
            var unknown = JavaType.Unknown.Instance;
            while (reader.PeekState() != CborReaderState.EndMap)
            {
                var name = reader.ReadTextString();
                switch (name)
                {
                    case "@ref":
                        context.RemotingContext.Add(reader.ReadInt64(), unknown);
                        break;
                    case "@c":
                        reader.ReadTextString();
                        break;
                    default:
                        throw new NotImplementedException(name);
                }
            }

            reader.ReadEndMap();
            return unknown;
        });
        RemotingContext.RegisterValueDeserializer<JavaType.Method>((type, reader, context) =>
        {
            var method = new JavaType.Method();

            while (reader.PeekState() != CborReaderState.EndMap)
            {
                var prop = reader.ReadTextString();
                switch (prop)
                {
                    case "@ref":
                        context.RemotingContext.Add(reader.ReadInt64(), method);
                        break;
                    case "@c":
                        reader.ReadTextString();
                        break;
                    case "flagsBitMap":
                        method.FlagsBitMap = reader.ReadInt64();
                        break;
                    case "declaringType":
                        method.DeclaringType = context.Deserialize<JavaType.FullyQualified>(reader)!;
                        break;
                    case "name":
                        method.Name = reader.ReadTextString();
                        break;
                    case "returnType":
                        method.ReturnType = context.Deserialize<JavaType>(reader)!;
                        break;
                    case "parameterNames":
                        reader.ReadStartArray();
                        IList<string> parameterNames = [];
                        while (reader.PeekState() != CborReaderState.EndArray)
                        {
                            parameterNames.Add(reader.ReadTextString());
                        }

                        reader.ReadEndArray();
                        method.ParameterNames = parameterNames;
                        break;
                    case "parameterTypes":
                        reader.ReadStartArray();
                        IList<JavaType> parameterTypes = [];
                        while (reader.PeekState() != CborReaderState.EndArray)
                        {
                            parameterTypes.Add(context.Deserialize<JavaType>(reader)!);
                        }

                        reader.ReadEndArray();
                        method.ParameterTypes = parameterTypes;
                        break;
                    case "thrownExceptions":
                        reader.ReadStartArray();
                        IList<JavaType> exceptions = [];
                        while (reader.PeekState() != CborReaderState.EndArray)
                        {
                            exceptions.Add(context.Deserialize<JavaType.FullyQualified>(reader)!);
                        }

                        reader.ReadEndArray();
                        method.ThrownExceptions = exceptions;
                        break;
                    case "annotations":
                        reader.ReadStartArray();
                        List<JavaType.FullyQualified> annotations = [];
                        while (reader.PeekState() != CborReaderState.EndArray)
                        {
                            annotations.Add(context.Deserialize<JavaType.FullyQualified>(reader)!);
                        }

                        reader.ReadEndArray();
                        method.Annotations = annotations;
                        break;
                    case "defaultValue":
                        reader.ReadStartArray();
                        IList<string> defaultValue = [];
                        while (reader.PeekState() != CborReaderState.EndArray)
                        {
                            defaultValue.Add(reader.ReadTextString());
                        }

                        reader.ReadEndArray();
                        method.DefaultValue = defaultValue;
                        break;
                    case "declaredFormalTypeNames":
                        reader.ReadStartArray();
                        IList<string> declaredFormalTypeNames = [];
                        while (reader.PeekState() != CborReaderState.EndArray)
                        {
                            declaredFormalTypeNames.Add(reader.ReadTextString());
                        }

                        reader.ReadEndArray();
                        method.DeclaredFormalTypeNames = declaredFormalTypeNames;
                        break;
                    default:
                        throw new NotImplementedException(prop);
                }
            }

            reader.ReadEndMap();
            return method;
        });
        RemotingContext.RegisterValueDeserializer<JavaType.Variable>((type, reader, context) =>
        {
            var variable = new JavaType.Variable();

            while (reader.PeekState() != CborReaderState.EndMap)
            {
                var prop = reader.ReadTextString();
                switch (prop)
                {
                    case "@ref":
                        context.RemotingContext.Add(reader.ReadInt64(), variable);
                        break;
                    case "@c":
                        reader.ReadTextString();
                        break;
                    case "flagsBitMap":
                        variable.FlagsBitMap = reader.ReadInt64();
                        break;
                    case "name":
                        variable.Name = reader.ReadTextString();
                        break;
                    case "owner":
                        variable.Owner = context.Deserialize<JavaType>(reader);
                        break;
                    case "type":
                        variable.Type = context.Deserialize<JavaType>(reader)!;
                        break;
                    case "annotations":
                        var capacity = reader.ReadStartArray();
                        var annotations = new List<JavaType.FullyQualified>(capacity ?? 0);
                        while (reader.PeekState() != CborReaderState.EndArray)
                        {
                            annotations.Add(context.Deserialize<JavaType.FullyQualified>(reader)!);
                        }

                        reader.ReadEndArray();
                        variable.Annotations = annotations;
                        break;
                    default:
                        throw new NotImplementedException(prop);
                }
            }

            reader.ReadEndMap();
            return variable;
        });
        RemotingContext.RegisterValueDeserializer<JavaType.Array>((type, reader, context) =>
        {
            var array = new JavaType.Array();

            while (reader.PeekState() != CborReaderState.EndMap)
            {
                var prop = reader.ReadTextString();
                switch (prop)
                {
                    case "@ref":
                        context.RemotingContext.Add(reader.ReadInt64(), array);
                        break;
                    case "@c":
                        reader.ReadTextString();
                        break;
                    case "elemType":
                        array.ElementType = context.Deserialize<JavaType>(reader)!;
                        break;
                    case "annotations":
                        reader.ReadStartArray();
                        List<JavaType.FullyQualified> annotations = [];
                        while (reader.PeekState() != CborReaderState.EndArray)
                        {
                            annotations.Add(context.Deserialize<JavaType.FullyQualified>(reader)!);
                        }

                        reader.ReadEndArray();
                        array.Annotations = annotations;
                        break;
                    default:
                        throw new NotImplementedException(prop);
                }
            }

            reader.ReadEndMap();
            return array;
        });
        RemotingContext.RegisterValueDeserializer<JavaType.Parameterized>((type, reader, context) =>
        {
            var parameterized = new JavaType.Parameterized();

            while (reader.PeekState() != CborReaderState.EndMap)
            {
                var prop = reader.ReadTextString();
                switch (prop)
                {
                    case "@ref":
                        context.RemotingContext.Add(reader.ReadInt64(), parameterized);
                        break;
                    case "@c":
                        reader.ReadTextString();
                        break;
                    case "type":
                        parameterized.Type = context.Deserialize<JavaType.FullyQualified>(reader);
                        break;
                    case "typeParameters":
                        reader.ReadStartArray();
                        List<JavaType> typeParameters = [];
                        while (reader.PeekState() != CborReaderState.EndArray)
                        {
                            typeParameters.Add(context.Deserialize<JavaType>(reader)!);
                        }

                        reader.ReadEndArray();
                        parameterized.TypeParameters = typeParameters;
                        break;
                    case "annotations":
                        reader.ReadStartArray();
                        List<JavaType.FullyQualified> annotations = [];
                        while (reader.PeekState() != CborReaderState.EndArray)
                        {
                            annotations.Add(context.Deserialize<JavaType.FullyQualified>(reader)!);
                        }

                        reader.ReadEndArray();
                        parameterized.Annotations = annotations;
                        break;
                    default:
                        throw new NotImplementedException(prop);
                }
            }

            reader.ReadEndMap();
            return parameterized;
        });
        RemotingContext.RegisterValueDeserializer<JavaType.GenericTypeVariable>((type, reader, context) =>
        {
            var typeVariable = new JavaType.GenericTypeVariable();

            while (reader.PeekState() != CborReaderState.EndMap)
            {
                var prop = reader.ReadTextString();
                switch (prop)
                {
                    case "@ref":
                        context.RemotingContext.Add(reader.ReadInt64(), typeVariable);
                        break;
                    case "@c":
                        reader.ReadTextString();
                        break;
                    case "name":
                        typeVariable.Name = reader.ReadTextString();
                        break;
                    case "variance":
                        typeVariable.Variance = (JavaType.GenericTypeVariable.VarianceType)reader.ReadInt64();
                        break;
                    case "bounds":
                        reader.ReadStartArray();
                        List<JavaType> bounds = [];
                        while (reader.PeekState() != CborReaderState.EndArray)
                        {
                            bounds.Add(context.Deserialize<JavaType>(reader)!);
                        }

                        reader.ReadEndArray();
                        typeVariable.Bounds = bounds;
                        break;
                    default:
                        throw new NotImplementedException(prop);
                }
            }

            reader.ReadEndMap();
            return typeVariable;
        });
        RemotingContext.RegisterValueDeserializer<JavaType.Class>((type, reader, context) =>
        {
            var cls = type == typeof(JavaType.ShallowClass) ? new JavaType.ShallowClass() : new JavaType.Class();

            while (reader.PeekState() != CborReaderState.EndMap)
            {
                var name = reader.ReadTextString();
                switch (name)
                {
                    case "@ref":
                        context.RemotingContext.Add(reader.ReadInt64(), cls);
                        break;
                    case "@c":
                        reader.ReadTextString();
                        break;
                    case "flagsBitMap":
                        cls.FlagsBitMap = reader.ReadInt64();
                        break;
                    case "fullyQualifiedName":
                        cls.FullyQualifiedName = reader.ReadTextString();
                        break;
                    case "kind":
                        cls.Kind = context.Deserialize<JavaType.FullyQualified.TypeKind>(reader);
                        break;
                    case "typeParameters":
                        reader.ReadStartArray();
                        List<JavaType> typeParameters = [];
                        while (reader.PeekState() != CborReaderState.EndArray)
                        {
                            typeParameters.Add(context.Deserialize<JavaType>(reader)!);
                        }

                        reader.ReadEndArray();
                        cls.TypeParameters = typeParameters;
                        break;
                    case "supertype":
                        cls.Supertype = context.Deserialize<JavaType.FullyQualified>(reader);
                        break;
                    case "owningClass":
                        cls.OwningClass = context.Deserialize<JavaType.FullyQualified>(reader);
                        break;
                    case "annotations":
                        reader.ReadStartArray();
                        List<JavaType.FullyQualified> annotations = [];
                        while (reader.PeekState() != CborReaderState.EndArray)
                        {
                            annotations.Add(context.Deserialize<JavaType.FullyQualified>(reader)!);
                        }

                        reader.ReadEndArray();
                        cls.Annotations = annotations;
                        break;
                    case "interfaces":
                        reader.ReadStartArray();
                        List<JavaType.FullyQualified> interfaces = [];
                        while (reader.PeekState() != CborReaderState.EndArray)
                        {
                            interfaces.Add(context.Deserialize<JavaType.FullyQualified>(reader)!);
                        }

                        reader.ReadEndArray();
                        cls.Interfaces = interfaces;
                        break;
                    case "members":
                        reader.ReadStartArray();
                        List<JavaType.Variable> members = [];
                        while (reader.PeekState() != CborReaderState.EndArray)
                        {
                            members.Add(context.Deserialize<JavaType.Variable>(reader)!);
                        }

                        reader.ReadEndArray();
                        cls.Members = members;
                        break;
                    case "methods":
                        reader.ReadStartArray();
                        List<JavaType.Method> methods = [];
                        while (reader.PeekState() != CborReaderState.EndArray)
                        {
                            methods.Add(context.Deserialize<JavaType.Method>(reader)!);
                        }

                        reader.ReadEndArray();
                        cls.Methods = methods;
                        break;
                    default:
                        throw new NotImplementedException(name);
                }
            }

            reader.ReadEndMap();
            return cls;
        });
    }

    private static void RegisterValueSerializers()
    {
        RemotingContext.RegisterValueSerializer<JavaType.Primitive>((value, type, writer, context) =>
        {
            writer.WriteStartArray(2);
            writer.WriteTextString("org.openrewrite.java.tree.JavaType$Primitive");
            writer.WriteInt64((long)value.Kind);
            writer.WriteEndArray();
        });
        RemotingContext.RegisterValueSerializer<JavaType.Unknown>((value, type, writer, context) =>
        {
            if (context.RemotingContext.TryGetId(value, out var id))
            {
                writer.WriteInt64(id);
                return;
            }

            writer.WriteStartMap(null);
            writer.WriteTextString("@c");
            writer.WriteTextString("org.openrewrite.java.tree.JavaType$Unknown");
            writer.WriteTextString("@ref");
            writer.WriteInt64(context.RemotingContext.Add(value));
            writer.WriteEndMap();
        });
        RemotingContext.RegisterValueSerializer<JavaType.Method>((value, type, writer, context) =>
        {
            if (context.RemotingContext.TryGetId(value, out var id))
            {
                writer.WriteInt64(id);
                return;
            }

            writer.WriteStartMap(null);
            writer.WriteTextString("@c");
            writer.WriteTextString("org.openrewrite.java.tree.JavaType$Method");
            writer.WriteTextString("@ref");
            writer.WriteInt64(context.RemotingContext.Add(value));
            writer.WriteTextString("flagsBitMap");
            writer.WriteInt64(value.FlagsBitMap);
            writer.WriteTextString("declaringType");
            context.Serialize(value.DeclaringType, null, writer);
            writer.WriteTextString("name");
            writer.WriteTextString(value.Name);
            writer.WriteTextString("returnType");
            context.Serialize(value.ReturnType, null, writer);
            if (value.ParameterNames.Count > 0)
            {
                writer.WriteTextString("parameterNames");
                writer.WriteStartArray(value.ParameterNames.Count);
                foreach (var parameterName in value.ParameterNames)
                    writer.WriteTextString(parameterName);
                writer.WriteEndArray();
            }

            if (value.ParameterTypes.Count > 0)
            {
                writer.WriteTextString("parameterTypes");
                writer.WriteStartArray(value.ParameterTypes.Count);
                foreach (var parameterType in value.ParameterTypes)
                    context.Serialize(parameterType, null, writer);
                writer.WriteEndArray();
            }

            if (value.ThrownExceptions.Count > 0)
            {
                writer.WriteTextString("thrownExceptions");
                writer.WriteStartArray(value.ThrownExceptions.Count);
                foreach (var thrownException in value.ThrownExceptions)
                    context.Serialize(thrownException, null, writer);
                writer.WriteEndArray();
            }

            if (value.Annotations.Count > 0)
            {
                writer.WriteTextString("annotations");
                writer.WriteStartArray(value.Annotations.Count);
                foreach (var annotation in value.Annotations)
                    context.Serialize(annotation, null, writer);
                writer.WriteEndArray();
            }

            if (value.DefaultValue != null)
            {
                writer.WriteTextString("defaultValue");
                writer.WriteStartArray(value.DefaultValue.Count);
                foreach (var defaultValue in value.DefaultValue)
                    writer.WriteTextString(defaultValue);
                writer.WriteEndArray();
            }

            if (value.DeclaredFormalTypeNames != null)
            {
                writer.WriteTextString("declaredFormalTypeNames");
                writer.WriteStartArray(value.DeclaredFormalTypeNames.Count);
                foreach (var defaultValue in value.DeclaredFormalTypeNames)
                    writer.WriteTextString(defaultValue);
                writer.WriteEndArray();
            }

            writer.WriteEndMap();
        });
        RemotingContext.RegisterValueSerializer<JavaType.Variable>((value, type, writer, context) =>
        {
            if (context.RemotingContext.TryGetId(value, out var id))
            {
                writer.WriteInt64(id);
                return;
            }

            writer.WriteStartMap(null);
            writer.WriteTextString("@c");
            writer.WriteTextString("org.openrewrite.java.tree.JavaType$Variable");
            writer.WriteTextString("@ref");
            writer.WriteInt64(context.RemotingContext.Add(value));
            writer.WriteTextString("flagsBitMap");
            writer.WriteInt64(value.FlagsBitMap);
            writer.WriteTextString("name");
            writer.WriteTextString(value.Name);
            writer.WriteTextString("owner");
            context.Serialize(value.Owner, null, writer);
            writer.WriteTextString("type");
            context.Serialize(value.Type, null, writer);

            if (value.Annotations.Count > 0)
            {
                writer.WriteTextString("annotations");
                writer.WriteStartArray(value.Annotations.Count);
                foreach (var annotation in value.Annotations)
                    context.Serialize(annotation, null, writer);
                writer.WriteEndArray();
            }

            writer.WriteEndMap();
        });
        RemotingContext.RegisterValueSerializer<JavaType.Array>((value, type, writer, context) =>
        {
            if (context.RemotingContext.TryGetId(value, out var id))
            {
                writer.WriteInt64(id);
                return;
            }

            writer.WriteStartMap(null);
            writer.WriteTextString("@c");
            writer.WriteTextString("org.openrewrite.java.tree.JavaType$Array");
            writer.WriteTextString("@ref");
            writer.WriteInt64(context.RemotingContext.Add(value));
            writer.WriteTextString("elemType");
            context.Serialize(value.ElementType, null, writer);

            if (value.Annotations.Count > 0)
            {
                writer.WriteTextString("annotations");
                writer.WriteStartArray(value.Annotations.Count);
                foreach (var annotation in value.Annotations)
                    context.Serialize(annotation, null, writer);
                writer.WriteEndArray();
            }

            writer.WriteEndMap();
        });
        RemotingContext.RegisterValueSerializer<JavaType.Parameterized>((value, type, writer, context) =>
        {
            if (context.RemotingContext.TryGetId(value, out var id))
            {
                writer.WriteInt64(id);
                return;
            }

            writer.WriteStartMap(null);
            writer.WriteTextString("@c");
            writer.WriteTextString("org.openrewrite.java.tree.JavaType$Parameterized");
            writer.WriteTextString("@ref");
            writer.WriteInt64(context.RemotingContext.Add(value));
            writer.WriteTextString("type");
            context.Serialize(value.Type, null, writer);

            if (value.TypeParameters.Count > 0)
            {
                writer.WriteTextString("typeParameters");
                writer.WriteStartArray(value.TypeParameters.Count);
                foreach (var typeParameter in value.TypeParameters)
                    context.Serialize(typeParameter, null, writer);
                writer.WriteEndArray();
            }

            if (value.Annotations.Count > 0)
            {
                writer.WriteTextString("annotations");
                writer.WriteStartArray(value.Annotations.Count);
                foreach (var annotation in value.Annotations)
                    context.Serialize(annotation, null, writer);
                writer.WriteEndArray();
            }

            writer.WriteEndMap();
        });
        RemotingContext.RegisterValueSerializer<JavaType.GenericTypeVariable>((value, type, writer, context) =>
        {
            if (context.RemotingContext.TryGetId(value, out var id))
            {
                writer.WriteInt64(id);
                return;
            }

            writer.WriteStartMap(null);
            writer.WriteTextString("@c");
            writer.WriteTextString("org.openrewrite.java.tree.JavaType$GenericTypeVariable");
            writer.WriteTextString("@ref");
            writer.WriteInt64(context.RemotingContext.Add(value));
            writer.WriteTextString("name");
            writer.WriteTextString(value.Name);
            writer.WriteTextString("variance");
            writer.WriteInt32((int)value.Variance);

            if (value.Bounds.Count > 0)
            {
                writer.WriteTextString("bounds");
                writer.WriteStartArray(value.Bounds.Count);
                foreach (var bound in value.Bounds)
                    context.Serialize(bound, null, writer);
                writer.WriteEndArray();
            }

            writer.WriteEndMap();
        });
        RemotingContext.RegisterValueSerializer<JavaType.Class>((value, type, writer, context) =>
        {
            if (context.RemotingContext.TryGetId(value, out var id))
            {
                writer.WriteInt64(id);
                return;
            }

            writer.WriteStartMap(null);
            writer.WriteTextString("@c");
            writer.WriteTextString(value is JavaType.ShallowClass
                ? "org.openrewrite.java.tree.JavaType$ShallowClass"
                : "org.openrewrite.java.tree.JavaType$Class");
            writer.WriteTextString("@ref");
            writer.WriteInt64(context.RemotingContext.Add(value));
            writer.WriteTextString("flagsBitMap");
            writer.WriteInt64(value.FlagsBitMap);
            writer.WriteTextString("fullyQualifiedName");
            writer.WriteTextString(value.FullyQualifiedName);
            writer.WriteTextString("kind");
            writer.WriteInt32((int)value.Kind);

            if (value.TypeParameters.Count > 0)
            {
                writer.WriteTextString("typeParameters");
                writer.WriteStartArray(value.TypeParameters.Count);
                foreach (var typeParameter in value.TypeParameters)
                    context.Serialize(typeParameter, null, writer);
                writer.WriteEndArray();
            }

            if (value.Supertype != null)
            {
                writer.WriteTextString("supertype");
                context.Serialize(value.Supertype, null, writer);
            }

            if (value.Supertype != null)
            {
                writer.WriteTextString("owningClass");
                context.Serialize(value.OwningClass, null, writer);
            }

            if (value.Annotations.Count > 0)
            {
                writer.WriteTextString("annotations");
                writer.WriteStartArray(value.Annotations.Count);
                foreach (var annotation in value.Annotations)
                    context.Serialize(annotation, null, writer);
                writer.WriteEndArray();
            }

            if (value.Interfaces.Count > 0)
            {
                writer.WriteTextString("interfaces");
                writer.WriteStartArray(value.Interfaces.Count);
                foreach (var iface in value.Interfaces)
                    context.Serialize(iface, null, writer);
                writer.WriteEndArray();
            }

            if (value.Members.Count > 0)
            {
                writer.WriteTextString("members");
                writer.WriteStartArray(value.Members.Count);
                foreach (var member in value.Members)
                    context.Serialize(member, null, writer);
                writer.WriteEndArray();
            }

            if (value.Methods.Count > 0)
            {
                writer.WriteTextString("methods");
                writer.WriteStartArray(value.Methods.Count);
                foreach (var method in value.Methods)
                    context.Serialize(method, null, writer);
                writer.WriteEndArray();
            }

            writer.WriteEndMap();
        });
    }
}
