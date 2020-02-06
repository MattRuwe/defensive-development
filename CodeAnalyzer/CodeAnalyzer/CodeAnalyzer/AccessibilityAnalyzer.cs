using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeAnalyzer
{
    public class AccessibilityAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "CodeAnalyzer2";
        private static readonly string Title = "Classes implementing interfaces should be private";
        private static readonly string MessageFormat = "The public class {0} implements interface {1}.  Classes that implement interfaces should be internal.";
        private static readonly string Description = "Classes that implement interfaces should not be public because it breaks encapsulation.";
        private const string Category = "Architecture";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(this.AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
        }

        private void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
        {
            var semanticModel = context.SemanticModel;
            var classDeclarationNode = context.Node as ClassDeclarationSyntax;
            if (classDeclarationNode == null) return;

            if (!(semanticModel.GetDeclaredSymbol(context.Node) is INamedTypeSymbol semanticClass))
                return;

            if (semanticClass.Interfaces.Any() && semanticClass.DeclaredAccessibility == Accessibility.Public)
            {
                var diagnostic = Diagnostic.Create(Rule, classDeclarationNode.GetLocation(), semanticClass.Name, semanticClass.Interfaces.First().Name);
                context.ReportDiagnostic(diagnostic);
            }

        }


    }
}