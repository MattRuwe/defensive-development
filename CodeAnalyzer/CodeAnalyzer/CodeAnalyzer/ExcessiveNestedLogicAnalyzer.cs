using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ExcessiveNestedLogicAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "CodeAnalyzer";

        private static readonly string Title = "If statement nesting level is too high";
        private static readonly string MessageFormat = "The if statement nesting level is too high in method {0}";
        private static readonly string Description = "The if statement nesting level is too high which leads to code that is difficult to maintain.  Consider implementing a method to reduce the nesting level.";
        private const string Category = "Logical Organization";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeIfStatementSyntax, SyntaxKind.IfStatement);
        }

        private void AnalyzeIfStatementSyntax(SyntaxNodeAnalysisContext context)
        {
            var ifStatementSyntax = (IfStatementSyntax)context.Node;

            var visitor = new IfStatementNestingLevelVisitor();
            var nestingDepth = visitor.Visit(ifStatementSyntax);

            if (nestingDepth >= 4)
            {
                var methodDeclarationSyntax = ifStatementSyntax.FirstAncestorOrSelf<MethodDeclarationSyntax>();
                var diagnostic = Diagnostic.Create(Rule, ifStatementSyntax.GetLocation(), methodDeclarationSyntax.Identifier);
                context.ReportDiagnostic(diagnostic);
            }

        }
    }

    class IfStatementNestingLevelVisitor : CSharpSyntaxVisitor<int>
    {
        public override int VisitBlock(BlockSyntax node)
        {
            return node.Statements.Select(Visit).Max();
        }

        public override int VisitIfStatement(IfStatementSyntax node)
        {
            var result = Visit(node.Statement);
            if (node.Else != null)
            {
                var elseResult = Visit(node.Else.Statement);
                result = Math.Max(result, elseResult);
            }

            return result + 1;
        }
    }
}
