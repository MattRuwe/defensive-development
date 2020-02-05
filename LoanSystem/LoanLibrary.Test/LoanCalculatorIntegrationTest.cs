using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API;
using API.DataContracts;
using LoanLibrary.DataContracts;
using LoanLibrary.DataModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using LoanFundamentals = LoanLibrary.DataContracts.LoanFundamentals;

namespace LoanLibrary.Test
{
    [TestClass]
    class LoanCalculatorIntegrationTest
    {
        private HttpClient _client;

        [TestInitialize]
        public void TestInitialize()
        {
            var webAppFactory = new LoanCalcWebAppFactory();

            _client = webAppFactory.CreateClient();
        }

        [TestMethod]
        [TestCategory("Integration-Test")]
        public async Task GetPaymentTest()
        {
            //Arrange
            var loanFundamentals = new LoanFundamentals()
            {
                Principal = 10000m,
                InterestRate = .08m,
                TermInMonths = 36
            };

            var content = new StringContent(JsonConvert.SerializeObject(loanFundamentals), Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PostAsync("/api/Loan", content);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.AreEqual("313.36", responseString);
        }

        [TestMethod]
        [TestCategory("Integration-Test")]
        public async Task GetamortizationScheduleTest()
        {
            //Arrange
            var loanFundamentals = new LoanFundamentals()
            {
                Principal = 10000m,
                InterestRate = .08m,
                TermInMonths = 36
            };

            var content = new StringContent(JsonConvert.SerializeObject(loanFundamentals), Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PostAsync("/api/Loan/AmortizationSchedule", content);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IList<Payment>>(responseString);

            //Assert
            Assert.AreEqual(36, result.Count());
            Assert.AreEqual(255.03m, result[5].Principal);
            Assert.AreEqual(58.33m, result[5].Interest);
            Assert.AreEqual(8749.98m, result[5].PrincipalBalance);
        }

        [TestMethod]
        [TestCategory("Integration-Test")]
        public async Task CalculateRiskTest()
        {
            //Arrange
            var loaneeCharacterisitic = new LoaneeCharacterisitic()
            {
                AnnualIncome = 100000m,
                CurrentAvailableCredit = 5000m,
                CurrentUtilizedCredit = 5000m,
                MissedPayments = new List<MissedPayment>()
                {
                    new MissedPayment() {Amount = 200m, DueDate = DateTime.Now.AddDays(-74)}
                },
                TotalAssets = 200000m,
                TotalMonthlyPaymentAmounts = 1500m
            };

            var serializedLoaneeCharacterisitics = JsonConvert.SerializeObject(loaneeCharacterisitic);
            var content = new StringContent(serializedLoaneeCharacterisitics, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PostAsync("/api/Loan/Risk", content);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.AreEqual("0.23", responseString);
        }

        [TestMethod]
        [TestCategory("Integration-Test")]
        public async Task GetInterestRatesTest()
        {
            //Arrange

            //Act
            var response = await _client.GetAsync("/api/Loan/InterestRates");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IList<InterestRate>>(responseString);
            //Assert
            Assert.AreEqual(100, result.Count);
        }
    }

    class LoanCalcWebAppFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

        }
    }
}
