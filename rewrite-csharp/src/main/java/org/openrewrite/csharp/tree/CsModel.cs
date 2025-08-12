/*
 * Copyright 2024 the original author or authors.
 * <p>
 * Licensed under the Moderne Source Available License (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * <p>
 * https://docs.moderne.io/licensing/moderne-source-available-license
 * <p>
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
namespace org.openrewrite.csharp.tree;

using com.fasterxml.jackson.annotation.JsonCreator;
using lombok.*;
using lombok.experimental.FieldDefaults;
using lombok.experimental.NonFinal;
using org.jspecify.annotations.Nullable;
using org.openrewrite.*;
using org.openrewrite.csharp.CSharpPrinter;
using org.openrewrite.csharp.CSharpVisitor;
using org.openrewrite.csharp.service.CSharpNamingService;
using org.openrewrite.internal.ListUtils;
using org.openrewrite.internal.NamingService;
using org.openrewrite.java.JavaPrinter;
using org.openrewrite.java.JavaTypeVisitor;
using org.openrewrite.java.JavaVisitor;
using org.openrewrite.java.internal.TypesInUse;
using org.openrewrite.java.tree.*;
using org.openrewrite.marker.Marker;
using org.openrewrite.marker.Markers;

using java.beans.Transient;
using java.lang.ref.SoftReference;
using java.lang.ref.WeakReference;
using java.nio.charset.Charset;
using java.nio.charset.StandardCharsets;
using java.nio.file.Path;
using java.util.ArrayList;
using java.util.Collections;
using java.util.List;
using java.util.UUID;
using java.util.concurrent.atomic.AtomicInteger;
using java.util.function.Predicate;
using java.util.stream.Collectors;

using static java.util.Collections.singletonList;

public interface CsModel : J {

    
    final class CompilationUnit {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        Path sourcePath;

        
        [Nullable]
        FileAttributes fileAttributes;

        
        [Nullable]
        String charsetName;

        
        bool charsetBomMarked;

        
        [Nullable]
        Checksum checksum;

        List<JRightPadded<ExternAlias>> externs;

        List<JRightPadded<UsingDirective>> usings;

        
        List<AttributeList> attributeLists;

        List<JRightPadded<Statement>> members;

        
        Space eof;
    }

    /**
     * Represents an operator declaration in C# classes, which allows overloading of operators
     * for custom types.
     * <p>
     * For example:
     * <pre>
     *     // Unary operator overload
     *     public static Vector operator +(Vector a)
     *
     *     // Binary operator overload
     *     public static Point operator *(Point p, float scale)
     *
     *     // Interface implementation
     *     IEnumerable<T>.Vector operator +(Vector a)
     *
     *     // Conversion operator
     *     public static explicit operator int(Complex c)
     *
     *     // Custom operator
     *     public static Point operator ++(Point p)
     * </pre>
     */
    
    final class OperatorDeclaration {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        List<AttributeList> attributeLists;

        
        List<J.Modifier> modifiers;

        /**
         * <pre>
         * IEnumerable<T>.Vector operator +(Vector a)
         * ^^^^^^^^^^^^^^^^^^
         * </pre>
         */
        [Nullable]
        JRightPadded<TypeTree> explicitInterfaceSpecifier;


        /**
         * <pre>
         * public static Vector operator +(Vector a)
         *                    ^^^^^^^^
         * </pre>
         */
        
        Cs.Keyword operatorKeyword;

        /**
         * <pre>
         * public static Integer operator checked +(Integer a, Integer b)
         *                               ^^^^^^^^
         * </pre>
         */
        
        Cs.[Nullable] Keyword checkedKeyword;

        /**
         * <pre>
         * public static Vector operator +(Vector a)
         *                            ^
         * </pre>
         */
        JLeftPadded<Operator> operatorToken;

        /**
         * <pre>
         * public static explicit operator int(Complex c)
         *                                ^^^^
         * </pre>
         */
        
        TypeTree returnType;

        /**
         * <pre>
         * public static Vector operator + (Vector a)
         *                                ^^^^^^^^^
         * </pre>
         */
        JContainer<Expression> parameters;

        /**
         * <pre>
         * public static Vector operator +(...) { ... }
         *                                      ^^^^^^^
         * </pre>
         */
        
        J.Block body;

        
        JavaType.[Nullable] Method methodType;

        public enum Operator {
            /**
             * + token
             */
            Plus,

            /**
             * - token
             */
            Minus,

            /**
             * ! token
             */
            Bang,

            /**
             * ~ token
             */
            Tilde,

            /**
             * ++ token
             */
            PlusPlus,

            /**
             * -- token
             */
            MinusMinus,

            /**
             * * token
             */
            Star,

            /**
             * / token
             */
            Division,

            /**
             * % token
             */
            Percent,

            /**
             * << token
             */
            LeftShift,

            /**
             * >> token
             */
            RightShift,

            /**
             * < token
             */
            LessThan,

            /**
             * > token
             */
            GreaterThan,

            /**
             * <= token
             */
            LessThanEquals,

            /**
             * >= token
             */
            GreaterThanEquals,

            /**
             * == token
             */
            Equals,

            /**
             * != token
             */
            NotEquals,

            /**
             * & token
             */
            Ampersand,

            /**
             * | token
             */
            Bar,

            /**
             * ^ token
             */
            Caret,

            /**
             * true token
             */
            True,

            /**
             * false token
             */
            False
        }
    }


    /**
     * Represents a C# ref expression used to pass variables by reference.
     * <p>
     * For example:
     * <pre>
     *     // Method call with ref argument
     *     Process(ref value);
     *
     *     // Return ref value
     *     return ref field;
     *
     *     // Local ref assignment
     *     ref int x = ref field;
     *
     *     // Ref property return
     *     public ref int Property => ref field;
     * </pre>
     */
    
    final class RefExpression {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * Process(ref value)
         *            ^^^^^
         * </pre>
         */
        
        Expression expression;
    }

    /**
     * Represents a C# pointer type declaration.
     * <p>
     * For example:
     * <pre>
     *     // Basic pointer declaration
     *     int* ptr;
     *        ^
     *
     *     // Pointer to pointer
     *     int** ptr;
     *         ^
     * </pre>
     */
    
    final class PointerType {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * int* ptr;
         * ^^^
         * </pre>
         */
        JRightPadded<TypeTree> elementType;
    }

    /**
     * Represents a C# ref type, which indicates that a type is passed or returned by reference.
     * Used in method parameters, return types, and local variable declarations.
     * <p>
     * For example:
     * <pre>
     *     // Method parameter
     *     void Process(ref int value)
     *
     *     // Method return type
     *     ref int GetValue()
     *
     *     // Local variable
     *     ref int number = ref GetValue();
     *
     *     // Property
     *     ref readonly int Property => ref field;
     * </pre>
     */
    
    final class RefType {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * ref readonly int number
         *     ^^^^^^^^
         * </pre>
         */
        
        [Nullable]
        Modifier readonlyKeyword;

        /**
         * <pre>
         * ref readonly int number
         *              ^^^
         * </pre>
         */
        
        TypeTree typeIdentifier;

        
        [Nullable]
        JavaType type;
    }


    
    final class ForEachVariableLoop {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        Control controlElement;

        JRightPadded<Statement> body;

        
        public static final class Control {

            
            UUID id;

            
            Space prefix;

            
            Markers markers;

            JRightPadded<Expression> variable;

            JRightPadded<Expression> iterable;
        }
    }

    /**
     * Represents a name and colon syntax in C#, which is used in various contexts such as named arguments,
     * tuple elements, and property patterns.
     * <p>
     * For example:
     * <pre>
     *     // In named arguments
     *     Method(name: "John", age: 25)
     *            ^^^^          ^^^^
     *
     *     // In tuple literals
     *     (name: "John", age: 25)
     *      ^^^^          ^^^^
     *
     *     // In property patterns
     *     { Name: "John", Age: 25 }
     *      ^^^^          ^^^^
     * </pre>
     */
    
    final class NameColon {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * Method(name: "John")
         *        ^^^^
         * </pre>
         */
        JRightPadded<J.Identifier> name;
    }


    
    final class Argument {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        [Nullable]
        JRightPadded<Identifier> nameColumn;


        [Nullable]
        Keyword refKindKeyword;

        
        Expression expression;
    }


    
    final class AnnotatedStatement {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        List<AttributeList> attributeLists;

        
        Statement statement;
    }

    
    final class ArrayRankSpecifier {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        JContainer<Expression> sizes;
    }

    
    final class AssignmentOperation {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        Expression variable;

        JLeftPadded<OperatorType> operator;

        
        Expression assignment;

        
        [Nullable]
        JavaType type;

        public enum OperatorType {
            NullCoalescing
        }
    }

    
    final class AttributeList {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        [Nullable]
        JRightPadded<Identifier> target;

        List<JRightPadded<Annotation>> attributes;
    }

    
    final class AwaitExpression {

        
        UUID id;

        Space prefix;
        Markers markers;

        J expression;

        [Nullable]
        JavaType type;
    }

    
    final class StackAllocExpression {

        
        UUID id;

        Space prefix;
        Markers markers;

        J.NewArray expression;

    }

    /**
     * Represents a C# goto statement, which performs an unconditional jump to a labeled statement,
     * case label, or default label within a switch statement.
     * <p>
     * For example:
     * <pre>
     *     // Simple goto statement
     *     goto Label;
     *
     *     // Goto case in switch statement
     *     goto case 1;
     *
     *     // Goto default in switch statement
     *     goto default;
     *
     *     // With label declaration
     *     Label:
     *     Console.WriteLine("At label");
     * </pre>
     */
    
    final class GotoStatement {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * goto case 1;
         *      ^^^^
         * goto default;
         *      ^^^^^^^
         * </pre>
         */
        
        [Nullable]
        Keyword caseOrDefaultKeyword;

        /**
         * <pre>
         * goto case 1;
         *           ^
         * goto Label;
         *      ^^^^^
         * </pre>
         */
        
        [Nullable]
        Expression target;
    }

    /**
     * Represents a C# event declaration.
     * <p>
     * For example:
     * <pre>
     * // Simple event declaration
     * public event EventHandler OnClick;
     *
     * // With explicit add/remove accessors
     * public event EventHandler OnChange {
     *     add { handlers += value; }
     *     remove { handlers -= value; }
     * }
     *
     * // Generic event
     * public event EventHandler<TEventArgs> OnDataChanged;
     *
     * // Custom delegate type
     * public event MyCustomDelegate OnCustomEvent;
     *
     * // Static event
     * public static event Action StaticEvent;
     * </pre>
     */
    
    final class EventDeclaration {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * [Obsolete] public event EventHandler OnClick;
         * ^^^^^^^^^
         * </pre>
         */
        
        List<AttributeList> attributeLists;

        /**
         * <pre>
         * public event EventHandler OnClick;
         * ^^^^^^
         * </pre>
         */
        
        List<Modifier> modifiers;

        JLeftPadded<TypeTree> typeExpression;

        [Nullable]
        JRightPadded<TypeTree> interfaceSpecifier;

        /**
         * <pre>
         * public event EventHandler OnClick;
         *                          ^^^^^^^
         * </pre>
         */
        
        Identifier name;

        /**
         * <pre>
         * public event EventHandler OnChange {
         *                                   ^^^^^^^^^^^^^^^^^^^^^^^^^^^^
         * </pre>
         */
        [Nullable]
        JContainer<Statement> accessors;
    }


    
    final class Binary {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        Expression left;

        JLeftPadded<OperatorType> operator;

        
        Expression right;

        
        [Nullable]
        JavaType type;

        public enum OperatorType {
            As,
            NullCoalescing
        }
    }

    
    class BlockScopeNamespaceDeclaration {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        JRightPadded<Expression> name;

        List<JRightPadded<ExternAlias>> externs;

        List<JRightPadded<UsingDirective>> usings;

        List<JRightPadded<Statement>> members;

        
        Space end;
    }

    
    final class CollectionExpression {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        List<JRightPadded<Expression>> elements;

        
        [Nullable]
        JavaType type;
    }

    
    final class ExpressionStatement {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        JRightPadded<Expression> expression;
    }

    
    final class ExternAlias {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        JLeftPadded<Identifier> identifier;
    }

    
    class FileScopeNamespaceDeclaration {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        JRightPadded<Expression> name;

        List<JRightPadded<ExternAlias>> externs;

        List<JRightPadded<UsingDirective>> usings;

        List<JRightPadded<Statement>> members;
    }

    
    class InterpolatedString {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        String start;

        List<JRightPadded<Expression>> parts;

        
        String end;
    }

    
    class Interpolation {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        JRightPadded<Expression> expression;

        [Nullable]
        JRightPadded<Expression> alignment;

        [Nullable]
        JRightPadded<Expression> format;
    }

    
    class NullSafeExpression {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        JRightPadded<Expression> expression;
    }

    
    final class StatementExpression {

        
        UUID id;
        Space prefix;
        Markers markers;

        Statement statement;
    }

    
    class UsingDirective {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        JRightPadded<Boolean> global;

        JLeftPadded<Boolean> statik;

        JLeftPadded<Boolean> unsafe;

        [Nullable]
        JRightPadded<Identifier> alias;

        
        TypeTree namespaceOrType;
    }


    
    class PropertyDeclaration {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        List<AttributeList> attributeLists;

        
        List<Modifier> modifiers;

        
        TypeTree typeExpression;

        [Nullable]
        JRightPadded<TypeTree> interfaceSpecifier;

        
        Identifier name;

        
        [Nullable]
        Block accessors;

        
        [Nullable]
        ArrowExpressionClause expressionBody;

        [Nullable]
        JLeftPadded<Expression> initializer;
    }

    
    final class Keyword {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        KeywordKind kind;

        public enum KeywordKind {
            Ref,
            Out,
            Await,
            Base,
            This,
            Break,
            Return,
            Not,
            Default,
            Case,
            Checked,
            Unchecked,
            Operator
        }
    }


    
    final class Lambda {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        J.Lambda lambdaExpression;

        
        [Nullable]
        TypeTree returnType;

        
        List<Modifier> modifiers;
    }

    
    final class ClassDeclaration {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        List<Cs.AttributeList> attributeList;

        
        List<Modifier> modifiers;

        J.ClassDeclaration.Kind kind;

        
        Identifier name;

        [Nullable]
        JContainer<Cs.TypeParameter> typeParameters;

        [Nullable]
        JContainer<Statement> primaryConstructor;

        [Nullable]
        JLeftPadded<TypeTree> extendings;

        [Nullable]
        JContainer<TypeTree> implementings;

        
        [Nullable]
        Block body;

        [Nullable]
        JContainer<TypeParameterConstraintClause> typeParameterConstraintClauses;

        
        JavaType.[Nullable] FullyQualified type;
    }


    //  CS specific method exists to allow for modelling for the following not possible in J version:
    // - implicit interface implementations
    // - Cs.AttributeList that may appear before any of the type variables
    // - generics constraints that appear on the end of the method declaration
    
    final class MethodDeclaration {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        List<AttributeList> attributes;

        
        List<Modifier> modifiers;

        [Nullable]
        JContainer<Cs.TypeParameter> typeParameters;

        
        TypeTree returnTypeExpression;

        [Nullable]
        JRightPadded<TypeTree> explicitInterfaceSpecifier;


        
        Identifier name;

        JContainer<Statement> parameters;


        /**
         * Null for abstract method declarations and interface method declarations.
         */
        
        [Nullable]
        Statement body;

        
        JavaType.[Nullable] Method methodType;

        JContainer<TypeParameterConstraintClause> typeParameterConstraintClauses;

    }

    
    final class UsingStatement {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        [Nullable]
        Keyword awaitKeyword;

        JLeftPadded<Expression> expression;

        /**
         * The block is null for using declaration form.
         */
        
        Statement statement;
    }
    //endregion

    
    final class TypeParameterConstraintClause {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;


        /**
         * class A&lt;T&gt; where <b><i>T</i></b> : class
         */
        JRightPadded<Identifier> typeParameter;
        /**
         * class A&lt;T&gt; where T : <b><i>class, ISomething</i></b>
         */
        JContainer<TypeParameterConstraint> typeParameterConstraints;
    }

    interface TypeParameterConstraint : J {
    }

    /**
     * Represents a type constraint in a type parameter's constraint clause.
     * Example: where T : SomeClass
     * where T : IInterface
     */
    
    final class TypeConstraint {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        TypeTree typeExpression;
    }

    /* ------------------ */

    interface AllowsConstraint : J {
    }

    /**
     * Represents an `allows` constraint in a where clause.
     * Example: where T : allows operator +
     */
    
    final class AllowsConstraintClause {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        JContainer<AllowsConstraint> expressions;
    }

    /**
     * Represents a ref struct constraint in a where clause.
     * Example: where T : allows ref struct
     */
    
    final class RefStructConstraint {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;
    }


    /**
     * Represents a class/struct constraint in a where clause.
     * Example: where T : class, where T : struct
     */
    
    final class ClassOrStructConstraint {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        TypeKind kind;

        public enum TypeKind {
            Class,
            Struct
        }
    }

    /**
     * Represents a constructor constraint in a where clause.
     * Example:
     * <pre>
     * where T : new()
     *           ^^^^^
     * </pre>
     */
    
    final class ConstructorConstraint {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;
    }

    /**
     * Represents a default constraint in a where clause.
     * Example:
     * <pre>
     * where T : default
     *           ^^^^^^^
     * </pre>
     */
    
    final class DefaultConstraint {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;
    }

    /**
     * A declaration expression node represents a local variable declaration in an expression context.
     * This is used in two primary scenarios in C#:
     * <ul>
     *     <li>Out variable declarations: {@code Method(out int x)}</li>
     *     <li>Deconstruction declarations: {@code int (x, y) = GetPoint()}</li>
     * </ul>
     * Example 1: Out variable declaration:
     * <pre>
     * if(int.TryParse(s, out int result)) {
     *     // use result
     * }
     * </pre>
     * Example 2: Deconstruction declaration:
     * <pre>
     * int (x, y) = point;
     * ^^^^^^^^^^
     * (int count, var (name, age)) = GetPersonDetails();
     *             ^^^^^^^^^^^^^^^ DeclarationExpression
     *                 ^^^^^^^^^^^ ParenthesizedVariableDesignation
     *  ^^^^^^^^^ DeclarationExpression
     *      ^^^^^ SingleVariableDesignation
     * </pre>
     */
    
    public final class DeclarationExpression {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;


        
        [Nullable]
        TypeTree typeExpression;

        
        VariableDesignation variables;
    }

    //region VariableDesignation

    /**
     * Interface for variable designators in declaration expressions.
     * This can be either a single variable name or a parenthesized list of designators for deconstruction.
     *
     * @see SingleVariableDesignation
     * @see ParenthesizedVariableDesignation
     */
    interface VariableDesignation : Expression, Cs {
    }

    /**
     * Represents a single variable declaration within a declaration expression.
     * Used both for simple out variable declarations and as elements within deconstruction declarations.
     * Example in out variable:
     * <pre>
     * int.TryParse(s, out int x)  // 'int x' is the SingleVariable
     * </pre>
     * Example in deconstruction:
     * <pre>
     * (int x, string y) = point;  // both 'int x' and 'string y' are SingleVariables
     * </pre>
     */
    
    public final class SingleVariableDesignation {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        Identifier name;
    }

    /**
     * Represents a parenthesized list of variable declarations used in deconstruction patterns.
     * Example of simple deconstruction:
     * <pre>
     * int (x, y) = point;
     * </pre>
     * Example of nested deconstruction:
     * <pre>
     * (int count, var (string name, int age)) = GetPersonDetails();
     *             ^^^^^^^^^^^^^^^^^^^^^^^^^^ nested ParenthesizedVariable
     *  ^^^^^^^^^ SingleVariableDesignation
     * </pre>
     */
    
    public final class ParenthesizedVariableDesignation {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        JContainer<VariableDesignation> variables;

        
        [Nullable]
        JavaType type;
    }

    /**
     * Represents a discard designation in pattern matching expressions, indicated by an underscore (_).
     * For example in pattern matching:
     * <pre>
     *
     * if (obj is _) // discard pattern
     *
     * // Or in deconstruction:
     *
     * var (x, _, z) = tuple; // discards second element
     *
     * </pre>
     */
    
    final class DiscardVariableDesignation {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        Identifier discard;
    }
    //endregion

    /**
     * Represents a tuple expression in C#.
     * Can be used in tuple construction, deconstruction and tuple literals.
     * Examples:
     * <pre>
     * // Tuple construction
     * var point = (1, 2);
     * // Named tuple elements
     * var person = (name: "John", age: 25);
     * // Nested tuples
     * var nested = (1, (2, 3));
     * // Tuple type with multiple elements
     * (string name, int age) person = ("John", 25);
     * </pre>
     */
    
    final class TupleExpression {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        JContainer<Argument> arguments;
    }

    /**
     * Represents a C# constructor declaration which may include an optional constructor initializer.
     * <p>
     * For example:
     * <pre>
     *   // Constructor with no initializer
     *   public MyClass() {
     *   }
     *
     *   // Constructor with base class initializer
     *   public MyClass(int x) : base(x) {
     *   }
     *
     *   // Constructor with this initializer
     *   public MyClass(string s) : this(int.Parse(s)) {
     *   }
     * </pre>
     */
    
    public class Constructor {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        [Nullable]
        ConstructorInitializer initializer;

        
        J.MethodDeclaration constructorCore;
    }

    /**
     * Represents a C# destructor which is a method called before an object is destroyed by the garbage collector.
     * A destructor must be named the same as the class prefixed with a tilde (~), cannot be explicitly called,
     * cannot have parameters or access modifiers, and cannot be overloaded or inherited.
     * <p>
     * For example:
     * <pre>
     *     // Basic destructor
     *     ~MyClass()
     *     {
     *         // Cleanup code
     *     }
     *
     *     // Destructor with cleanup logic
     *     ~ResourceHandler()
     *     {
     *         if (handle != IntPtr.Zero)
     *         {
     *             CloseHandle(handle);
     *         }
     *     }
     *
     *     // Class with both constructor and destructor
     *     public class FileWrapper
     *     {
     *         public FileWrapper()
     *         {
     *             // Initialize
     *         }
     *
     *         ~FileWrapper()
     *         {
     *             // Cleanup
     *         }
     *     }
     * </pre>
     * <p>
     * Note: In modern C#, it's recommended to implement IDisposable pattern instead of relying on destructors
     * for deterministic cleanup of resources, as destructors are non-deterministic and can impact performance.
     */
    
    public class DestructorDeclaration {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        J.MethodDeclaration methodCore;
    }

    
    class Unary {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        JLeftPadded<Type> operator;

        
        Expression expression;

        
        [Nullable]
        JavaType type;

        public enum Type {

            /**
             * Represent x! syntax
             */
            SuppressNullableWarning,
            /**
             * Represent *ptr pointer indirection syntax (get value at pointer)
             */
            PointerIndirection,
            /**
             * Represent int* pointer type
             */
            PointerType,
            /**
             * Represent &a to get pointer access for a variable
             */
            AddressOf,

            /**
             * Represent [..1]
             */
            Spread,
            /**
             * Represent [^3] syntax
             */
            FromEnd;
        }
    }


    /**
     * Represents a constructor initializer which is a call to another constructor, either in the same class (this)
     * or in the base class (base).
     * Examples:
     * <pre>
     * class Person {
     * // Constructor with 'this' initializer
     * public Person(string name) : this(name, 0) { }
     * // Constructor with 'base' initializer
     * public Person(string name, int age) : base(name) { }
     * }
     * </pre>
     */
    
    final class ConstructorInitializer {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        Keyword keyword;

        JContainer<Expression> arguments;
    }

    /**
     * Represents a C# tuple type specification, which allows grouping multiple types into a single type.
     * Can be used in method returns, variable declarations, etc.
     * <p>
     * For example:
     * <pre>
     *   // Simple tuple type
     *   (int, string) coordinates;
     *
     *   // Tuple type with named elements
     *   (int x, string label) namedTuple;
     *
     *   // Nested tuple types
     *   (int, (string, bool)) complexTuple;
     *
     *   // As method return type
     *   public (string name, int age) GetPersonDetails() { }
     *
     *   // As parameter type
     *   public void ProcessData((int id, string value) data) { }
     * </pre>
     */
    
    public class TupleType {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        JContainer<TupleElement> elements;

        
        [Nullable]
        JavaType type;
    }

    /**
     * Represents a single element within a tuple type, which may include an optional
     * identifier for named tuple elements.
     */
    
    public class TupleElement {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        TypeTree type;

        
        [Nullable]
        Identifier name;

    }


    /**
     * Represents a C# new class instantiation expression, which can optionally include an object/collection initializer.
     * <p>
     * For example:
     * <pre>
     * // Simple new class without initializer
     * new Person("John", 25)
     *
     * // New class with object initializer
     * new Person { Name = "John", Age = 25 }
     *
     * // New class with collection initializer
     * new List<int> { 1, 2, 3 }
     *
     * // New class with constructor and initializer
     * new Person("John") { Age = 25 }
     * </pre>
     * The newClassCore field contains the basic class instantiation including constructor call,
     * while the initializer field contains the optional object/collection initializer expressions
     * wrapped in a JContainer to preserve whitespace around curly braces and between initializer expressions.
     */
    
    final class NewClass {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        J.NewClass newClassCore;

        
        [Nullable]
        InitializerExpression initializer;

    }

    /**
     * Represents an initializer expression that consists of a list of expressions, typically used in array
     * or collection initialization contexts. The expressions are contained within delimiters like curly braces.
     * <p>
     * For example:
     * <pre>
     * new int[] { 1, 2, 3 }
     *            ^^^^^^^^^
     * new List<string> { "a", "b", "c" }
     *                   ^^^^^^^^^^^^^^^
     * </pre>
     * The JContainer wrapper captures whitespace before the opening brace, while also preserving whitespace
     * after each expression (before commas) through its internal JRightPadded elements.
     */
    
    final class InitializerExpression {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        JContainer<Expression> expressions;
    }

    /**
     * Represents implicit element access in C# which allows accessing elements without specifying the element accessor target.
     * This is commonly used in object initializers, collection initializers and anonymous object initializers.
     * <p>
     * For example:
     * <pre>
     * // Collection initializer
     * new List<Point> {
     *     { 10, 20 }, // ImplicitElementAccess with two arguments
     *     { 30, 40 }
     * }
     *
     * // Object initializer
     * new Dictionary<string, string> {
     *     { "key1", "value1" }, // ImplicitElementAccess wrapping key-value pair arguments
     *     { "key2", "value2" }
     * }
     * </pre>
     * The argumentList field contains the list of arguments wrapped in braces, with whitespace preserved
     * before the opening brace and between arguments.
     */
    
    final class ImplicitElementAccess {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        JContainer<Argument> argumentList;
    }

    /**
     * Represents a C# yield statement which can either return a value or break from an iterator.
     * <p>
     * For example:
     * <pre>
     *   yield return value;   // Returns next value in iterator
     *   yield break;          // Signals end of iteration
     * </pre>
     */
    
    final class Yield {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        Keyword returnOrBreakKeyword;

        
        [Nullable]
        Expression expression;

    }

    /**
     * An expression that yields the default value of a type.
     * <p>
     * For example:
     * <pre>
     *   default(int)         // Returns 0
     *   default(string)      // Returns null
     *   default(bool)        // Returns false
     *   default(MyClass)     // Returns null
     *   var x = default;     // Type inferred from context (C# 7.1+)
     * </pre>
     */
    
    final class DefaultExpression {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        [Nullable]
        JContainer<TypeTree> typeOperator;
    }

    /**
     * Represents a C# is pattern expression that performs pattern matching.
     * The expression consists of a value to test, followed by the 'is' keyword and a pattern.
     * <p>
     * For example:
     * <pre>
     *     // Type pattern
     *     if (obj is string)
     *
     *     // Type pattern with declaration
     *     if (obj is string str)
     *
     *     // Constant pattern
     *     if (number is 0)
     *
     *     // Property pattern
     *     if (person is { Name: "John", Age: 25 })
     *
     *     // Relational pattern
     *     if (number is > 0)
     *
     *     // Var pattern
     *     if (expr is var result)
     *
     *     // List pattern
     *     if (list is [1, 2, 3])
     * </pre>
     */
    
    final class IsPattern {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        Expression expression;

        JLeftPadded<Pattern> pattern;
    }

    //region Patterns

    /**
     * Base interface for all C# pattern types that can appear on the right-hand side of an 'is' expression.
     * This includes type patterns, constant patterns, declaration patterns, property patterns, etc.
     */
    interface Pattern : Expression, Cs {

    }

    /**
     * Represents a unary pattern in C#, which negates another pattern using the "not" keyword.
     * <p>
     * For example:
     * <pre>
     *     // Using "not" pattern to negate a type pattern
     *     if (obj is not string) { }
     *
     *     // Using "not" pattern with constant pattern
     *     if (value is not 0) { }
     *
     *     // Using "not" pattern with other patterns
     *     switch (obj) {
     *         case not null: // Negates null constant pattern
     *             break;
     *         case not int: // Negates type pattern
     *             break;
     *     }
     * </pre>
     */
    
    final class UnaryPattern {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * a is not b
         *      ^^^
         * </pre>
         */
        
        Keyword operator;

        
        Pattern pattern;
    }

    /**
     * Represents a C# type pattern, which matches a value against a type and optionally assigns it to a new variable.
     * <p>
     * For example:
     * <pre>
     *     // Simple type pattern
     *     if (obj is string)
     *
     *     // Type pattern with variable declaration
     *     if (obj is string str)
     *
     *     // Type pattern with array type
     *     if (obj is int[])
     *
     *     // Type pattern with generic type
     *     if (obj is List&lt;string&gt; stringList)
     *
     *     // Type pattern with nullable type
     *     if (obj is string? nullableStr)
     *
     *     // Switch expression with type pattern
     *     object value = someValue switch {
     *         string s => s.Length,
     *         int n => n * 2,
     *         _ => 0
     *     };
     * </pre>
     */
    
    final class TypePattern {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        TypeTree typeIdentifier;

        [Nullable]
        VariableDesignation designation;

    }

    /**
     * Represents a C# binary pattern that combines two patterns with a logical operator.
     * The binary pattern is used in pattern matching to create compound pattern tests.
     * <p>
     * For example:
     * <pre>
     *     // Using 'and' to combine patterns
     *     if (obj is string { Length: &gt; 0 } and not null)
     *
     *     // Using 'or' to combine patterns
     *     if (number is &gt; 0 or &lt; -10)
     *
     *     // Combining type patterns
     *     if (obj is IList and not string)
     *
     *     // Complex combinations
     *     if (value is &gt;= 0 and &lt;= 100)
     *
     *     // Multiple binary patterns
     *     if (obj is IEnumerable and not string and not int[])
     *
     *     // In switch expressions
     *     return size switch {
     *         &lt; 0 or &gt; 100 =&gt; "Invalid",
     *         &gt;= 0 and &lt;= 50 =&gt; "Small",
     *         _ =&gt; "Large"
     *     };
     * </pre>
     */
    
    final class BinaryPattern {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        Pattern left;

        JLeftPadded<OperatorType> operator;

        
        Pattern right;

        public enum OperatorType {
            And,
            Or
        }
    }

    /**
     * Represents a C# constant pattern that matches against literal values or constant expressions.
     * <p>
     * For example:
     * <pre>
     *     // Literal constant patterns
     *     if (obj is null)
     *     if (number is 42)
     *     if (flag is true)
     *     if (ch is 'A')
     *     if (str is "hello")
     *
     *     // Constant expressions
     *     const int MAX = 100;
     *     if (value is MAX)
     *
     *     // In switch expressions
     *     return value switch {
     *         null => "undefined",
     *         0 => "zero",
     *         1 => "one",
     *         _ => "other"
     *     };
     *
     *     // With other pattern combinations
     *     if (str is not null and "example")
     *
     *     // Enum constant patterns
     *     if (day is DayOfWeek.Sunday)
     * </pre>
     */
    
    final class ConstantPattern {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * if (obj is 42)
         *            ^^
         * </pre>
         */
        
        Expression value;
    }

    /**
     * Represents a C# discard pattern (_), which matches any value and discards it.
     * <p>
     * For example:
     * <pre>
     *     // Simple discard pattern in is expression
     *     if (obj is _)
     *
     *     // In switch expressions
     *     return value switch {
     *         1 => "one",
     *         2 => "two",
     *         _ => "other"    // Discard pattern as default case
     *     };
     *
     *     // With relational patterns
     *     if (value is > 0 and _)
     *
     *     // In property patterns
     *     if (obj is { Id: _, Name: "test" })
     * </pre>
     */
    
    final class DiscardPattern {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        [Nullable]
        JavaType type;
    }

    /**
     * Represents a C# list pattern that matches elements in a list or array against a sequence of patterns.
     * <p>
     * For example:
     * <pre>
     *     // Simple list patterns
     *     if (array is [1, 2, 3] lst)
     *     if (list is [1, _, 3])
     *
     *     // With designation
     *     if (points is [(0, 0), (1, 1)] coords)
     *
     *     // With slices
     *     if (numbers is [1, .., 5] sequence)
     *     if (values is [1, 2, .., 8, 9] arr)
     *
     *     // With subpatterns
     *     if (points is [(0, 0), (1, 1)])
     *
     *     // With type patterns
     *     if (list is [int i, string s] result)
     *
     *     // In switch expressions
     *     return array switch {
     *         [var first, _] arr => arr.Length,
     *         [1, 2, ..] seq => "starts with 1,2",
     *         [] empty => "empty",
     *         _ => "other"
     *     };
     *
     *     // With length patterns
     *     if (array is [> 0, <= 10] valid)
     * </pre>
     */
    
    final class ListPattern {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * if (array is [1, 2, 3] lst)
         *              ^^^^^^^^^
         * </pre>
         */
        JContainer<Pattern> patterns;

        /**
         * <pre>
         * if (array is [1, 2, 3] lst)
         *                        ^^^
         * </pre>
         */
        
        [Nullable]
        VariableDesignation designation;
    }

    /**
     * Represents a C# parenthesized pattern expression that groups a nested pattern.
     * <p>
     * For example:
     * <pre>
     *     // Simple parenthesized pattern
     *     if (obj is (string or int))
     *
     *     // With nested patterns
     *     if (obj is not (null or ""))
     *
     *     // In complex pattern combinations
     *     if (value is > 0 and (int or double))
     *
     *     // In switch expressions
     *     return value switch {
     *         (> 0 and < 10) => "single digit",
     *         (string or int) => "basic type",
     *         _ => "other"
     *     };
     * </pre>
     */
    
    final class ParenthesizedPattern {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * if (obj is (string or int))
         *            ^^^^^^^^^^^^^^^
         * </pre>
         */
        JContainer<Pattern> pattern;
    }

    /**
     * Represents a C# recursive pattern that can match nested object structures, including property patterns and positional patterns.
     * <p>
     * For example:
     * <pre>
     *     // Simple property pattern
     *     if (obj is { Name: "test", Age: > 18 })
     *
     *     // With type pattern
     *     if (obj is Person { Name: "test" } p)
     *
     *     // With nested patterns
     *     if (obj is { Address: { City: "NY" } })
     *
     *     // Positional patterns (deconstructions)
     *     if (point is (int x, int y) { x: > 0, y: > 0 })
     *
     *     // With variable designation
     *     if (obj is { Id: int id, Name: string name } result)
     *
     *     // In switch expressions
     *     return shape switch {
     *         Circle { Radius: var r } => Math.PI * r * r,
     *         Rectangle { Width: var w, Height: var h } => w * h,
     *         _ => 0
     *     };
     * </pre>
     */
    
    final class RecursivePattern {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * if (obj is Person { Name: "test" })
         *            ^^^^^^
         * </pre>
         */
        
        [Nullable]
        TypeTree typeQualifier;

        /**
         * <pre>
         * if (point is (int x, int y))
         *              ^^^^^^^^^^^^^^
         * </pre>
         */
        
        [Nullable]
        PositionalPatternClause positionalPattern;

        /**
         * <pre>
         * if (obj is { Name: "test", Age: 18 })
         *            ^^^^^^^^^^^^^^^^^^^^^^^^^
         * </pre>
         */
        
        [Nullable]
        PropertyPatternClause propertyPattern;

        /**
         * <pre>
         * if (obj is Person { Name: "test" } p)
         *                                    ^
         * </pre>
         */
        
        [Nullable]
        VariableDesignation designation;
    }

    /**
     * Represents a var pattern that is used in switch statement pattern matching.
     * <pre>
     * case var (x, y):
     *      ^^^
     * </pre>
     */
    
    final class VarPattern {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * case var (x, y):
         *          ^^^^^^
         * </pre>
         */
        
        VariableDesignation designation;
    }

    /**
     * Represents a positional pattern clause in C# pattern matching, which matches the deconstructed parts of an object.
     * <p>
     * For example:
     * <pre>
     *     // Simple positional pattern
     *     if (point is (0, 0))
     *
     *     // With variable declarations
     *     if (point is (int x, int y))
     *
     *     // With nested patterns
     *     if (point is (> 0, < 100))
     *
     *     // In switch expressions
     *     return point switch {
     *         (0, 0) => "origin",
     *         (var x, var y) when x == y => "on diagonal",
     *         _ => "other"
     *     };
     * </pre>
     */
    
    final class PositionalPatternClause {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * if (point is (0, 0))
         *              ^^^^^^
         * </pre>
         */
        JContainer<Subpattern> subpatterns;
    }

    /**
     * Represents a C# relational pattern that matches values using comparison operators.
     * <p>
     * For example:
     * <pre>
     *     // Simple relational patterns
     *     if (number is > 0)
     *     if (value is <= 100)
     *
     *     // In switch expressions
     *     return size switch {
     *         > 100 => "Large",
     *         < 0 => "Invalid",
     *         _ => "Normal"
     *     };
     *
     *     // Combined with other patterns
     *     if (x is > 0 and < 100)
     *
     *     // With properties
     *     if (person is { Age: >= 18 })
     * </pre>
     */
    
    final class RelationalPattern {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * if (number is > 100)
         *               ^
         * </pre>
         */
        JLeftPadded<OperatorType> operator;

        /**
         * <pre>
         * if (number is > 100)
         *                 ^^^
         * </pre>
         */
        
        Expression value;

        public enum OperatorType {
            LessThan,
            LessThanOrEqual,
            GreaterThan,
            GreaterThanOrEqual
        }
    }

    /**
     * Represents a C# slice pattern that matches sequences with arbitrary elements between fixed elements.
     * <p>
     * For example:
     * <pre>
     *     // Simple slice pattern
     *     if (array is [1, .., 5])
     *
     *     // Multiple elements before and after
     *     if (array is [1, 2, .., 8, 9])
     *
     *     // Just prefix elements
     *     if (array is [1, 2, ..])
     *
     *     // Just suffix elements
     *     if (array is [.., 8, 9])
     *
     *     // In switch expressions
     *     return array switch {
     *         [var first, .., var last] => $"{first}..{last}",
     *         [var single] => single.ToString(),
     *         [] => "empty"
     *     };
     * </pre>
     */
    
    final class SlicePattern {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;

    }

    /**
     * Represents a property pattern clause in C# pattern matching, which matches against object properties.
     * <p>
     * For example:
     * <pre>
     *     // Simple property pattern
     *     if (obj is { Name: "test" })
     *
     *     // Multiple properties
     *     if (person is { Name: "John", Age: > 18 })
     *
     *     // Nested property patterns
     *     if (order is { Customer: { Name: "test" } })
     *
     *     // With variable declarations
     *     if (person is { Id: int id, Name: string name })
     *
     *     // In switch expressions
     *     return shape switch {
     *         { Type: "circle", Radius: var r } => Math.PI * r * r,
     *         { Type: "square", Side: var s } => s * s,
     *         _ => 0
     *     };
     * </pre>
     */
    
    final class PropertyPatternClause {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * if (obj is { Name: "test", Age: 18 })
         *            ^^^^^^^^^^^^^^^^^^^^^^^^^
         * </pre>
         */
        JContainer<Expression> subpatterns;
    }


    /**
     * Represents a subpattern in C# pattern matching, which can appear in property patterns or positional patterns.
     * Each subpattern consists of an optional name with a corresponding pattern.
     * <p>
     * For example:
     * <pre>
     *     // In property patterns
     *     if (obj is { Name: "test", Age: > 18 })
     *                  ^^^^^^^^^^^^  ^^^^^^^^^
     *
     *     // In positional patterns
     *     if (point is (x: > 0, y: > 0))
     *                   ^^^^^^  ^^^^^^
     *
     *     // With variable declarations
     *     if (person is { Id: var id, Name: string name })
     *                     ^^^^^^^^^^  ^^^^^^^^^^^^^^^^^
     *
     *     // Nested patterns
     *     if (obj is { Address: { City: "NY" } })
     *                  ^^^^^^^^^^^^^^^^^^^^^^^
     *
     *     // In switch expressions
     *     return shape switch {
     *         { Radius: var r } => Math.PI * r * r,
     *           ^^^^^^^^^^^
     *         { Width: var w, Height: var h } => w * h,
     *           ^^^^^^^^^^^^  ^^^^^^^^^^^^^
     *         _ => 0
     *     };
     * </pre>
     */
    
    final class Subpattern {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * if (obj is { Name: "test" })
         *               ^^^^
         * if (point is (x: > 0))
         *               ^
         * </pre>
         */
        
        [Nullable]
        Expression name;

        /**
         * <pre>
         * if (obj is { Name: "test" })
         *                    ^^^^^
         * if (point is (x: > 0))
         *                  ^^
         * </pre>
         */
        JLeftPadded<Pattern> pattern;
    }
    //endregion

    /**
     * Represents a C# switch expression which provides a concise way to handle multiple patterns with corresponding expressions.
     * <p>
     * For example:
     * <pre>
     * var description = size switch {
     *     < 0 => "negative",
     *     0 => "zero",
     *     > 0 => "positive"
     * };
     *
     * var color = (r, g, b) switch {
     *     var (r, g, b) when r == g && g == b => "grayscale",
     *     ( > 128, _, _) => "bright red",
     *     _ => "other"
     * };
     * </pre>
     */
    
    final class SwitchExpression {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * size switch { ... }
         * ^^^^
         * </pre>
         */
        JRightPadded<Expression> expression;

        /**
         * <pre>
         * size switch { ... }
         *             ^^^^^
         * </pre>
         */
        JContainer<SwitchExpressionArm> arms;
    }

    /**
     * Represents a single case arm in a switch expression, consisting of a pattern, optional when clause, and result expression.
     * <p>
     * For example:
     * <pre>
     * case < 0 when IsValid() => "negative",
     * > 0 => "positive",
     * _ => "zero"
     *
     * // With complex patterns and conditions
     * (age, role) switch {
     *     ( > 21, "admin") when HasPermission() => "full access",
     *     ( > 18, _) => "basic access",
     *     _ => "no access"
     * }
     * </pre>
     */
    
    final class SwitchExpressionArm {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * < 0 when IsValid() => "negative"
         * ^^^
         * </pre>
         */
        
        Pattern pattern;

        /**
         * <pre>
         * < 0 when IsValid() => "negative"
         *     ^^^^^^^^^^^^^^
         * </pre>
         */
        [Nullable]
        JLeftPadded<Expression> whenExpression;

        /**
         * <pre>
         * < 0 when IsValid() => "negative"
         *                       ^^^^^^^^^^
         * </pre>
         */
        JLeftPadded<Expression> expression;
    }

    /**
     * Represents a switch statement section containing one or more case labels followed by a list of statements.
     * <p>
     * For example:
     * <pre>
     * switch(value) {
     *     case 1:                    // single case label
     *     case 2:                    // multiple case labels
     *         Console.WriteLine("1 or 2");
     *         break;
     *
     *     case int n when n > 0:     // pattern case with when clause
     *         Console.WriteLine("positive");
     *         break;
     *
     *     case Person { Age: > 18 }: // recursive pattern
     *         Console.WriteLine("adult");
     *         break;
     *
     *     default:                   // default label
     *         Console.WriteLine("default");
     *         break;
     * }
     * </pre>
     */
    
    final class SwitchSection {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * case 1:
         * case 2:
         * ^^^^^^^
         * </pre>
         */
        
        List<SwitchLabel> labels;

        /**
         * <pre>
         * case 1:
         *     Console.WriteLine("1");
         *     break;
         *     ^^^^^^^^^^^^^^^^^^^^
         * </pre>
         */
        List<JRightPadded<Statement>> statements;
    }

    public interface SwitchLabel : Expression {

    }

    /**
     * Represents a default case label in a switch statement.
     * <p>
     * For example:
     * <pre>
     * switch(value) {
     *     case 1:
     *         break;
     *     default:      // default label
     *         Console.WriteLine("default");
     *         break;
     * }
     *
     * // Also used in switch expressions
     * var result = value switch {
     *     1 => "one",
     *     default => "other"
     * };
     * </pre>
     */
    
    final class DefaultSwitchLabel {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * default:
         *        ^
         * </pre>
         */
        
        Space colonToken;
    }

    /**
     * Represents a pattern-based case label in a switch statement, optionally including a when clause.
     * <p>
     * For example:
     * <pre>
     * switch(obj) {
     *     case int n when n > 0:
     *     case string s when s.Length > 0:
     *     case [] when IsValid():
     *     case Person { Age: > 18 }:
     *     case not null:
     *     case > 100:
     * }
     * </pre>
     */
    
    final class CasePatternSwitchLabel {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * case int n when n > 0:
         *      ^^^^^
         * </pre>
         */
        
        Pattern pattern;


        /**
         * <pre>
         * case int n when n > 0:
         *            ^^^^^^^^^^
         * </pre>
         */
        [Nullable]
        JLeftPadded<Expression> whenClause;

        /**
         * <pre>
         * case int n when n > 0 :
         *                      ^
         * </pre>
         */
        
        Space colonToken;
    }

    /**
     * Represents a C# switch statement for control flow based on pattern matching and case labels.
     * <p>
     * For example:
     * <pre>
     * switch(value) {
     *     case 1:
     *         Console.WriteLine("one");
     *         break;
     *
     *     case int n when n > 0:
     *         Console.WriteLine("positive");
     *         break;
     *
     *     case Person { Age: > 18 }:
     *         Console.WriteLine("adult");
     *         break;
     *
     *     case string s:
     *         Console.WriteLine($"string: {s}");
     *         break;
     *
     *     default:
     *         Console.WriteLine("default");
     *         break;
     * }
     * </pre>
     */
    
    final class SwitchStatement {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * switch(value) {
         *       ^^^^^^
         * </pre>
         */
        JContainer<Expression> expression;

        /**
         * <pre>
         * switch(value) {
         *     case 1:
         *         Console.WriteLine("one");
         *         break;
         *     default:
         *         Console.WriteLine("default");
         *         break;
         * }
         * ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
         * </pre>
         */
        JContainer<SwitchSection> sections;
    }

    /**
     * Represents a C# lock statement which provides thread synchronization.
     * <p>
     * For example:
     * <pre>
     *     // Simple lock statement
     *     lock (syncObject) {
     *         // protected code
     *     }
     *
     *     // Lock with local variable
     *     lock (this.lockObj) {
     *         sharedResource.Modify();
     *     }
     *
     *     // Lock with property
     *     lock (SyncRoot) {
     *         // thread-safe operations
     *     }
     * </pre>
     */
    
    final class LockStatement {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * lock (syncObject) { }
         *      ^^^^^^^^^^
         * </pre>
         */
        
        J.ControlParentheses<Expression> expression;

        /**
         * <pre>
         * lock (syncObject) { }
         *                  ^^^^^
         * </pre>
         */
        JRightPadded<Statement> statement;
    }

    /**
     * Represents a C# fixed statement which pins a moveable variable at a memory location.
     * The fixed statement prevents the garbage collector from relocating a movable variable
     * and declares a pointer to that variable.
     * <p>
     * For example:
     * <pre>
     *     // Fixed statement with array
     *     fixed (int* p = array) {
     *         // use p
     *     }
     *
     *     // Fixed statement with string
     *     fixed (char* p = str) {
     *         // use p
     *     }
     *
     *     // Multiple pointers in one fixed statement
     *     fixed (byte* p1 = &b1, p2 = &b2) {
     *         // use p1 and p2
     *     }
     *
     *     // Fixed statement with custom type
     *     fixed (CustomStruct* ptr = &struct) {
     *         // use ptr
     *     }
     * </pre>
     */
    
    final class FixedStatement {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * fixed (int* p = array) { }
         *       ^^^^^^^^^^^^^^
         * </pre>
         */
        
        ControlParentheses<J.VariableDeclarations> declarations;

        /**
         * <pre>
         * fixed (int* p = array) { }
         *                       ^^^^^
         *
         * or
         *
         * fixed (int* p = array)
         *  return p;
         *  ^^^^^^^^^
         * </pre>
         */
        
        Block block;
    }
    /**
     * Represents a C# checked or unchecked expression which controls overflow checking behavior.
     * <p>
     * For example:
     * <pre>
     *     // Checked expression
     *     int result = checked(x + y);
     *
     *     // Unchecked expression
     *     int value = unchecked(a * b);
     *
     * </pre>
     */
    
    final class CheckedExpression {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * checked(x + y)
         * ^^^^^^^
         * </pre>
         */
        
        Keyword checkedOrUncheckedKeyword;

        /**
         * <pre>
         * checked(x + y)
         *       ^^^^^^^
         * </pre>
         */
        
        ControlParentheses<Expression> expression;
    }
    /**
     * Represents a C# checked statement which enforces overflow checking for arithmetic operations
     * and conversions. Operations within a checked block will throw OverflowException if arithmetic
     * overflow occurs.
     * <p>
     * For example:
     * <pre>
     *     // Basic checked block
     *     checked {
     *         int result = int.MaxValue + 1; // throws OverflowException
     *     }
     *
     *     // Checked with multiple operations
     *     checked {
     *         int a = int.MaxValue;
     *         int b = a + 1;     // throws OverflowException
     *         short s = (short)a; // throws OverflowException if out of range
     *     }
     *
     *     // Nested arithmetic operations
     *     checked {
     *         int result = Math.Abs(int.MinValue); // throws OverflowException
     *     }
     * </pre>
     */
    
    final class CheckedStatement {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        Keyword keyword;

        /**
         * <pre>
         * checked {
         *         ^^^^^^^^^
         * }
         * ^
         * </pre>
         */
        
        J.Block block;
    }

    /**
     * Represents a C# unsafe statement block which allows direct memory manipulation and pointer operations.
     * Code within an unsafe block can perform operations like pointer arithmetic, fixed-size buffers,
     * and direct memory access.
     * <p>
     * For example:
     * <pre>
     *     // Basic unsafe block
     *     unsafe {
     *         int* ptr = &value;
     *     }
     *
     *     // Unsafe with pointer operations
     *     unsafe {
     *         int* p1 = &x;
     *         int* p2 = p1 + 1;
     *         *p2 = 100;
     *     }
     *
     *     // Unsafe with fixed buffers
     *     unsafe {
     *         fixed (byte* ptr = bytes) {
     *             // Direct memory access
     *         }
     *     }
     *
     *     // Unsafe with sizeof operations
     *     unsafe {
     *         int size = sizeof(CustomStruct);
     *         byte* buffer = stackalloc byte[size];
     *     }
     * </pre>
     */
    
    final class UnsafeStatement {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * unsafe {
         *        ^^^^^^^^^
         * }
         * ^
         * </pre>
         */
        
        J.Block block;
    }

    /**
     * Represents a C# range expression which creates a Range value representing a sequence of indices.
     * Range expressions use the '..' operator to specify start and end bounds, and can use '^' to specify
     * indices from the end.
     * <p>
     * For example:
     * <pre>
     *     // Full range
     *     arr[..]
     *
     *     // Range with start index
     *     arr[2..]
     *
     *     // Range with end index
     *     arr[..5]
     *
     *     // Range with both indices
     *     arr[2..5]
     *
     *     // Range with end-relative indices using '^'
     *     arr[..^1]     // excludes last element
     *     arr[1..^1]    // from index 1 to last-1
     *     arr[^2..^1]   // second-to-last to last-but-one
     *
     *     // Standalone range expressions
     *     Range r1 = 1..4;
     *     Range r2 = ..^1;
     * </pre>
     */
    
    final class RangeExpression {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * 2  ..5
         * ^^^
         * </pre>
         */
        [Nullable]
        JRightPadded<Expression> start;

        /**
         * <pre>
         * 2..5
         *   ^
         * </pre>
         */
        
        [Nullable]
        Expression end;
    }


    /**
     * Represents a C# LINQ query expression that provides SQL-like syntax for working with collections.
     * <p>
     * For example:
     * <pre>
     *     // Simple query
     *     from user in users
     *     where user.Age > 18
     *     select user.Name
     *
     *     // Query with multiple clauses
     *     from c in customers
     *     join o in orders on c.Id equals o.CustomerId
     *     where o.Total > 1000
     *     orderby o.Date
     *     select new { c.Name, o.Total }
     *
     *     // Query with multiple from clauses
     *     from c in customers
     *     from o in c.Orders
     *     where o.Total > 1000
     *     select new { c.Name, o.Total }
     * </pre>
     */
    
    final class QueryExpression {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * from user in users
         * ^^^^^^^^^^^^^^^^^^
         * </pre>
         */
        
        FromClause fromClause;

        /**
         * <pre>
         * from user in users
         * where user.Age > 18
         * select user.Name
         * ^^^^^^^^^^^^^^^^^ excluding the from clause
         * </pre>
         */
        
        QueryBody body;
    }


    public interface SelectOrGroupClause : Cs
    {

    }

    /**
     * Represents the body of a LINQ query expression, consisting of the query clauses and a final select or group clause.
     * <p>
     * For example:
     * <pre>
     *     // Body of query includes everything after initial 'from':
     *     from c in customers
     *     where c.Age > 18       // Clauses part
     *     orderby c.LastName     // Clauses part
     *     select c.Name          // SelectOrGroup part
     *     into oldCustomers      // Continuation part
     *     where oldCustomers...
     *
     *     // Another example with join:
     *     from o in orders
     *     join c in customers    // Clauses part
     *         on o.CustomerId equals c.Id
     *     where o.Total > 1000   // Clauses part
     *     select new { o, c }    // SelectOrGroup part
     * </pre>
     */
    
    final class QueryBody {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * from c in customers
         * where c.Age > 18
         * ^^^^^^^^^^^^^^^^
         * orderby c.LastName
         * ^^^^^^^^^^^^^^^^^^
         * select c.Name
         * </pre>
         */
        
        List<QueryClause> clauses;

        /**
         * <pre>
         * from c in customers
         * where c.Age > 18
         * select c.Name
         * ^^^^^^^^^^^^^ the final select or group clause
         * </pre>
         */
        
        [Nullable]
        SelectOrGroupClause selectOrGroup;

        /**
         * <pre>
         * from c in customers
         * select c
         * into temp            // Continuation starts here
         * where temp.Age > 18
         * select temp.Name
         * </pre>
         */
        
        [Nullable]
        QueryContinuation continuation;
    }

    interface QueryClause : Cs
    {

    }
    /**
     * Represents a LINQ from clause that introduces a range variable and its source collection.
     * This is typically the initial clause of a LINQ query.
     * <p>
     * For example:
     * <pre>
     *     // Simple from clause
     *     from user in users
     *
     *     // With type
     *     from Customer c in customers
     *
     *     // With pattern match
     *     from (x, y) in points
     *
     *     // With type and pattern
     *     from (int x, int y) in coordinates
     * </pre>
     */
    
    final class FromClause {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * from Customer c in customers
         *     ^^^^^^^^^
         * </pre>
         */
        
        [Nullable]
        TypeTree typeIdentifier;

        /**
         * <pre>
         * from Customer c in customers
         *              ^^
         * </pre>
         */
        JRightPadded<Identifier> identifier;

        /**
         * <pre>
         * from user in users
         *             ^^^^^^
         * </pre>
         */
        
        Expression expression;
    }
    /**
     * Represents a let clause in a C# LINQ query expression that introduces
     * a new range variable based on a computation.
     * <p>
     * For example:
     * <pre>
     *     // Simple let clause
     *     from n in numbers
     *     let square = n * n
     *     select square
     *
     *     // Multiple let clauses
     *     from s in strings
     *     let length = s.Length
     *     let upperCase = s.ToUpper()
     *     select new { s, length, upperCase }
     *
     *     // Let with complex expressions
     *     from p in people
     *     let fullName = p.FirstName + " " + p.LastName
     *     let age = DateTime.Now.Year - p.BirthYear
     *     select new { fullName, age }
     * </pre>
     */
    
    final class LetClause {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * let square = n * n
         *    ^^^^^^^^^
         * </pre>
         */
        JRightPadded<J.Identifier> identifier;

        /**
         * <pre>
         * let square = n * n
         *             ^^^^^^
         * </pre>
         */
        
        Expression expression;
    }

    /**
     * Represents a C# join clause in a LINQ query expression.
     * <p>
     * For example:
     * <pre>
     * // Simple join
     * join customer in customers on order.CustomerId equals customer.Id
     *
     * // Join with into (group join)
     * join category in categories
     *   on product.CategoryId equals category.Id
     *   into productCategories
     *
     * // Multiple joins
     * from order in orders
     * join customer in customers
     *   on order.CustomerId equals customer.Id
     * join employee in employees
     *   on order.EmployeeId equals employee.Id
     * </pre>
     */
    
    final class JoinClause {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * join customer in customers
         *     ^^^^^^^^^^^^
         * </pre>
         */
        JRightPadded<Identifier> identifier;

        /**
         * <pre>
         * join customer in customers on order.CustomerId equals customer.Id
         *                 ^^^^^^^^^^^^^
         * </pre>
         */
        JRightPadded<Expression> inExpression;

        /**
         * <pre>
         * join customer in customers on order.CustomerId equals customer.Id
         *                              ^^^^^^^^^^^^^^^^^^^^^^^^
         * </pre>
         */
        JRightPadded<Expression> leftExpression;

        /**
         * <pre>
         * join customer in customers on order.CustomerId equals customer.Id
         *                                                      ^^^^^^^^^^^^
         * </pre>
         */
        
        Expression rightExpression;

        /**
         * <pre>
         * join category in categories on product.CategoryId equals category.Id into productCategories
         *                                                                     ^^^^^^^^^^^^^^^^^^^^^^^
         * </pre>
         */
        [Nullable]
        JLeftPadded<JoinIntoClause> into;
    }

    /**
     * Represents the 'into' portion of a group join clause in C# LINQ syntax.
     * Used to specify the identifier that will hold the grouped results.
     * <p>
     * For example:
     * <pre>
     * // Group join using into clause
     * join category in categories
     *    on product.CategoryId equals category.Id
     *    into productCategories
     *
     * // Multiple group joins
     * join orders in db.Orders
     *    on customer.Id equals orders.CustomerId
     *    into customerOrders
     * join returns in db.Returns
     *    on customer.Id equals returns.CustomerId
     *    into customerReturns
     * </pre>
     */
    
    final class JoinIntoClause {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * into productCategories
         *     ^^^^^^^^^^^^^^^^^^
         * </pre>
         */
        
        Identifier identifier;
    }

    /**
     * Represents a C# LINQ where clause that filters elements in a query based on a condition.
     * <p>
     * For example:
     * <pre>
     *     // Simple where clause
     *     from p in people
     *     where p.Age >= 18
     *     select p
     *
     *     // Multiple where clauses
     *     from p in people
     *     where p.Age >= 18
     *     where p.Name.StartsWith("J")
     *     select p
     *
     *     // Where with complex condition
     *     from o in orders
     *     where o.Total > 1000 && o.Status == "Pending"
     *     select o
     * </pre>
     */
    
    final class WhereClause {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * where p.Age >= 18
         *      ^^^^^^^^^^^^
         * </pre>
         */
        
        Expression condition;
    }

    /**
     * Represents a C# LINQ orderby clause that specifies the ordering of results in a query.
     * <p>
     * For example:
     * <pre>
     *     // Simple orderby with single key
     *     from p in people
     *     orderby p.LastName
     *     select p
     *
     *     // Multiple orderings
     *     from p in people
     *     orderby p.LastName ascending, p.FirstName descending
     *     select p
     *
     *     // Orderby with complex key expressions
     *     from o in orders
     *     orderby o.Customer.Name, o.Total * 1.08
     *     select o
     * </pre>
     */
    
    final class OrderByClause {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * orderby p.LastName ascending, p.FirstName descending
         *         ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
         * </pre>
         */
        List<JRightPadded<Ordering>> orderings;
    }

    /**
     * Represents a LINQ query continuation using the 'into' keyword, which allows query results to be
     * further processed in subsequent query clauses.
     * <p>
     * For example:
     * <pre>
     *     // Query continuation with grouping
     *     from c in customers
     *     group c by c.Country into g
     *     select new { Country = g.Key, Count = g.Count() }
     *
     *     // Multiple continuations
     *     from n in numbers
     *     group n by n % 2 into g
     *     select new { Modulo = g.Key, Items = g } into r
     *     where r.Items.Count() > 2
     *     select r
     * </pre>
     */
    
    final class QueryContinuation {
        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * group c by c.Country into g
         *                         ^^^
         * </pre>
         */
        
        J.Identifier identifier;

        /**
         * <pre>
         * group c by c.Country into g
         * select new { Country = g.Key }
         * ^^^^^^^^^^^^^^^^^^^^^^^^^^^^
         * </pre>
         */
        
        QueryBody body;
    }


    /**
     * Represents a single ordering clause within C# orderby expression.
     * <pre>
     * orderby name ascending
     * orderby age descending, name ascending
     * </pre>
     */
    
    final class Ordering {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * orderby name ascending
         *        ^^^^
         * </pre>
         */
        JRightPadded<Expression> expression;

        /**
         * <pre>
         * orderby name ascending
         *             ^^^^^^^^^
         * </pre>
         */
        
        [Nullable]
        DirectionKind direction;

        public enum DirectionKind {
            Ascending,
            Descending
        }
    }

    /**
     * Represents a select clause in a LINQ expression in C#.
     * <pre>
     * // Simple select
     * select item
     *
     * // Select with projection
     * select new { Name = p.Name, Age = p.Age }
     * </pre>
     */
    
    final class SelectClause {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * select item
         *        ^^^^
         * </pre>
         */
        
        Expression expression;
    }

    /**
     * Represents a group clause in a LINQ query.
     * <pre>
     * // Simple group by
     * group item by key
     *
     * // Group by with complex key
     * group customer by new { customer.State, customer.City }
     * </pre>
     */
    
    final class GroupClause {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * group item by key
         *       ^^^^
         * </pre>
         */
        JRightPadded<Expression> groupExpression;

        /**
         * <pre>
         * group item by key
         *              ^^^
         * </pre>
         */
        
        Expression key;
    }
    /**
     * Represents a C# indexer declaration which allows objects to be indexed like arrays.
     * <pre>
     * // Simple indexer
     * public int this[int index] { get { } set { } }
     *
     * // Indexer with multiple parameters
     * public string this[int x, int y] { get; set; }
     *
     * // Readonly indexer
     * public MyType this[string key] { get; }
     *
     * // Interface indexer
     * string this[int index] { get; set; }
     *
     * // Protected indexer with expression body
     * protected internal int this[int i] =&gt; array[i];
     * </pre>
     */
    
    final class IndexerDeclaration {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        List<Modifier> modifiers;

        /**
         * <pre>
         * public int this[int index]
         *        ^^^
         * </pre>
         */
        
        TypeTree typeExpression;

        /**
         * <pre>
         * public int IFoo.this[int index]
         *          ^^^^^
         * </pre>
         */
        [Nullable]
        JRightPadded<TypeTree> explicitInterfaceSpecifier;

        /**
         * <pre>
         * public TypeName ISomeType.this[int index]
         *                          ^^^^
         * </pre>
         * Either FieldAccess (when interface qualified) or Identifier ("this")
         */
        
        Expression indexer;

        /**
         * <pre>
         * public int this[int index] { get; set; }
         *               ^^^^^^^^^^
         * </pre>
         */
        JContainer<Expression> parameters;

        /**
         * <pre>
         * public int this[int index] => array[index];
         *                            ^^^^^^^^^^^^^^^^
         * </pre>
         */
        [Nullable]
        JLeftPadded<Expression> expressionBody;

        /**
         * <pre>
         * public int this[int index] { get; set; }
         *                           ^^^^^^^^^^^^
         * </pre>
         */
        [Nullable]
        Block accessors;
    }


    /**
     * Represents a C# delegate declaration which defines a type that can reference methods.
     * Delegates act as type-safe function pointers and provide the foundation for events in C#.
     * <p>
     * For example:
     * <pre>
     * // Simple non-generic delegate with single parameter
     * public delegate void Logger(string message);
     *
     * // Generic delegate
     * public delegate T Factory<T>() where T : class, new();
     *
     * // Delegate with multiple parameters and constraint
     * public delegate TResult Convert<T, TResult>(T input)
     *     where T : struct
     *     where TResult : class;
     *
     * // Static delegate (C# 11+)
     * public static delegate int StaticHandler(string msg);
     *
     * // Protected access
     * protected delegate bool Validator<T>(T item);
     * </pre>
     */
    
    final class DelegateDeclaration {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        List<AttributeList> attributes;

        /**
         * <pre>
         * public delegate void MyDelegate(string message);
         * ^^^^^^
         * </pre>
         */
        
        List<Modifier> modifiers;

        /**
         * <pre>
         * public delegate void MyDelegate(string message);
         *               ^^^^
         * </pre>
         */
        JLeftPadded<TypeTree> returnType;

        /**
         * <pre>
         * public delegate void MyDelegate(string message);
         *                     ^^^^^^^^^^^
         * </pre>
         */
        
        Identifier identifier;

        /**
         * <pre>
         * public delegate T GenericDelegate<T>(T item);
         *                                  ^^^
         * </pre>
         */
        [Nullable]
        JContainer<Cs.TypeParameter> typeParameters;

        /**
         * <pre>
         * public delegate void MyDelegate(string message);
         *                                ^^^^^^^^^^^^^^^^
         * </pre>
         */
        JContainer<Statement> parameters;

        /**
         * <pre>
         * public delegate T Factory<T>() where T : class;
         *                               ^^^^^^^^^^^^^^^^
         * </pre>
         */
        [Nullable]
        JContainer<TypeParameterConstraintClause> typeParameterConstraintClauses;
    }

    /**
     * Represents a C# operator conversion declaration that defines custom type conversion behavior.
     * <pre>
     * // Implicit conversion
     * public static implicit operator string(MyType t) =&gt; t.ToString();
     *
     * // Explicit conversion
     * public static explicit operator int(MyType t) { return t.Value; }
     *
     * // With expression body
     * public static explicit operator double(MyType t) =&gt; t.Value;
     *
     * // With block body
     * public static implicit operator bool(MyType t) {
     *     return t.Value != 0;
     * }
     */
    
    final class ConversionOperatorDeclaration {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * public static implicit operator string(MyType t)
         * ^^^^^^^^^^^^^
         * </pre>
         */
        
        List<Modifier> modifiers;

        /**
         * <pre>
         * public static implicit operator string(MyType t)
         *               ^^^^^^^^
         * </pre>
         */
        JLeftPadded<ExplicitImplicit> kind;

        /**
         * <pre>
         * public static implicit operator string(MyType t)
         *                                ^^^^^^^
         * </pre>
         */
        JLeftPadded<TypeTree> returnType;

        /**
         * <pre>
         * public static implicit operator string(MyType t)
         *                                      ^^^^^^^^^
         * </pre>
         */
        JContainer<Statement> parameters;

        /**
         * <pre>
         * public static implicit operator string(MyType t) => t.ToString();
         *                                                 ^^^^^^^^^^^^^^^
         * </pre>
         */
        [Nullable]
        JLeftPadded<Expression> expressionBody;

        /**
         * <pre>
         * public static implicit operator string(MyType t) { return t.ToString(); }
         *                                                 ^^^^^^^^^^^^^^^^^^^^^^^
         * </pre>
         */
        
        [Nullable]
        Block body;

        public enum ExplicitImplicit {
            Implicit,
            Explicit
        }
    }

    /**
     * Represents a C# type parameter in generic type declarations, including optional variance and constraints.
     * <p>
     * For example:
     * <pre>
     *     // Simple type parameter
     *     class Container&lt;T&gt;
     *
     *     // Type parameter with variance
     *     interface IEnumerable&lt;out T&gt;
     *
     *     // Type parameter with attributes
     *     class Handler&lt;[Category("A")] T&gt;
     *
     *     // Type parameter with variance and attributes
     *     interface IComparer&lt;[NotNull] in T&gt;
     * </pre>
     */
    
    final class TypeParameter {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        List<AttributeList> attributeLists;

        /**
         * <pre>
         * interface IEnumerable<out T>
         *                      ^^^
         * </pre>
         */
        [Nullable]
        JLeftPadded<VarianceKind> variance;

        /**
         * <pre>
         * class Container<T>
         *                 ^
         * </pre>
         */
        
        Identifier name;

        public enum VarianceKind {
            In,
            Out
        }
    }

    /**
     * Represents a C# enum declaration, including optional modifiers, attributes, and enum members.
     * <p>
     * For example:
     * <pre>
     *     // Simple enum
     *     public enum Colors { Red, Green, Blue }
     *
     *     // Enum with base type
     *     enum Flags : byte { None, All }
     *
     *     // Enum with attributes and explicit values
     *     [Flags]
     *     internal enum Permissions {
     *         None = 0,
     *         Read = 1,
     *         Write = 2,
     *         ReadWrite = Read | Write
     *     }
     * </pre>
     */
    
    final class EnumDeclaration {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        [Nullable]
        List<AttributeList> attributeLists;

        
        List<Modifier> modifiers;

        /**
         * <pre>
         * public enum Colors { Red, Green }
         *            ^^^^^^
         * </pre>
         */
        JLeftPadded<Identifier> name;

        /**
         * <pre>
         * enum Flags : byte { None }
         *           ^^^^^^^
         * </pre>
         */
        [Nullable]
        JLeftPadded<TypeTree> baseType;

        /**
         * <pre>
         * enum Colors { Red, Green, Blue }
         *             ^^^^^^^^^^^^^^^^^^
         * </pre>
         */
        [Nullable]
        JContainer<Expression> members;
    }

    /**
     * Represents a C# enum member declaration, including optional attributes and initializer.
     * <p>
     * For example:
     * <pre>
     *     // Simple enum member
     *     Red,
     *
     *     // Member with initializer
     *     Green = 2,
     *
     *     // Member with attributes and expression initializer
     *     [Obsolete]
     *     Blue = Red | Green,
     * </pre>
     */
    
    final class EnumMemberDeclaration {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        List<AttributeList> attributeLists;

        /**
         * <pre>
         * Red = 1
         * ^^^
         * </pre>
         */
        
        Identifier name;

        /**
         * <pre>
         * Red = 1
         *     ^^^
         * </pre>
         */
        [Nullable]
        JLeftPadded<Expression> initializer;
    }

    /**
     * Represents a C# alias qualified name, which uses an extern alias to qualify a name.
     * <p>
     * For example:
     * <pre>
     *     // Using LibA to qualify TypeName
     *     LibA::TypeName
     *
     *     // Using LibB to qualify namespace
     *     LibB::System.Collections
     * </pre>
     */
    
    final class AliasQualifiedName {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * LibA::TypeName
         * ^^^^
         * </pre>
         */
        JRightPadded<Identifier> alias;

        /**
         * <pre>
         * LibA::TypeName
         *      ^^^^^^^^
         * </pre>
         * In case of method invocation, whole expression gets placed here
         */
        
        Expression name;
    }

    
    final class ArrayType {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        [Nullable]
        TypeTree typeExpression;

        
        List<ArrayDimension> dimensions;


        
        [Nullable]
        JavaType type;

    }

    
    final class Try {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;


        
        Block body;

        
        List<Cs.Try.Catch> catches;

        [Nullable]
        JLeftPadded<Block> finallie;


        /**
         * Represents a C# catch clause in a try/catch statement, which optionally includes a filter expression.
         * <p>
         * For example:
         * <pre>
         *     // Simple catch clause
         *     catch (Exception e) { }
         *
         *     // Catch with filter expression
         *     catch (Exception e) when (e.Code == 404) { }
         *
         *     // Multiple catch clauses with filters
         *     try {
         *         // code
         *     }
         *     catch (ArgumentException e) when (e.ParamName == "id") { }
         *     catch (Exception e) when (e.InnerException != null) { }
         * </pre>
         */
        
        public static final class Catch {

            
            UUID id;

            
            Space prefix;

            
            Markers markers;

            /**
             * <pre>
             * catch (Exception e) when (e.Code == 404) { }
             *      ^^^^^^^^^^^^^^
             * </pre>
             */
            
            ControlParentheses<VariableDeclarations> parameter;

            /**
             * <pre>
             * catch (Exception e) when (e.Code == 404) { }
             *                    ^^^^^^^^^^^^^^^^^^^^^
             * </pre>
             */
            [Nullable]
            JLeftPadded<ControlParentheses<Expression>> filterExpression;

            /**
             * <pre>
             * catch (Exception e) when (e.Code == 404) { }
             *                                         ^^^^
             * </pre>
             */
            
            Block body;
        }
    }

    /**
     * Represents a C# arrow expression clause (=>).
     * <p>
     * For example:
     * <pre>
     *     // In property accessors
     *     public string Name {
     *         get => _name;
     *     }
     *
     *     // In methods
     *     public string GetName() => _name;
     *
     *     // In properties
     *     public string FullName => $"{FirstName} {LastName}";
     *
     *     // In operators
     *     public static implicit operator string(Person p) => p.Name;
     * </pre>
     */
    
    final class ArrowExpressionClause {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        /**
         * <pre>
         * get => value;
         *     ^^^^^^^^
         * </pre>
         */
        JRightPadded<Expression> expression;
    }

    /**
     * Represents a C# accessor declaration (get/set/init) within a property or indexer.
     * <p>
     * For example:
     * <pre>
     *     // Simple get/set accessors
     *     public int Value {
     *         get { return _value; }
     *         set { _value = value; }
     *     }
     *
     *     // Expression body accessor
     *     public string Name {
     *         get => _name;
     *     }
     *
     *     // Auto-implemented property accessors
     *     public bool IsValid { get; set; }
     *
     *     // Init-only setter
     *     public string Id { get; init; }
     *
     *     // Access modifiers on accessors
     *     public int Age {
     *         get { return _age; }
     *         private set { _age = value; }
     *     }
     * </pre>
     */
    
    final class AccessorDeclaration {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        List<AttributeList> attributes;

        
        List<Modifier> modifiers;

        /**
         * <pre>
         * get { return value; }
         * ^^^
         * </pre>
         */
        JLeftPadded<AccessorKinds> kind;

        /**
         * <pre>
         * get => value;
         *     ^^^^^^^^^
         * </pre>
         */
        
        [Nullable]
        ArrowExpressionClause expressionBody;

        /**
         * <pre>
         * get { return value; }
         *     ^^^^^^^^^^^^^^^
         * </pre>
         */
        
        J.[Nullable] Block body;

        public enum AccessorKinds {
            Get,
            Set,
            Init,
            Add,
            Remove
        }
    }
    
    final class PointerFieldAccess {

        
        UUID id;

        
        Space prefix;

        
        Markers markers;

        
        Expression target;

        JLeftPadded<Identifier> name;

        
        [Nullable]
        JavaType type;
    }
}
