using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestHelper;
using CodeAnalyzer;

namespace CodeAnalyzer.Test
{
    [TestClass]
    public class IfStatementNestingRule : CodeFixVerifier
    {

        //No diagnostics expected to show up
        [TestMethod]
        public void NoRuleViolationTest()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void RuleViolationTest()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TestClass
        {   
            public int TestMethod(int value)
            {
                if(value > 1)
                {
                    if(value > 2>
                    {
                        if(value > 3)
                        {
                            if(value > 4)
                            {
                                return 4;
                            }
                            return 3;
                        }
                        return 2;
                    }
                    return 1;
                }
            }
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "CodeAnalyzer",
                Message = "The if statement nesting level is too high in method TestMethod",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {new DiagnosticResultLocation("Test0.cs", 15, 17)}
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new CodeAnalyzerAnalyzer();
        }
    }
}
