using System.Collections;
using Rewrite.Java.Tree;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp.Tree;

public abstract class CoordinateBuilder
{
    protected J Tree { get; }

    protected CoordinateBuilder(J tree)
    {
        this.Tree = tree;
    }

    protected virtual CSharpCoordinates Before(Space.Location location)
    {
        return new CSharpCoordinates(Tree, location, CSharpCoordinates.Mode.BEFORE, null);
    }

    protected virtual CSharpCoordinates After(Space.Location location)
    {
        return new CSharpCoordinates(Tree, location, CSharpCoordinates.Mode.AFTER, null);
    }

    protected virtual CSharpCoordinates Replace(Space.Location location)
    {
        return new CSharpCoordinates(Tree, location, CSharpCoordinates.Mode.REPLACEMENT, null);
    }

    /// <summary>
    /// Even though statements are not a superset of expressions,
    /// this provides a way for statements that are
    /// also expressions to have a generic coordinate.
    /// </summary>
    public class Statement : Expression
    {
        public Statement(Rewrite.RewriteJava.Tree.Statement tree)
            : base(tree)
        {
        }

        public CSharpCoordinates After()
        {
            return After(Space.Location.STATEMENT_PREFIX);
        }

        public virtual CSharpCoordinates Before()
        {
            return Before(Space.Location.STATEMENT_PREFIX);
        }

        public override CSharpCoordinates Replace()
        {
            return Replace(Space.Location.STATEMENT_PREFIX);
        }
    }

    public class Expression : CoordinateBuilder
    {
        /// <summary>
        /// <param name="tree">This is of type <see cref="J"/> only so that
        /// statements that are not expressions can call
        /// super on this type. Since <see cref="Statement"/>
        /// overrides <see cref="Replace"/>, this is not
        /// otherwise a problem.</param>
        /// </summary>
        public Expression(J tree)
            : base(tree)
        {
        }

        public virtual CSharpCoordinates Replace()
        {
            return Replace(Space.Location.EXPRESSION_PREFIX);
        }
    }

    public class Annotation : Expression
    {
        public Annotation(J.Annotation tree)
            : base(tree)
        {
        }

        public override CSharpCoordinates Replace()
        {
            return Replace(Space.Location.ANNOTATION_PREFIX);
        }

        public CSharpCoordinates ReplaceArguments()
        {
            return Replace(Space.Location.ANNOTATION_ARGUMENTS);
        }
    }

    public class Block : Statement
    {
        public Block(J.Block tree)
            : base(tree)
        {
        }

        public CSharpCoordinates FirstStatement()
        {
            if (((J.Block)Tree).Statements.Count == 0)
            {
                return LastStatement();
            }
            else
            {
                return ((J.Block)Tree).Statements[0].Coordinates.Before();
            }
        }

        public CSharpCoordinates AddStatement(IComparer<RewriteJava.Tree.Statement>? idealOrdering)
        {
            return new CSharpCoordinates(Tree, Space.Location.BLOCK_END, CSharpCoordinates.Mode.BEFORE, idealOrdering?.AsUntyped());
        }

        public CSharpCoordinates AddMethodDeclaration(IComparer<J.MethodDeclaration> idealOrdering)
        {
            IComparer natural =Comparer<J.MethodDeclaration>.Default;

            return AddStatement(Comparer<RewriteJava.Tree.Statement>.Create((s1, s2) => s1 is Cs.MethodDeclaration && s2 is Cs.MethodDeclaration ?
                    idealOrdering.Compare((J.MethodDeclaration)s1, (J.MethodDeclaration)s2) :
                    natural.Compare(s1.Id, s2.Id)
            ));
        }

        public CSharpCoordinates LastStatement()
        {
            return Before(Space.Location.BLOCK_END);
        }
    }

    public class ClassDeclaration : Statement
    {
        public ClassDeclaration(Cs.ClassDeclaration tree)
            : base(tree)
        {
        }

        /// <summary>
        /// <param name="idealOrdering">The new annotation will be inserted in as close to an ideal ordering
        /// as possible, understanding that the existing annotations may not be
        /// ordered according to the IComparer.</param>
        /// <returns>A variable with a new annotation, inserted before the annotation it would appear
        /// before in an ideal ordering, or as the last annotation if it would not appear before any
        /// existing annotations in an ideal ordering.</returns>
        /// </summary>
        public CSharpCoordinates AddAnnotation(IComparer idealOrdering)
        {
            return new CSharpCoordinates(Tree, Space.Location.ANNOTATIONS, CSharpCoordinates.Mode.BEFORE, idealOrdering);
        }

        public CSharpCoordinates ReplaceAnnotations()
        {
            return Replace(Space.Location.ANNOTATIONS);
        }

        public CSharpCoordinates ReplaceTypeParameters()
        {
            return Replace(Space.Location.TYPE_PARAMETERS);
        }

        public CSharpCoordinates ReplaceExtendsClause()
        {
            return Replace(Space.Location.EXTENDS);
        }

        public CSharpCoordinates ReplaceImplementsClause()
        {
            return Replace(Space.Location.IMPLEMENTS);
        }

        public CSharpCoordinates AddImplementsClause()
        {
            return new CSharpCoordinates(Tree, Space.Location.IMPLEMENTS, CSharpCoordinates.Mode.AFTER, null);
        }
    }

    public class FieldAccess : CoordinateBuilder
    {
        public FieldAccess(J.FieldAccess tree)
            : base(tree)
        {
        }

        public CSharpCoordinates Replace()
        {
            return Replace(Space.Location.FIELD_ACCESS_PREFIX);
        }
    }

    public class Identifier : Expression
    {
        public Identifier(J.Identifier tree)
            : base(tree)
        {
        }

        public override CSharpCoordinates Replace()
        {
            return Replace(Space.Location.IDENTIFIER_PREFIX);
        }
    }

    public static class Lambda
    {
        public class Parameters : CoordinateBuilder
        {
            public Parameters(J.Lambda.Parameters tree)
                : base(tree)
            {
            }

            public CSharpCoordinates Replace()
            {
                return Replace(Space.Location.LAMBDA_PARAMETERS_PREFIX);
            }
        }
    }

    public class MethodDeclaration : Statement
    {
        public MethodDeclaration(Cs.MethodDeclaration tree)
            : base(tree)
        {
        }

        /// <summary>
        /// <param name="idealOrdering">The new annotation will be inserted in as close to an ideal ordering
        /// as possible, understanding that the existing annotations may not be
        /// ordered according to the IComparer.</param>
        /// <returns>A method with a new annotation, inserted before the annotation it would appear
        /// before in an ideal ordering, or as the last annotation if it would not appear before any
        /// existing annotations in an ideal ordering.</returns>
        /// </summary>
        public CSharpCoordinates AddAnnotation(IComparer idealOrdering)
        {
            return new CSharpCoordinates(Tree, Space.Location.ANNOTATIONS, CSharpCoordinates.Mode.BEFORE, idealOrdering);
        }

        public CSharpCoordinates ReplaceAnnotations()
        {
            return Replace(Space.Location.ANNOTATIONS);
        }

        public CSharpCoordinates ReplaceTypeParameters()
        {
            return Replace(Space.Location.TYPE_PARAMETERS);
        }

        public CSharpCoordinates ReplaceParameters()
        {
            return Replace(Space.Location.METHOD_DECLARATION_PARAMETERS);
        }

        public CSharpCoordinates ReplaceThrows()
        {
            return Replace(Space.Location.THROWS);
        }

        public CSharpCoordinates ReplaceBody()
        {
            return Replace(Space.Location.BLOCK_PREFIX);
        }
    }

    public class MethodInvocation : Statement
    {
        public MethodInvocation(J.MethodInvocation tree)
            : base(tree)
        {
        }

        public CSharpCoordinates ReplaceArguments()
        {
            return Replace(Space.Location.METHOD_INVOCATION_ARGUMENTS);
        }

        /// <summary>
        /// Indicates replacement of the invocation's name and argument list, while preserving its select.
        /// </summary>
        public CSharpCoordinates ReplaceMethod()
        {
            return Replace(Space.Location.METHOD_INVOCATION_NAME);
        }
    }

    public class Unary : Statement
    {
        public Unary(J.Unary tree)
            : base(tree)
        {
        }
        
        public Unary(Cs.Unary tree)
            : base(tree)
        {
        }

        protected override CSharpCoordinates After(Space.Location location)
        {
            return After(IsModifying() ? Space.Location.STATEMENT_PREFIX : Space.Location.EXPRESSION_PREFIX);
        }

        public override CSharpCoordinates Before()
        {
            return Before(IsModifying() ? Space.Location.STATEMENT_PREFIX : Space.Location.EXPRESSION_PREFIX);
        }

        public override CSharpCoordinates Replace()
        {
            return Replace(IsModifying() ? Space.Location.STATEMENT_PREFIX : Space.Location.EXPRESSION_PREFIX);
        }

        private bool IsModifying()
        {
            return (Tree as J.Unary)?.Operator.IsModifying() ?? (Tree as Cs.Unary)?.Operator.IsModifying() ?? false;
        }
    }

    public class Package : Statement
    {
        public Package(J.Package tree)
            : base(tree)
        {
        }

        public override CSharpCoordinates Replace()
        {
            return Replace(Space.Location.PACKAGE_PREFIX);
        }
    }

    public class VariableDeclarations : Statement
    {
        public VariableDeclarations(J.VariableDeclarations tree)
            : base(tree)
        {
        }

        public CSharpCoordinates ReplaceAnnotations()
        {
            return Replace(Space.Location.ANNOTATIONS);
        }

        public CSharpCoordinates AddAnnotation(IComparer idealOrdering)
        {
            return new CSharpCoordinates(Tree, Space.Location.ANNOTATIONS, CSharpCoordinates.Mode.BEFORE, idealOrdering);
        }
    }


}
