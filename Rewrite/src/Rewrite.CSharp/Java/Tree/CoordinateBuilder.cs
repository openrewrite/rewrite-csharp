// using System.Collections;
//
// namespace Rewrite.RewriteCSharp.Tree;
//
// public abstract class CoordinateBuilder
// {
//     protected J Tree { get; }
//
//     protected CoordinateBuilder(J tree)
//     {
//         this.Tree = tree;
//     }
//
//     protected Coordinates Before(Space.Location location)
//     {
//         return new Coordinates(Tree, location, Coordinates.Mode.BEFORE, null);
//     }
//
//     protected Coordinates After(Space.Location location)
//     {
//         return new Coordinates(Tree, location, Coordinates.Mode.AFTER, null);
//     }
//
//     protected Coordinates Replace(Space.Location location)
//     {
//         return new Coordinates(Tree, location, Coordinates.Mode.REPLACEMENT, null);
//     }
//
//     /// <summary>
//     /// Even though statements are not a superset of expressions,
//     /// this provides a way for statements that are
//     /// also expressions to have a generic coordinate.
//     /// </summary>
//     public class Statement : Expression
//     {
//         public Statement(Rewrite.RewriteJava.Tree.Statement tree)
//             : base(tree)
//         {
//         }
//
//         public Coordinates After()
//         {
//             return After(Space.Location.STATEMENT_PREFIX);
//         }
//
//         public Coordinates Before()
//         {
//             return Before(Space.Location.STATEMENT_PREFIX);
//         }
//
//         public override Coordinates Replace()
//         {
//             return Replace(Space.Location.STATEMENT_PREFIX);
//         }
//     }
//
//     public class Expression : CoordinateBuilder
//     {
//         /// <summary>
//         /// <param name="tree">This is of type <see cref="J"/> only so that
//         /// statements that are not expressions can call
//         /// super on this type. Since <see cref="Statement"/>
//         /// overrides <see cref="Replace"/>, this is not
//         /// otherwise a problem.</param>
//         /// </summary>
//         public Expression(J tree)
//             : base(tree)
//         {
//         }
//
//         public virtual Coordinates Replace()
//         {
//             return Replace(Space.Location.EXPRESSION_PREFIX);
//         }
//     }
//
//     public class Annotation : Expression
//     {
//         public Annotation(J.Annotation tree)
//             : base(tree)
//         {
//         }
//
//         public override Coordinates Replace()
//         {
//             return Replace(Space.Location.ANNOTATION_PREFIX);
//         }
//
//         public Coordinates ReplaceArguments()
//         {
//             return Replace(Space.Location.ANNOTATION_ARGUMENTS);
//         }
//     }
//
//     public class Block : Statement
//     {
//         public Block(J.Block tree)
//             : base(tree)
//         {
//         }
//
//         public Coordinates FirstStatement()
//         {
//             if (((J.Block)Tree).Statements.Count == 0)
//             {
//                 return LastStatement();
//             }
//             else
//             {
//                 return ((J.Block)Tree).Statements[0].GetCoordinates().Before();
//             }
//         }
//
//         public Coordinates AddStatement(IComparer idealOrdering)
//         {
//             return new Coordinates(Tree, Space.Location.BLOCK_END, Coordinates.Mode.BEFORE, idealOrdering);
//         }
//
//         public Coordinates AddMethodDeclaration(IComparer idealOrdering)
//         {
//             IComparer natural = IComparer.NaturalOrder();
//             return AddStatement((org.openrewrite.java.tree.Statement s1, org.openrewrite.java.tree.Statement s2) => s1 is J.MethodDeclaration && s2 is J.MethodDeclaration ?
//                     idealOrdering.Compare((J.MethodDeclaration)s1, (J.MethodDeclaration)s2) :
//                     natural.Compare(s1.GetId(), s2.GetId())
//             );
//         }
//
//         public Coordinates LastStatement()
//         {
//             return Before(Space.Location.BLOCK_END);
//         }
//     }
//
//     public class ClassDeclaration : Statement
//     {
//         public ClassDeclaration(J.ClassDeclaration tree)
//             : base(tree)
//         {
//         }
//
//         /// <summary>
//         /// <param name="idealOrdering">The new annotation will be inserted in as close to an ideal ordering
//         /// as possible, understanding that the existing annotations may not be
//         /// ordered according to the IComparer.</param>
//         /// <returns>A variable with a new annotation, inserted before the annotation it would appear
//         /// before in an ideal ordering, or as the last annotation if it would not appear before any
//         /// existing annotations in an ideal ordering.</returns>
//         /// </summary>
//         public Coordinates AddAnnotation(IComparer idealOrdering)
//         {
//             return new Coordinates(Tree, Space.Location.ANNOTATIONS, Coordinates.Mode.BEFORE, idealOrdering);
//         }
//
//         public Coordinates ReplaceAnnotations()
//         {
//             return Replace(Space.Location.ANNOTATIONS);
//         }
//
//         public Coordinates ReplaceTypeParameters()
//         {
//             return Replace(Space.Location.TYPE_PARAMETERS);
//         }
//
//         public Coordinates ReplaceExtendsClause()
//         {
//             return Replace(Space.Location.EXTENDS);
//         }
//
//         public Coordinates ReplaceImplementsClause()
//         {
//             return Replace(Space.Location.IMPLEMENTS);
//         }
//
//         public Coordinates AddImplementsClause()
//         {
//             return new Coordinates(Tree, Space.Location.IMPLEMENTS, Coordinates.Mode.AFTER, null);
//         }
//     }
//
//     public class FieldAccess : CoordinateBuilder
//     {
//         public FieldAccess(J.FieldAccess tree)
//             : base(tree)
//         {
//         }
//
//         public Coordinates Replace()
//         {
//             return Replace(Space.Location.FIELD_ACCESS_PREFIX);
//         }
//     }
//
//     public class Identifier : Expression
//     {
//         public Identifier(J.Identifier tree)
//             : base(tree)
//         {
//         }
//
//         public override Coordinates Replace()
//         {
//             return Replace(Space.Location.IDENTIFIER_PREFIX);
//         }
//     }
//
//     public static class Lambda
//     {
//         public class Parameters : CoordinateBuilder
//         {
//             public Parameters(J.Lambda.Parameters tree)
//                 : base(tree)
//             {
//             }
//
//             public Coordinates Replace()
//             {
//                 return Replace(Space.Location.LAMBDA_PARAMETERS_PREFIX);
//             }
//         }
//     }
//
//     public class MethodDeclaration : Statement
//     {
//         public MethodDeclaration(J.MethodDeclaration tree)
//             : base(tree)
//         {
//         }
//
//         /// <summary>
//         /// <param name="idealOrdering">The new annotation will be inserted in as close to an ideal ordering
//         /// as possible, understanding that the existing annotations may not be
//         /// ordered according to the IComparer.</param>
//         /// <returns>A method with a new annotation, inserted before the annotation it would appear
//         /// before in an ideal ordering, or as the last annotation if it would not appear before any
//         /// existing annotations in an ideal ordering.</returns>
//         /// </summary>
//         public Coordinates AddAnnotation(IComparer idealOrdering)
//         {
//             return new Coordinates(Tree, Space.Location.ANNOTATIONS, Coordinates.Mode.BEFORE, idealOrdering);
//         }
//
//         public Coordinates ReplaceAnnotations()
//         {
//             return Replace(Space.Location.ANNOTATIONS);
//         }
//
//         public Coordinates ReplaceTypeParameters()
//         {
//             return Replace(Space.Location.TYPE_PARAMETERS);
//         }
//
//         public Coordinates ReplaceParameters()
//         {
//             return Replace(Space.Location.METHOD_DECLARATION_PARAMETERS);
//         }
//
//         public Coordinates ReplaceThrows()
//         {
//             return Replace(Space.Location.THROWS);
//         }
//
//         public Coordinates ReplaceBody()
//         {
//             return Replace(Space.Location.BLOCK_PREFIX);
//         }
//     }
//
//     public class MethodInvocation : Statement
//     {
//         public MethodInvocation(J.MethodInvocation tree)
//             : base(tree)
//         {
//         }
//
//         public Coordinates ReplaceArguments()
//         {
//             return Replace(Space.Location.METHOD_INVOCATION_ARGUMENTS);
//         }
//
//         /// <summary>
//         /// Indicates replacement of the invocation's name and argument list, while preserving its select.
//         /// </summary>
//         public Coordinates ReplaceMethod()
//         {
//             return Replace(Space.Location.METHOD_INVOCATION_NAME);
//         }
//     }
//
//     public class Unary : Statement
//     {
//         public Unary(J.Unary tree)
//             : base(tree)
//         {
//         }
//
//         protected override Coordinates After(Space.Location location)
//         {
//             return After(IsModifying() ? Space.Location.STATEMENT_PREFIX : Space.Location.EXPRESSION_PREFIX);
//         }
//
//         public override Coordinates Before()
//         {
//             return Before(IsModifying() ? Space.Location.STATEMENT_PREFIX : Space.Location.EXPRESSION_PREFIX);
//         }
//
//         public override Coordinates Replace()
//         {
//             return Replace(IsModifying() ? Space.Location.STATEMENT_PREFIX : Space.Location.EXPRESSION_PREFIX);
//         }
//
//         private bool IsModifying()
//         {
//             return ((J.Unary)Tree).GetOperator().IsModifying();
//         }
//     }
//
//     public class Package : Statement
//     {
//         public Package(J.Package tree)
//             : base(tree)
//         {
//         }
//
//         public override Coordinates Replace()
//         {
//             return Replace(Space.Location.PACKAGE_PREFIX);
//         }
//     }
//
//     public class VariableDeclarations : Statement
//     {
//         public VariableDeclarations(J.VariableDeclarations tree)
//             : base(tree)
//         {
//         }
//
//         public Coordinates ReplaceAnnotations()
//         {
//             return Replace(Space.Location.ANNOTATIONS);
//         }
//
//         public Coordinates AddAnnotation(IComparer idealOrdering)
//         {
//             return new Coordinates(Tree, Space.Location.ANNOTATIONS, Coordinates.Mode.BEFORE, idealOrdering);
//         }
//     }
//
//     public class Yield : Statement
//     {
//         public Yield(J.Yield tree)
//             : base(tree)
//         {
//         }
//
//         public override Coordinates Replace()
//         {
//             return Replace(Space.Location.YIELD_PREFIX);
//         }
//     }
// }
