using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CodeAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "CodeAnalyzer";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
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

            var visitor = new NestingLevelVisitor();
            var nestingDepth = visitor.Visit(ifStatementSyntax);

            if (nestingDepth >= 4)
            {
                var methodDeclarationSyntax = ifStatementSyntax.FirstAncestorOrSelf<MethodDeclarationSyntax>();
                var diagnostic = Diagnostic.Create(Rule, ifStatementSyntax.GetLocation(), methodDeclarationSyntax.Identifier);
                context.ReportDiagnostic(diagnostic);
            }

        }

        //private static void AnalyzeSymbol(SymbolAnalysisContext context)
        //{
        //    // TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
        //    var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

        //    // Find just those named type symbols with names containing lowercase letters.
        //    if (namedTypeSymbol.Name.ToCharArray().Any(char.IsLower))
        //    {
        //        // For all such symbols, produce a diagnostic.
        //        var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

        //        context.ReportDiagnostic(diagnostic);
        //    }
        //}
    }

    class NestingLevelVisitor : CSharpSyntaxVisitor<int>
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
