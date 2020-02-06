using System.Collections.Generic;
using API.DataContracts;
using LoanLibrary;
using LoanLibrary.Contracts;
using LoanLibrary.DataContracts;
using LoanLibrary.DataModel;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private ILoanCalculator _loanCalculator;
        private IDataAccess _dataAccess;

        public LoanController(ILoanCalculator loanCalculator, IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _loanCalculator = loanCalculator;
        }

        [HttpPost]
        public ActionResult<decimal> GetPayment(ApiLoanFundamentals apiLoanFundamentals)
        {
            var loanFundamentals = new LoanFundamentals()
            {
                Principal = apiLoanFundamentals.Principal,
                InterestRate = apiLoanFundamentals.InterestRate,
                TermInMonths = apiLoanFundamentals.TermInMonths
            };

            var payment = _loanCalculator.CalculateMonthlyPayment(loanFundamentals);

            return Ok(payment);
        }

        [HttpPost("AmortizationSchedule")]
        public ActionResult<IEnumerable<Payment>> AmortizationSchedule(ApiLoanFundamentals apiLoanFundamentals)
        {
            var loanFundamentals = new LoanFundamentals()
            {
                Principal = apiLoanFundamentals.Principal,
                InterestRate = apiLoanFundamentals.InterestRate,
                TermInMonths = apiLoanFundamentals.TermInMonths
            };

            var payments = _loanCalculator.GetAmoritization(loanFundamentals);

            return Ok(payments);
        }

        [HttpPost("Risk")]
        public ActionResult<decimal> CalculateRisk(ApiRiskFactors apiRiskFactors)
        {

            var riskFactors = new RiskFactors()
            {
                AnnualIncome = apiRiskFactors.AnnualIncome,
                TotalAssets = apiRiskFactors.TotalAssets,
                CurrentUtilizedCredit = apiRiskFactors.CurrentUtilizedCredit,
                CurrentAvailableCredit = apiRiskFactors.CurrentAvailableCredit,
                MissedPayments = apiRiskFactors.MissedPayments,
                TotalMonthlyPaymentAmounts = apiRiskFactors.TotalMonthlyPaymentAmounts
            };

            var risk = _loanCalculator.CalculateRisk(riskFactors);

            return Ok(risk);
        }

        [HttpGet("InterestRates")]
        public ActionResult<List<InterestRate>> GetInterestRates()
        {
            return Ok(_dataAccess.GetInterestRates());
        }

        [HttpGet("CreateDatabase")]
        public ActionResult CreateDatabase()
        {
            _dataAccess.CreateDatabase();

            return Ok();
        }
    }
}
