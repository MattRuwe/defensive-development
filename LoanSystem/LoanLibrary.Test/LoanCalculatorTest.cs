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
        [DataRow(036, 2.08, 311.28, 311.43)]
        public void GetAmoritizationTest(int paymentNumber, double interest, double principal, double principalBalance)
        {
            var calc = new LoanCalculator();

            var amoritization = calc.GetAmoritization(10000, 0.08m, 36).ToList();

            foreach (var payment in amoritization)
            {
                Console.WriteLine($"{payment.PaymentNumber.ToString().PadLeft(3, '0')}-{payment.Interest + payment.Principal:C} {payment.Interest:C} {payment.Principal:C} {payment.PrincipalBalance:C}");
            }

            var selectedPayment = amoritization[paymentNumber - 1];
            Assert.AreEqual((decimal)interest, selectedPayment.Interest);
            Assert.AreEqual((decimal)principal, selectedPayment.Principal);
            Assert.AreEqual((decimal)principalBalance, selectedPayment.PrincipalBalance);
        }

        [TestMethod]
        public void CalculateMonthlyPaymentTest()
        {
            var calc = new LoanCalculator();

            var payment = calc.CalculateMonthlyPayment(10000, 36, 0.08m);

            Assert.AreEqual(313.36m, payment);
        }
    }
}
