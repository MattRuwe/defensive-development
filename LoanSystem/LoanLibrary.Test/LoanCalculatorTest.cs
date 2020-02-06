using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoanLibrary.Test
{
    [TestClass]
    public class LoanCalculatorTest
    {
        [TestMethod]
        [DataRow(1, 66.67, 246.69, 10000)]
        [DataRow(2, 65.02, 248.34, 9753.31)]
        [DataRow(15, 42.62, 270.74, 6392.57)]
        [DataRow(30, 14.24, 299.12, 2136.32)]
        [DataRow(36, 2.08, 311.28, 311.43)]
        public void GetAmoritizationTest(int paymentNumber, double interest, double principal, double principalBalance)
        {
            //Arrange
            var calc = new LoanCalculator(10000, 0.08m, 36);

            //Act
            var amoritization = calc.GetAmoritization().ToList();

            //Assert
            var selectedPayment = amoritization[paymentNumber - 1];
            Assert.AreEqual((decimal)interest, selectedPayment.Interest);
            Assert.AreEqual((decimal)principal, selectedPayment.Principal);
            Assert.AreEqual((decimal)principalBalance, selectedPayment.PrincipalBalance);
        }

        [TestMethod]
        [DataRow(10000, 0.08, 36, 313.36)]
        [DataRow(20000, 0.03, 60, 359.37)]
        public void CalculateMonthlyPaymentTest(double loanAmount, double interestRate, int termInMonths, double expectedPayment)
        {
            //Arrange
            var calc = new LoanCalculator((decimal)loanAmount, (decimal)interestRate, termInMonths);

            //Act
            var actualPayment = calc.CalculateMonthlyPayment();

            //Assert
            Assert.AreEqual((decimal)expectedPayment, actualPayment);
        }
    }
}
