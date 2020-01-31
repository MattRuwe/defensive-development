using System;
using System.Linq;
using LoanLibrary.Contracts;
using LoanLibrary.DataContracts;
using LoanLibrary.DataModel;
using LoanLibrary.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoanLibrary.Test
{
    [TestClass]
    public class LoanCalculatorTest
    {
        private ServiceProvider _serviceProvider;

        [TestInitialize]
        public void TestInitialize()
        {
            var sc = new ServiceCollection();
            sc.AddDbContext<LoanContext>(o =>
            {
                o.UseInMemoryDatabase("LoanCalc");
            });

            sc.AddTransient<ILoanCalculator, LoanCalculator>();
            sc.AddTransient<IDataAccess, DataAccess>();
            _serviceProvider = sc.BuildServiceProvider();
        }


        [TestMethod]
        [DataRow(1, 66.67, 246.69, 10000)]
        [DataRow(2, 65.02, 248.34, 9753.31)]
        [DataRow(15, 42.62, 270.74, 6392.57)]
        [DataRow(30, 14.24, 299.12, 2136.32)]
        [DataRow(036, 2.08, 311.28, 311.43)]
        public void GetAmoritizationTest(int paymentNumber, double interest, double principal, double principalBalance)
        {
            //Arrange
            var calc = _serviceProvider.GetService<ILoanCalculator>();

            //Act
            var amoritization = calc.GetAmoritization(10000, 0.08m, 36).ToList();

            foreach (var payment in amoritization)
            {
                Console.WriteLine($"{payment.PaymentNumber.ToString().PadLeft(3, '0')}-{payment.Interest + payment.Principal:C} {payment.Interest:C} {payment.Principal:C} {payment.PrincipalBalance:C}");
            }

            //Assert
            var selectedPayment = amoritization[paymentNumber - 1];
            Assert.AreEqual((decimal)interest, selectedPayment.Interest);
            Assert.AreEqual((decimal)principal, selectedPayment.Principal);
            Assert.AreEqual((decimal)principalBalance, selectedPayment.PrincipalBalance);
        }

        [TestMethod]
        public void CalculateMonthlyPaymentTest()
        {
            //Arrange
            var calc = _serviceProvider.GetService<ILoanCalculator>();

            //Act
            var payment = calc.CalculateMonthlyPayment(10000, 0.08m, 36);

            //Assert
            Assert.AreEqual(313.36m, payment);
        }

        [TestMethod]
        [DataRow(50000, 10000, 9000, 0, 700, 150000, 0.2)]
        [DataRow(150000, 10000, 9000, 0, 700, 150000, 0.2)]
        [DataRow(150000, 10000, 9000, 5, 700, 150000, 0.23)]
        [DataRow(150000, 10000, 9000, 5, 7000, 50000, 0.58)]
        public void CalculateRiskTest(double annualIncome, double availableCredit, double creditUtilized, int missedPayments, double totalMonthlyPayments, double totalAssets, double riskResult)
        {
            //Arrange
            var calc = _serviceProvider.GetService<ILoanCalculator>();

            var riskFactors = new RiskFactors()
            {
                AnnualIncome = (decimal)annualIncome,
                CurrentAvailableCredit = (decimal)availableCredit,
                CurrentUtilizedCredit = (decimal)creditUtilized,
                MissedPayments = Enumerable.Range(0, missedPayments).Select(_ => new MissedPayment()).ToList(),
                TotalMonthlyPaymentAmounts = (decimal)totalMonthlyPayments,
                TotalAssets = (decimal)totalAssets
            };

            //Act
            var risk = calc.CalculateRisk(riskFactors);

            //Assert
            Assert.AreEqual((decimal)riskResult, risk);
        }
    }
}
