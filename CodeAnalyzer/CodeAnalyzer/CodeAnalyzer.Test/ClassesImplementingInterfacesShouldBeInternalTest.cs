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
    public class ClassesImplementingInterfacesShouldBeInternalTest : CodeFixVerifier
    {

        //No diagnostics expected to show up
        [TestMethod]
        public void NoRuleViolationTest()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ClassWithoutInterfaceAccessibilityTest()
        {
            var test = @"
                namespace MyNamespace
                {
                    public class TestClass
                    {   
                        public int TestMethod(int value)
                        {    
                            return 1;
                        }
                    }
                }";
            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void InternalClassWithInterfaceAccessibilityTest()
        {
            var test = @"
                namespace MyNamespace
                {
                    public interface IMyInterface
                    {
                        int TestMethod(int value);
                    }

                    class TestClass : IMyInterface
                    {   
                        public int TestMethod(int value)
                        {    
                            return 1;
                        }
                    }
                }";
            VerifyCSharpDiagnostic(test);
        }


        [TestMethod]
        public void ClassWithInterfaceAccessibilityTest()
        {
            var test = @"
                namespace MyNamespace
                {
                    public interface IMyInterface
                    {
                        int TestMethod(int value);
                    }

                    public class TestClass : IMyInterface
                    {   
                        public int TestMethod(int value)
                        {    
                            return 1;
                        }
                    }
                }";

            var expected = new DiagnosticResult
            {
                Id = "CodeAnalyzer2",
                Message = "The public class TestClass implements interface IMyInterface.  Classes that implement interfaces should be internal.",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {new DiagnosticResultLocation("Test0.cs", 9, 21)}
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new AccessibilityAnalyzer();
        }
    }
}
