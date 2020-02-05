using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

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
    }

    class LoanCalcWebAppFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            
        }
    }
}
