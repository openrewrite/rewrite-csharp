package org.openrewrite.csharp.tree;

import org.junit.jupiter.api.Test;
import org.openrewrite.ExecutionContext;
import org.openrewrite.InMemoryExecutionContext;
import org.openrewrite.PrintOutputCapture;
import org.openrewrite.csharp.CSharpPrinter;
import org.openrewrite.internal.ListUtils;
import org.openrewrite.java.JavaParser;
import org.openrewrite.java.JavaVisitor;
import org.openrewrite.Tree;
import org.openrewrite.java.tree.*;
import org.openrewrite.marker.Markers;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.*;
import java.util.stream.Collectors;

public class CsModelGeneratorTest {

    @Test
    public void generateCsModel() throws IOException {
        Path csJavaPath = Paths.get("src/main/java/org/openrewrite/csharp/tree/Cs.java");
        Path csModelPath = Paths.get("src/main/java/org/openrewrite/csharp/tree/CsModel.java");
        Path csModel2Path = Paths.get("src/main/java/org/openrewrite/csharp/tree/CsModel.cs");

        String csJavaSource = Files.readString(csJavaPath);

        JavaParser javaParser = JavaParser.fromJavaVersion()
          .classpath(JavaParser.runtimeClasspath())
          .build();

        ExecutionContext ctx = new InMemoryExecutionContext();

        J.CompilationUnit cu = javaParser.parse(csJavaSource)
          .findFirst()
          .map(J.CompilationUnit.class::cast)
          .orElseThrow(() -> new IllegalStateException("Failed to parse Cs.java"));

        // Transform the compilation unit
        J.CompilationUnit transformed = (J.CompilationUnit) new CsModelTransformer().visitNonNull(cu, ctx);

        // Write the transformed file
        String output = transformed.print();

        // Post-process to fix @Nullable placement for Cs.Something types
//        output = fixNullableAnnotationInOutput(output);


//        Files.writeString(csModelPath, output);
//
//        System.out.println("Successfully generated CsModel.java");

        var csPrinter = new CSharpPrinter<Integer>();
        var printOutput = new PrintOutputCapture<Integer>(0);
        csPrinter.visit(transformed, printOutput);
        output = printOutput.getOut();
        Files.writeString(csModel2Path, output);

    }


    private static class CsModelTransformer extends JavaVisitor<ExecutionContext> {

        private boolean isTopLevelInterface = true;
        private int nestingLevel = 0;

        @Override
        public J.CompilationUnit visitCompilationUnit(J.CompilationUnit cu, ExecutionContext ctx) {
            cu = (J.CompilationUnit) super.visitCompilationUnit(cu, ctx);
            return cu;
        }

        @Override
        public J.ClassDeclaration visitClassDeclaration(J.ClassDeclaration classDecl, ExecutionContext ctx) {
            String className = classDecl.getSimpleName();

            // Skip Padding classes entirely
            if (className.equals("Padding")) {
                return null;
            }

            // Track nesting level
            nestingLevel++;

            // Change interface name from Cs to CsModel for the top-level interface
            if (isTopLevelInterface && classDecl.getKind() == J.ClassDeclaration.Kind.Type.Interface &&
              className.equals("Cs")) {
                classDecl = classDecl.withName(classDecl.getName().withSimpleName("CsModel"));
                isTopLevelInterface = false;
            }

            // Process implements clause for classes
            if (classDecl.getKind() == J.ClassDeclaration.Kind.Type.Class) {
                classDecl = processImplementsClause(classDecl);
            }

            // strip out all annotations
            classDecl = classDecl.withLeadingAnnotations(new ArrayList<>());

            // Remove all methods from the body
            if (classDecl.getBody() != null) {
                List<Statement> filteredStatements = classDecl.getBody().getStatements().stream()
                  .filter(stmt -> {
                      if (stmt instanceof J.MethodDeclaration) {
                          return false;
                      }
                      if (stmt instanceof J.ClassDeclaration) {
                          J.ClassDeclaration innerClass = (J.ClassDeclaration) stmt;
                          // Keep non-Padding classes
                          return !innerClass.getSimpleName().equals("Padding");
                      }
                      return true;
                  })
                  .map(stmt -> (Statement) stmt.accept(this, ctx))
                  .filter(Objects::nonNull)
                  .collect(Collectors.toList());

                classDecl = classDecl.withBody(classDecl.getBody().withStatements(filteredStatements));
            }

            nestingLevel--;
            return classDecl;
        }

        private J.ClassDeclaration processImplementsClause(J.ClassDeclaration classDecl) {
            List<TypeTree> implementsList = classDecl.getImplements();
            if (implementsList == null || implementsList.isEmpty()) {
                return classDecl;
            }

            // Collect interfaces other than Cs
            List<String> otherInterfaces = new ArrayList<>();
            for (TypeTree impl : implementsList) {
                String implName = getTypeName(impl);
                if (!implName.equals("Cs") && !implName.equals("CsModel")) {
                    otherInterfaces.add(implName);
                }
            }

            // Clear implements list
            classDecl = classDecl.withImplements(null);

            // Add @Implements annotation if there are other interfaces
            if (!otherInterfaces.isEmpty()) {
                String annotationValue = otherInterfaces.stream()
                  .map(i -> "\"" + i + "\"")
                  .collect(Collectors.joining(", "));

                if (otherInterfaces.size() == 1) {
                    annotationValue = "\"" + otherInterfaces.get(0) + "\"";
                } else {
                    annotationValue = "{" + annotationValue + "}";
                }

                J.Annotation implementsAnnotation = new J.Annotation(
                  Tree.randomId(),
                  Space.format("\n    "),
                  Markers.EMPTY,
                  new J.Identifier(Tree.randomId(), Space.EMPTY, Markers.EMPTY,
                    Collections.emptyList(), "Implements", null, null),
                  JContainer.build(
                    Space.EMPTY,
                    Collections.singletonList(JRightPadded.build(
                      new J.Literal(Tree.randomId(), Space.EMPTY, Markers.EMPTY,
                        annotationValue, annotationValue, null,
                        JavaType.Primitive.String)
                    )),
                    Markers.EMPTY
                  )
                );

                List<J.Annotation> annotations = new ArrayList<>(classDecl.getLeadingAnnotations());
                annotations.add(implementsAnnotation);
                classDecl = classDecl.withLeadingAnnotations(annotations);
            }

            return classDecl;
        }

        private String getTypeName(TypeTree type) {
            if (type instanceof J.Identifier) {
                return ((J.Identifier) type).getSimpleName();
            } else if (type instanceof J.FieldAccess) {
                return ((J.FieldAccess) type).getSimpleName();
            } else if (type instanceof J.ParameterizedType) {
                NameTree clazz = ((J.ParameterizedType) type).getClazz();
                if (clazz instanceof TypeTree) {
                    return getTypeName((TypeTree) clazz);
                }
                return clazz.toString();
            }
            return type.toString();
        }

        @Override
        public J.VariableDeclarations visitVariableDeclarations(J.VariableDeclarations multiVariable, ExecutionContext ctx) {
            // Check if this field should be excluded
            if (shouldExcludeField(multiVariable)) {
                return null;
            }
            var annotations = multiVariable.getLeadingAnnotations();

            multiVariable = multiVariable.withLeadingAnnotations(annotations.stream().filter(x -> x.getSimpleName().contains("Nullable")).toList());
            // Remove all modifiers (private, final, etc.)
            multiVariable = multiVariable.withModifiers(Collections.emptyList());

            return multiVariable;
        }

        private boolean shouldExcludeField(J.VariableDeclarations field) {
            if (field.getTypeExpression() == null) {
                return false;
            }

            String typeStr = field.getTypeExpression().print();

            // Exclude SoftReference<TypesInUse> typesInUse
            if (typeStr.contains("SoftReference") && typeStr.contains("TypesInUse")) {
                return true;
            }

            // Exclude WeakReference<Padding> padding
            if (typeStr.contains("WeakReference") && typeStr.contains("Padding")) {
                return true;
            }

            return false;
        }

        @Override
        public J.MethodDeclaration visitMethodDeclaration(J.MethodDeclaration method, ExecutionContext ctx) {
            // Remove all methods
            return null;
        }
    }

    private static class JavaToCsharpTransformer extends JavaVisitor<ExecutionContext> {


        @Override
        public J.VariableDeclarations visitVariableDeclarations(J.VariableDeclarations multiVariable, ExecutionContext ctx) {
            boolean isNullable = multiVariable
              .getLeadingAnnotations()
              .stream()
              .anyMatch(x -> x.getSimpleName().equals("Nullable"));
            multiVariable = multiVariable.withLeadingAnnotations(new ArrayList<>());
            return multiVariable;
        }
    }
}