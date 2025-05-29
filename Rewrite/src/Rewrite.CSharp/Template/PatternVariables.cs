// using System.Text;
// using Rewrite.RewriteCSharp.Tree;
//
// namespace Rewrite.Template;
//
// internal class PatternVariables
// {
//     private static readonly string DEFAULT_LABEL = "!";
//
//     private class ResultCollector
//     {
//         public readonly StringBuilder Builder = new StringBuilder();
//         public bool InstanceOfFound;
//     }
//
//     /// <summary>
//     /// Simplifies pattern variable condition.
//     /// </summary>
//     static string? SimplifiedPatternVariableCondition(Expression condition, J toReplace)
//     {
//         ResultCollector resultCollector = new ResultCollector();
//         SimplifiedPatternVariableCondition0(condition, toReplace, resultCollector);
//         return resultCollector.InstanceOfFound ? resultCollector.Builder.ToString() : null;
//     }
//
//     private static bool SimplifiedPatternVariableCondition0(J expr, J toReplace, ResultCollector collector)
//     {
//         if (expr == toReplace)
//         {
//             collector.Builder.Append('§');
//             return true;
//         }
//
//         if (expr is J.IParentheses parens)
//         {
//             collector.Builder.Append('(');
//             try
//             {
//                 return SimplifiedPatternVariableCondition0(parens.Tree, toReplace, collector);
//             }
//             finally
//             {
//                 collector.Builder.Append(')');
//             }
//         }
//         else if (expr is J.Unary unary)
//         {
//             switch (unary.Operator)
//             {
//                 case Cs.OperatorDeclaration.Operator.PlusPlus:
//                 {
//                     bool found = SimplifiedPatternVariableCondition0(unary.Expression, toReplace, collector);
//                     collector.Builder.Append("++");
//                     return found;
//                 }
//                 case Cs.OperatorDeclaration.Operator.PostDecrement:
//                 {
//                     bool found = SimplifiedPatternVariableCondition0(unary.Expression, toReplace, collector);
//                     collector.Builder.Append("--");
//                     return found;
//                 }
//                 case Cs.OperatorDeclaration.Operator.PreIncrement:
//                     collector.Builder.Append("++");
//                     break;
//                 case Cs.OperatorDeclaration.Operator.PreDecrement:
//                     collector.Builder.Append("--");
//                     break;
//                 case Cs.OperatorDeclaration.Operator.Positive:
//                     collector.Builder.Append('+');
//                     break;
//                 case Cs.OperatorDeclaration.Operator.Negative:
//                     collector.Builder.Append('-');
//                     break;
//                 case Cs.OperatorDeclaration.Operator.Complement:
//                     collector.Builder.Append('~');
//                     break;
//                 case Cs.OperatorDeclaration.Operator.Not:
//                     collector.Builder.Append('!');
//                     break;
//                 default:
//                     throw new IllegalStateException("Unexpected unary operator: " + unary.Operator);
//             }
//             return SimplifiedPatternVariableCondition0(unary.Expression, toReplace, collector);
//         }
//         else if (expr is J.Binary)
//         {
//             J.Binary binary = (J.Binary)expr;
//             int length = collector.Builder.Length;
//             bool result = SimplifiedPatternVariableCondition0(binary.Left, toReplace, collector);
//             switch (binary.Operator)
//             {
//                 case Cs.OperatorDeclaration.Operator.Addition:
//                     collector.Builder.Append('+');
//                     break;
//                 case Cs.OperatorDeclaration.Operator.Subtraction:
//                     collector.Builder.Append('-');
//                     break;
//                 case Cs.OperatorDeclaration.Operator.Multiplication:
//                     collector.Builder.Append('*');
//                     break;
//                 case Cs.OperatorDeclaration.Operator.Division:
//                     collector.Builder.Append('/');
//                     break;
//                 case Cs.OperatorDeclaration.Operator.Modulo:
//                     collector.Builder.Append('%');
//                     break;
//                 case Cs.OperatorDeclaration.Operator.LessThan:
//                     collector.Builder.Append('<');
//                     break;
//                 case Cs.OperatorDeclaration.Operator.GreaterThan:
//                     collector.Builder.Append('>');
//                     break;
//                 case Cs.OperatorDeclaration.Operator.LessThanOrEqual:
//                     collector.Builder.Append("<=");
//                     break;
//                 case Cs.OperatorDeclaration.Operator.GreaterThanOrEqual:
//                     collector.Builder.Append(">=");
//                     break;
//                 case Cs.OperatorDeclaration.Operator.Equal:
//                     collector.Builder.Append("==");
//                     break;
//                 case Cs.OperatorDeclaration.Operator.NotEqual:
//                     collector.Builder.Append("!=");
//                     break;
//                 case Cs.OperatorDeclaration.Operator.BitAnd:
//                     collector.Builder.Append('&');
//                     break;
//                 case Cs.OperatorDeclaration.Operator.BitOr:
//                     collector.Builder.Append('|');
//                     break;
//                 case Cs.OperatorDeclaration.Operator.BitXor:
//                     collector.Builder.Append('^');
//                     break;
//                 case Cs.OperatorDeclaration.Operator.LeftShift:
//                     collector.Builder.Append("<<");
//                     break;
//                 case Cs.OperatorDeclaration.Operator.RightShift:
//                     collector.Builder.Append(">>");
//                     break;
//                 case Cs.OperatorDeclaration.Operator.UnsignedRightShift:
//                     collector.Builder.Append(">>>");
//                     break;
//                 case Cs.OperatorDeclaration.Operator.Or:
//                     collector.Builder.Append("||");
//                     break;
//                 case Cs.OperatorDeclaration.Operator.And:
//                     collector.Builder.Append("&&");
//                     break;
//                 default:
//                     throw new IllegalStateException("Unexpected binary operator: " + binary.Operator);
//             }
//             result |= SimplifiedPatternVariableCondition0(binary.Right, toReplace, collector);
//             if (!result)
//             {
//                 switch (binary.Operator)
//                 {
//                     case Cs.OperatorDeclaration.Operator.LessThan:
//                     case Cs.OperatorDeclaration.Operator.GreaterThan:
//                     case Cs.OperatorDeclaration.Operator.LessThanOrEqual:
//                     case Cs.OperatorDeclaration.Operator.GreaterThanOrEqual:
//                     case Cs.OperatorDeclaration.Operator.Equal:
//                     case Cs.OperatorDeclaration.Operator.NotEqual:
//                     case Cs.OperatorDeclaration.Operator.Or:
//                     case Cs.OperatorDeclaration.Operator.And:
//                         collector.Builder.Length = length;
//                         collector.Builder.Append("true");
//                         return false;
//                 }
//             }
//             return result;
//         }
//         else if (expr is J.InstanceOf)
//         {
//             J.InstanceOf instanceOf = (J.InstanceOf)expr;
//             if (instanceOf.Pattern != null)
//             {
//                 collector.Builder.Append("((Object)null) instanceof ").Append(instanceOf.Clazz).Append(' ').Append(instanceOf.Pattern);
//                 collector.InstanceOfFound = true;
//                 return true;
//             }
//             collector.Builder.Append("true");
//         }
//         else if (expr is J.Literal)
//         {
//             J.Literal literal = (J.Literal)expr;
//             collector.Builder.Append(literal.Value);
//         }
//         else if (expr is Expression)
//         {
//             collector.Builder.Append("null");
//         }
//         return false;
//     }
//
//     static bool NeverCompletesNormally(Statement statement)
//     {
//         return NeverCompletesNormally0(statement, new HashSet<string>());
//     }
//
//     private static bool NeverCompletesNormally0(Statement statement, HashSet<string> labelsToIgnore)
//     {
//         if (statement is J.Return || statement is J.Throw)
//         {
//             return true;
//         }
//         else if (statement is J.Break)
//         {
//             J.Break breakStatement = (J.Break)statement;
//             return breakStatement.Label != null && !labelsToIgnore.Contains(breakStatement.Label.SimpleName) ||
//                    breakStatement.Label == null && !labelsToIgnore.Contains(DEFAULT_LABEL);
//         }
//         else if (statement is J.Continue)
//         {
//             J.Continue continueStatement = (J.Continue)statement;
//             return continueStatement.Label != null && !labelsToIgnore.Contains(continueStatement.Label.SimpleName) ||
//                    continueStatement.Label == null && !labelsToIgnore.Contains(DEFAULT_LABEL);
//         }
//         else if (statement is J.Block)
//         {
//             return NeverCompletesNormally0(GetLastStatement(statement), labelsToIgnore);
//         }
//         else if (statement is Loop)
//         {
//             Loop loop = (Loop)statement;
//             return NeverCompletesNormallyIgnoringLabel(loop.Body, DEFAULT_LABEL, labelsToIgnore);
//         }
//         else if (statement is J.If)
//         {
//             J.If if_ = (J.If)statement;
//             return if_.ElsePart != null &&
//                    NeverCompletesNormally0(if_.ThenPart, labelsToIgnore) &&
//                    NeverCompletesNormally0(if_.ElsePart.Body, labelsToIgnore);
//         }
//         else if (statement is J.Switch)
//         {
//             J.Switch switch_ = (J.Switch)statement;
//             if (switch_.Cases.Statements.Count == 0)
//             {
//                 return false;
//             }
//             Statement defaultCase = null;
//             foreach (Statement case_ in switch_.Cases.Statements)
//             {
//                 if (!NeverCompletesNormallyIgnoringLabel(case_, DEFAULT_LABEL, labelsToIgnore))
//                 {
//                     return false;
//                 }
//                 if (case_ is J.Case)
//                 {
//                     Expression elem = ((J.Case)case_).Pattern;
//                     if (elem is J.Identifier && ((J.Identifier)elem).SimpleName.Equals("default"))
//                     {
//                         defaultCase = case_;
//                     }
//                 }
//             }
//             return NeverCompletesNormallyIgnoringLabel(defaultCase, DEFAULT_LABEL, labelsToIgnore);
//         }
//         else if (statement is J.Case)
//         {
//             J.Case case_ = (J.Case)statement;
//             if (case_.Statements.Count == 0)
//             {
//                 // fallthrough to next case
//                 return true;
//             }
//             return NeverCompletesNormally0(GetLastStatement(case_), labelsToIgnore);
//         }
//         else if (statement is J.Try)
//         {
//             J.Try try_ = (J.Try)statement;
//             if (try_.Finally != null && try_.Finally.Statements.Count > 0 &&
//                 NeverCompletesNormally0(try_.Finally, labelsToIgnore))
//             {
//                 return true;
//             }
//             bool bodyHasExit = false;
//             if (try_.Body.Statements.Count > 0 &&
//                 !(bodyHasExit = NeverCompletesNormally0(try_.Body, labelsToIgnore)))
//             {
//                 return false;
//             }
//             foreach (J.Try.Catch catch_ in try_.Catches)
//             {
//                 if (!NeverCompletesNormally0(catch_.Body, labelsToIgnore))
//                 {
//                     return false;
//                 }
//             }
//             return bodyHasExit;
//         }
//         else if (statement is J.Synchronized)
//         {
//             return NeverCompletesNormally0(((J.Synchronized)statement).Body, labelsToIgnore);
//         }
//         else if (statement is J.Label)
//         {
//             string label = ((J.Label)statement).Label.SimpleName;
//             Statement labeledStatement = ((J.Label)statement).Statement;
//             return NeverCompletesNormallyIgnoringLabel(labeledStatement, label, labelsToIgnore);
//         }
//         return false;
//     }
//
//     private static bool NeverCompletesNormallyIgnoringLabel(Statement statement, string label, HashSet<string> labelsToIgnore)
//     {
//         bool added = labelsToIgnore.Add(label);
//         try
//         {
//             return NeverCompletesNormally0(statement, labelsToIgnore);
//         }
//         finally
//         {
//             if (added)
//             {
//                 labelsToIgnore.Remove(label);
//             }
//         }
//     }
//
//     private static Statement GetLastStatement(Statement statement)
//     {
//         if (statement is J.Block)
//         {
//             IList<Statement> statements = ((J.Block)statement).Statements;
//             return statements.Count == 0 ? null : GetLastStatement(statements[statements.Count - 1]);
//         }
//         else if (statement is J.Case)
//         {
//             IList<Statement> statements = ((J.Case)statement).Statements;
//             return statements.Count == 0 ? null : GetLastStatement(statements[statements.Count - 1]);
//         }
//         else if (statement is Loop)
//         {
//             return GetLastStatement(((Loop)statement).Body);
//         }
//         return statement;
//     }
// }