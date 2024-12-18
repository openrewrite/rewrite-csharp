using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Rewrite.RewriteJava.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
public interface JavaType
{
    internal static JavaType[] EMPTY_JAVA_TYPE_ARRAY = [];
    internal static FullyQualified[] EMPTY_FULLY_QUALIFIED_ARRAY = [];
    internal static Method[] EMPTY_METHOD_ARRAY = [];
    internal static Variable[] EMPTY_VARIABLE_ARRAY = [];
    internal static Annotation.ElementValue[] EMPTY_ANNOTATION_VALUE_ARRAY = [];

    internal int? ManagedReference => null;

    internal JavaType WithManagedReference(int id)
    {
        return this;
    }

    public bool IsAssignableFrom(Regex pattern)
    {
        if (this is FullyQualified fq)
        {
            if (pattern.Match(fq.FullyQualifiedName).Success)
            {
                return true;
            }

            if (fq.Supertype != null && (fq.Supertype as JavaType).IsAssignableFrom(pattern))
            {
                return true;
            }

            return fq.Interfaces.Any(anInterface => (anInterface as JavaType).IsAssignableFrom(pattern));
        }

        if (this is GenericTypeVariable generic)
        {
            return generic.Bounds.Any(bound => bound.IsAssignableFrom(pattern));
        }

        return false;
    }

    public sealed class MultiCatch : JavaType
    {
        private readonly IList<JavaType> _throwableTypes;

        public MultiCatch(IList<JavaType>? throwableTypes)
        {
            _throwableTypes = throwableTypes ?? EMPTY_JAVA_TYPE_ARRAY;
        }

        public IList<JavaType> ThrowableTypes => _throwableTypes;
    }

    public sealed class Intersection : JavaType
    {
        private readonly IList<JavaType> _bounds;

        public Intersection(IList<JavaType>? bounds)
        {
            this._bounds = bounds ?? EMPTY_JAVA_TYPE_ARRAY;
        }

        public IList<JavaType> Bounds => _bounds;
    }

    public abstract class FullyQualified : JavaType
    {
        public abstract string FullyQualifiedName { get; internal set; }
        public abstract FullyQualified WithFullyQualifiedName(string fullyQualifiedName);
        public abstract IList<FullyQualified> Annotations { get; internal set; }
        public abstract bool HasFlags(Flag[] test);
        public abstract ISet<Flag> GetFlags();
        public abstract IList<FullyQualified> Interfaces { get; internal set; }
        public abstract TypeKind Kind { get; internal set; }
        public abstract IList<Variable> Members { get; internal set; }
        public abstract IList<Method> Methods { get; internal set; }
        public abstract IList<JavaType> TypeParameters { get; internal set; }

        public abstract FullyQualified? Supertype { get; internal set; }
        public abstract FullyQualified? OwningClass { get; internal set; }

        public string GetClassName()
        {
            var fqn = FullyQualifiedName;
            var className = fqn[(fqn.LastIndexOf('.') + 1)..];
            return TypeUtils.ToFullyQualifiedName(className);
        }

        public string GetPackageName()
        {
            var fqn = FullyQualifiedName;
            var endPackage = fqn.LastIndexOf('.');
            return endPackage < 0 ? "" : fqn[..endPackage];
        }

        public virtual bool IsAssignableTo(string fullyQualifiedName)
        {
            return TypeUtils.FullyQualifiedNamesAreEqual(FullyQualifiedName, fullyQualifiedName) ||
                   Interfaces.Any(anInterface => anInterface.IsAssignableTo(fullyQualifiedName))
                   || (Supertype?.IsAssignableTo(fullyQualifiedName) ?? false);
        }

        public virtual bool IsAssignableFrom(JavaType? type)
        {
            if (type is FullyQualified clazz)
            {
                return TypeUtils.FullyQualifiedNamesAreEqual(FullyQualifiedName, clazz.FullyQualifiedName) ||
                       IsAssignableFrom(clazz.Supertype) ||
                       clazz.Interfaces.Any(IsAssignableFrom);
            }

            if (type is GenericTypeVariable generic)
            {
                return generic.Bounds.Any(IsAssignableFrom);
            }

            return false;
        }

        public IEnumerator<Method> GetVisibleMethods() => GetVisibleMethods(GetPackageName());

        private IEnumerator<Method> GetVisibleMethods(String packageName)
        {
            return new FullyQualifiedEnumerator<Method>(
                this,
                packageName,
                m => m.FlagsBitMap,
                fq => fq.Methods,
                fq => fq.GetVisibleMethods(packageName)
            );
        }

        public IEnumerator<Variable> GetVisibleMembers() => GetVisibleMembers(GetPackageName());

        private IEnumerator<Variable> GetVisibleMembers(string packageName)
        {
            return new FullyQualifiedEnumerator<Variable>(
                this,
                packageName,
                v => v.FlagsBitMap,
                fq => fq.Members,
                fq => fq.GetVisibleMembers(packageName)
            );
        }

        private class FullyQualifiedEnumerator<E>(
#pragma warning disable // TODO: needs to be fully implemented
            FullyQualified fq,
            string packageName,
            Func<E, long> flagsFn,
            Func<FullyQualified, IList<E>> baseFn,
            Func<FullyQualified, IEnumerator<E>> recursiveFn) : IEnumerator<E>

        {
            public bool MoveNext()
            {
                throw new NotImplementedException();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            public E Current { get; } = default!;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }

        public enum TypeKind
        {
            Class,
            Enum,
            Interface,
            Annotation,
            Record,
            Value
        }
    }
#pragma warning restore

    public class Class : FullyQualified
    {
        internal Class()
        {
        }


        public Class(
            int? managedReference,
            long flagsBitMap,
            string fullyQualifiedName,
            TypeKind kind,
            JavaType[]? typeParameters,
            FullyQualified? supertype,
            FullyQualified? owningClass,
            FullyQualified[]? annotations,
            FullyQualified[]? interfaces,
            Variable[]? members,
            Method[]? methods
        )
        {
            ManagedReference = managedReference;
            FlagsBitMap = flagsBitMap;
            FullyQualifiedName = fullyQualifiedName;
            Kind = kind;
            TypeParameters = typeParameters ?? EMPTY_JAVA_TYPE_ARRAY;
            Supertype = supertype;
            OwningClass = owningClass;
            Annotations = annotations ?? EMPTY_FULLY_QUALIFIED_ARRAY;
            Interfaces = interfaces ?? EMPTY_FULLY_QUALIFIED_ARRAY;
            Members = members ?? EMPTY_VARIABLE_ARRAY;
            Methods = methods ?? EMPTY_METHOD_ARRAY;
        }

        private Class(
            int? managedReference,
            long flagsBitMap,
            string fullyQualifiedName,
            TypeKind kind,
            IList<JavaType>? typeParameters,
            FullyQualified? supertype,
            FullyQualified? owningClass,
            IList<FullyQualified>? annotations,
            IList<FullyQualified>? interfaces,
            IList<Variable>? members,
            IList<Method>? methods
        )
        {
            ManagedReference = managedReference;
            FlagsBitMap = flagsBitMap;
            FullyQualifiedName = fullyQualifiedName;
            Kind = kind;
            TypeParameters = typeParameters ?? EMPTY_JAVA_TYPE_ARRAY;
            Supertype = supertype;
            OwningClass = owningClass;
            Annotations = annotations ?? EMPTY_FULLY_QUALIFIED_ARRAY;
            Interfaces = interfaces ?? EMPTY_FULLY_QUALIFIED_ARRAY;
            Members = members ?? EMPTY_VARIABLE_ARRAY;
            Methods = methods ?? EMPTY_METHOD_ARRAY;
        }

        public int? ManagedReference { get; internal set; }
        public long FlagsBitMap { get; internal set; }
        public override string FullyQualifiedName { get; internal set; } = null!;
        public override TypeKind Kind { get; internal set; }

        public override IList<JavaType> TypeParameters { get; internal set; } = EMPTY_JAVA_TYPE_ARRAY;
        public override FullyQualified? Supertype { get; internal set; }
        public override FullyQualified? OwningClass { get; internal set; }

        public override IList<FullyQualified> Annotations { get; internal set; } = EMPTY_FULLY_QUALIFIED_ARRAY;

        public override IList<FullyQualified> Interfaces { get; internal set; } = EMPTY_FULLY_QUALIFIED_ARRAY;
        public override IList<Variable> Members { get; internal set; } = EMPTY_VARIABLE_ARRAY;
        public override IList<Method> Methods { get; internal set; } = EMPTY_METHOD_ARRAY;

        public override ISet<Flag> GetFlags() => FlagExtensions.BitMapToFlags(FlagsBitMap);

        public override bool HasFlags(params Flag[] test)
        {
            return FlagExtensions.HasFlags(FlagsBitMap, test);
        }

        public override FullyQualified WithFullyQualifiedName(string fullyQualifiedName)
        {
            if (fullyQualifiedName.Equals(FullyQualifiedName))
            {
                return this;
            }

            return new Class(ManagedReference, FlagsBitMap, fullyQualifiedName, Kind, TypeParameters, Supertype,
                OwningClass, Annotations, Interfaces, Members, Methods);
        }

        public Class WithInterfaces(IList<FullyQualified>? interfaces)
        {
            var i = interfaces ?? EMPTY_FULLY_QUALIFIED_ARRAY;
            if (i.SequenceEqual(Interfaces))
            {
                return this;
            }

            return new Class(ManagedReference, FlagsBitMap, FullyQualifiedName, Kind, TypeParameters,
                Supertype, OwningClass, Annotations, i, Members, Methods);
        }

        public Class WithMembers(IList<Variable>? members)
        {
            var m = members ?? EMPTY_VARIABLE_ARRAY;
            if (m.SequenceEqual(Members))
            {
                return this;
            }

            return new Class(ManagedReference, FlagsBitMap, FullyQualifiedName, Kind, TypeParameters,
                Supertype, OwningClass, Annotations, Interfaces, m, Methods);
        }

        /// <summary>
        /// Hello <see cref="string.PadLeft(int)"/>
        /// </summary>
        /// <param name="typeParameters"></param>
        /// <param name="supertype"></param>
        /// <param name="owningClass"></param>
        /// <param name="annotations"></param>
        /// <param name="interfaces"></param>
        /// <param name="members"></param>
        /// <param name="methods"></param>
        internal void UnsafeSet(IList<JavaType>? typeParameters, FullyQualified? supertype, FullyQualified? owningClass,
            IList<FullyQualified>? annotations, IList<FullyQualified>? interfaces,
            IList<Variable>? members, IList<Method>? methods)
        {
            TypeParameters = typeParameters ?? EMPTY_JAVA_TYPE_ARRAY;
            Supertype = supertype;
            OwningClass = owningClass;
            Annotations = annotations ?? EMPTY_FULLY_QUALIFIED_ARRAY;
            Interfaces = interfaces ?? EMPTY_FULLY_QUALIFIED_ARRAY;
            Members = members ?? EMPTY_VARIABLE_ARRAY;
            Methods = methods ?? EMPTY_METHOD_ARRAY;
        }
    }

    public sealed class ShallowClass : Class
    {
        internal ShallowClass()
        {
        }

        public ShallowClass(int? ManagedReference,
            long FlagsBitMap,
            string FullyQualifiedName,
            TypeKind Kind,
            JavaType[]? TypeParameters,
            FullyQualified? Supertype,
            FullyQualified? OwningClass,
            FullyQualified[]? Annotations,
            FullyQualified[]? Interfaces,
            Variable[]? Members,
            Method[]? Methods) : base(ManagedReference, FlagsBitMap, FullyQualifiedName, Kind, TypeParameters,
            Supertype,
            OwningClass, Annotations, Interfaces, Members, Methods)
        {
        }
    }

    public sealed class Parameterized : FullyQualified
    {
        internal Parameterized()
        {
        }

        public Parameterized(int? managedReference, FullyQualified? type, JavaType[]? typeParameters)
        {
            ManagedReference = managedReference;
            Type = type ?? Unknown.Instance;
            TypeParameters = typeParameters ?? EMPTY_JAVA_TYPE_ARRAY;
        }

        Parameterized(int? managedReference, FullyQualified? type, IList<JavaType>? typeParameters)
        {
            ManagedReference = managedReference;
            Type = type ?? Unknown.Instance;
            TypeParameters = typeParameters ?? EMPTY_JAVA_TYPE_ARRAY;
        }

        public int? ManagedReference { get; internal set; }
        public FullyQualified? Type { get; internal set; }

        public override string FullyQualifiedName
        {
            get => Type!.FullyQualifiedName;
            internal set => Type!.FullyQualifiedName = value;
        }

        public override FullyQualified WithFullyQualifiedName(string fullyQualifiedName)
        {
            var newType = Type!.WithFullyQualifiedName(fullyQualifiedName);
            if (newType == Type)
            {
                return this;
            }

            return new Parameterized(ManagedReference, newType, TypeParameters);
        }

        public override IList<FullyQualified> Annotations { get; internal set; } = EMPTY_FULLY_QUALIFIED_ARRAY;

        public override bool HasFlags(params Flag[] test)
        {
            return Type!.HasFlags(test);
        }

        public override ISet<Flag> GetFlags()
        {
            return Type!.GetFlags();
        }

        public override IList<FullyQualified> Interfaces { get; internal set; } = EMPTY_FULLY_QUALIFIED_ARRAY;
        public override TypeKind Kind { get; internal set; }
        public override IList<Variable> Members { get; internal set; } = EMPTY_VARIABLE_ARRAY;
        public override IList<Method> Methods { get; internal set; } = EMPTY_METHOD_ARRAY;
        public override IList<JavaType> TypeParameters { get; internal set; } = EMPTY_JAVA_TYPE_ARRAY;
        public override FullyQualified? Supertype { get; internal set; }
        public override FullyQualified? OwningClass { get; internal set; }
    }

    public sealed class Annotation : FullyQualified
    {
        internal Annotation()
        {
        }

        public Annotation(FullyQualified type,
            IList<ElementValue>? values)
        {
            Type = type;
            Values = values ?? EMPTY_ANNOTATION_VALUE_ARRAY;
        }

        public IList<ElementValue> Values { get; internal set; } = EMPTY_ANNOTATION_VALUE_ARRAY;

        public Annotation WithValues(IList<ElementValue>? values)
        {
            var newValues = values ?? EMPTY_ANNOTATION_VALUE_ARRAY;
            if (newValues == Values)
            {
                return this;
            }

            return new Annotation(Type, newValues);
        }

        public FullyQualified Type { get; internal set; } = null!;

        public Annotation WithType(FullyQualified type)
        {
            if (type == Type)
            {
                return this;
            }

            return new Annotation(type, Values);
        }

        public override string FullyQualifiedName
        {
            get => Type.FullyQualifiedName;
            internal set => Type.FullyQualifiedName = value;
        }

        public override FullyQualified WithFullyQualifiedName(string fullyQualifiedName)
        {
            return WithType(Type.WithFullyQualifiedName(fullyQualifiedName));
        }

        public override IList<FullyQualified> Annotations
        {
            get => Type.Annotations;
            internal set => Type.Annotations = value;
        }

        public override bool HasFlags(Flag[] test)
        {
            return Type.HasFlags(test);
        }

        public override ISet<Flag> GetFlags()
        {
            return Type.GetFlags();
        }

        public override IList<FullyQualified> Interfaces
        {
            get => Type.Interfaces;
            internal set => Type.Interfaces = value;
        }

        public override TypeKind Kind
        {
            get => Type.Kind;
            internal set => Type.Kind = value;
        }

        public override IList<Variable> Members
        {
            get => Type.Members;
            internal set => Type.Members = value;
        }

        public override IList<Method> Methods
        {
            get => Type.Methods;
            internal set => Type.Methods = value;
        }

        public override IList<JavaType> TypeParameters
        {
            get => Type.TypeParameters;
            internal set => Type.TypeParameters = value;
        }

        public override FullyQualified? Supertype
        {
            get => Type.Supertype;
            internal set => Type.Supertype = value;
        }

        public override FullyQualified? OwningClass
        {
            get => Type.OwningClass;
            internal set => Type.OwningClass = value;
        }

        public interface ElementValue
        {
            JavaType Element { get; }
            object GetValue();
        }

        public sealed class SingleElementValue : ElementValue
        {
            internal SingleElementValue()
            {
            }

            public SingleElementValue(JavaType element, object value)
            {
                Element = element;
                if (value is JavaType javaType)
                {
                    ReferenceValue = javaType;
                }
                else
                {
                    ConstantValue = value;
                }
            }

            public JavaType Element { get; internal set; } = null!;

            internal object? ConstantValue { get; set; } = null!;
            internal JavaType? ReferenceValue { get; set; } = null!;

            public object GetValue()
            {
                return ConstantValue ?? ReferenceValue!;
            }
        }

        public sealed class ArrayElementValue : ElementValue
        {
            internal ArrayElementValue()
            {
            }

            public ArrayElementValue(JavaType element, IList<object> values)
            {
                Element = element;
                if (values.Count == 0)
                {
                    ConstantValues = null;
                    ReferenceValues = null;
                }
                else if (values is IList<JavaType> javaTypes)
                {
                    ReferenceValues = javaTypes;
                }
                else
                {
                    ConstantValues = values;
                }
            }

            public JavaType Element { get; internal set; } = null!;

            internal IList<object>? ConstantValues { get; set; }
            internal IList<JavaType>? ReferenceValues { get; set; }

            public object GetValue()
            {
                return ConstantValues != null ? ConstantValues :
                    ReferenceValues != null ? ReferenceValues :
                    EMPTY_ANNOTATION_VALUE_ARRAY;
            }
        }
    }

    public sealed class GenericTypeVariable : JavaType
    {
        internal GenericTypeVariable()
        {
        }

        public GenericTypeVariable(int? managedReference,
            string name,
            GenericTypeVariable.VarianceType variance,
            IList<JavaType>? bounds)
        {
            ManagedReference = managedReference;
            Name = name;
            Variance = variance;
            Bounds = bounds ?? EMPTY_JAVA_TYPE_ARRAY;
        }

        public enum VarianceType
        {
            INVARIANT,
            COVARIANT,
            CONTRAVARIANT
        }

        public IList<JavaType> Bounds { get; internal set; } = EMPTY_JAVA_TYPE_ARRAY;

        public GenericTypeVariable WithBounds(IList<JavaType>? bounds)
        {
            var newBounds = bounds ?? EMPTY_JAVA_TYPE_ARRAY;
            if (newBounds == Bounds)
            {
                return this;
            }

            return new GenericTypeVariable(ManagedReference, Name, Variance, newBounds);
        }

        internal int? ManagedReference { get; set; }

        public VarianceType Variance { get; internal set; }

        public string Name { get; internal set; } = null!;
    }

    public sealed class Array : JavaType
    {
        public Array()
        {
        }

        public Array(int? managedReference, JavaType? elementType, IList<FullyQualified>? annotations)
        {
            ManagedReference = managedReference;
            ElementType = elementType ?? Unknown.Instance;
            Annotations = annotations ?? EMPTY_FULLY_QUALIFIED_ARRAY;
        }

        internal int? ManagedReference { get; set; }

        public JavaType ElementType { get; internal set; } = Unknown.Instance;

        public IList<FullyQualified> Annotations { get; internal set; } = EMPTY_FULLY_QUALIFIED_ARRAY;

        public void UnsafeSet(JavaType elementType, IList<FullyQualified>? annotations)
        {
            ElementType = elementType;
            Annotations = annotations ?? EMPTY_FULLY_QUALIFIED_ARRAY;
        }
    }

    public class Primitive(Primitive.PrimitiveType kind) : JavaType
    {
        public PrimitiveType Kind => kind;

        public string GetKeyword() => kind == PrimitiveType.None ? "" : kind.ToString().ToLower();

        public bool IsNumeric()
        {
            return kind is PrimitiveType.Double or PrimitiveType.Int or PrimitiveType.Float or PrimitiveType.Long
                or PrimitiveType.Short;
        }

        public enum PrimitiveType
        {
            Boolean,
            Byte,
            Char,
            Double,
            Float,
            Int,
            Long,
            Short,
            Void,
            String,
            None,
            Null,
        }
    }

    public sealed class Method : JavaType
    {
        internal Method()
        {
        }

        public Method(int? managedReference,
            long flagsBitMap,
            FullyQualified? declaringType,
            string name,
            JavaType? returnType,
            IList<string>? parameterNames,
            IList<JavaType>? parameterTypes,
            IList<FullyQualified>? thrownExceptions,
            IList<FullyQualified>? annotations,
            IList<string>? defaultValue)
        {
            ManagedReference = managedReference;
            FlagsBitMap = flagsBitMap;
            DeclaringType = declaringType ?? Unknown.Instance;
            Name = name;
            ReturnType = returnType ?? Unknown.Instance;
            ParameterNames = parameterNames ?? (Enumerable.Empty<string>() as IList<string>)!;
            ParameterTypes = parameterTypes ?? EMPTY_JAVA_TYPE_ARRAY;
            ThrownExceptions = thrownExceptions ?? EMPTY_FULLY_QUALIFIED_ARRAY;
            Annotations = annotations ?? EMPTY_FULLY_QUALIFIED_ARRAY;
            DefaultValue = defaultValue;
        }

        public int? ManagedReference { get; internal set; }

        public long FlagsBitMap { get; internal set; }

        public bool HasFlags(params Flag[] flags)
        {
            return FlagExtensions.HasFlags(FlagsBitMap, flags);
        }

        public ISet<Flag> GetFlags() => FlagExtensions.BitMapToFlags(FlagsBitMap);

        public FullyQualified DeclaringType { get; internal set; } = Unknown.Instance;

        public string Name { get; internal set; } = null!;

        public JavaType ReturnType { get; internal set; } = Unknown.Instance;

        public IList<string> ParameterNames { get; internal set; } = [];

        public IList<JavaType> ParameterTypes { get; internal set; } = EMPTY_JAVA_TYPE_ARRAY;

        public bool isConstructor()
        {
            return "<constructor>".Equals(Name);
        }

        public IList<FullyQualified> ThrownExceptions { get; set; } = [];

        public IList<FullyQualified> Annotations { get; internal set; } = [];

        public IList<string>? DefaultValue { get; internal set; } = [];
    }

    public sealed class Variable : JavaType
    {
        internal Variable()
        {
        }

        public Variable(int? managedReference,
            long flagsBitMap,
            string name,
            JavaType? owner,
            JavaType? type,
            IList<FullyQualified>? annotations)
        {
            ManagedReference = managedReference;
            FlagsBitMap = flagsBitMap;
            Name = name;
            Owner = owner;
            Type = type ?? Unknown.Instance;
            Annotations = annotations ?? EMPTY_FULLY_QUALIFIED_ARRAY;
        }

        public int? ManagedReference { get; internal set; }

        public long FlagsBitMap { get; internal set; }
        public ISet<Flag> GetFlags() => FlagExtensions.BitMapToFlags(FlagsBitMap);

        public bool HasFlags(params Flag[] flags) => FlagExtensions.HasFlags(FlagsBitMap, flags);

        public string Name { get; internal set; } = null!;

        public JavaType? Owner { get; internal set; }

        public JavaType Type { get; internal set; } = Unknown.Instance;

        public Variable WithType(JavaType? javaType)
        {
            var jType = javaType ?? Unknown.Instance;
            return Type == jType
                ? this
                : new Variable(ManagedReference, FlagsBitMap, Name, Owner, jType, Annotations);
        }

        public IList<FullyQualified> Annotations { get; internal set; } = EMPTY_FULLY_QUALIFIED_ARRAY;

        public void UnsafeSet(JavaType? owner, JavaType? type, IList<FullyQualified>? annotations)
        {
            Owner = owner;
            Type = type ?? Unknown.Instance;
            Annotations = annotations ?? (Enumerable.Empty<FullyQualified>() as IList<FullyQualified>)!;
        }
    }

    public sealed class Unknown : FullyQualified, JavaType
    {
        private static readonly Unknown instance = new();

        private Unknown()
        {
        }

        public static Unknown Instance => instance;

        public override string FullyQualifiedName
        {
            get => "<unknown>";
            internal set => throw new NotImplementedException();
        }

        public override FullyQualified WithFullyQualifiedName(string fullyQualifiedName)
        {
            return this;
        }

        public override IList<FullyQualified> Annotations
        {
            get => EMPTY_FULLY_QUALIFIED_ARRAY;
            internal set => throw new NotImplementedException();
        }

        public override bool HasFlags(params Flag[] test)
        {
            return false;
        }

        public override ISet<Flag> GetFlags()
        {
            return ImmutableHashSet<Flag>.Empty;
        }

        public override IList<FullyQualified> Interfaces
        {
            get => EMPTY_FULLY_QUALIFIED_ARRAY;
            internal set => throw new NotImplementedException();
        }

        public override TypeKind Kind
        {
            get => TypeKind.Class;
            internal set => throw new NotImplementedException();
        }

        public override IList<Variable> Members
        {
            get => EMPTY_VARIABLE_ARRAY;
            internal set => throw new NotImplementedException();
        }

        public override IList<Method> Methods
        {
            get => EMPTY_METHOD_ARRAY;
            internal set => throw new NotImplementedException();
        }

        public override FullyQualified? OwningClass
        {
            get => null;
            internal set => throw new NotImplementedException();
        }

        public override FullyQualified? Supertype
        {
            get => null;
            internal set => throw new NotImplementedException();
        }

        public override IList<JavaType> TypeParameters
        {
            get => EMPTY_JAVA_TYPE_ARRAY;
            internal set => throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "Unknown";
        }

        bool JavaType.IsAssignableFrom(Regex pattern)
        {
            return false;
        }

        public override bool IsAssignableFrom(JavaType? type)
        {
            return false;
        }

        public override bool IsAssignableTo(string fullyQualifiedName)
        {
            return false;
        }
    }
}
