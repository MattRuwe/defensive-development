using System.Collections.Generic;
using API.DataContracts;
using LoanLibrary;
using Microsoft.AspNetCore.Mvc;
using LoanFundamentals = API.DataContracts.LoanFundamentals;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        [HttpPost("AmortizationSchedule")]
        public ActionResult<IEnumerable<Payment>> AmortizationSchedule(LoanFundamentals loanFundamentals)
        {
            var loanCalc = new LoanCalculator();

            var payments = loanCalc.GetAmoritization(loanFundamentals.Principal, loanFundamentals.InterestRate, loanFundamentals.TermInMonths);

            return Ok(payments);
        }

        [HttpPost("Risk")]
        public ActionResult<decimal> CalculateRisk(LoaneeCharacterisitic loaneeCharacterisitic)
        {
            var loanCalc = new LoanCalculator();

            var riskFactors = new RiskFactors()
            {
                AnnualIncome = loaneeCharacterisitic.AnnualIncome,
                TotalAssets = loaneeCharacterisitic.TotalAssets,
                CurrentUtilizedCredit = loaneeCharacterisitic.CurrentUtilizedCredit,
                CurrentAvailableCredit = loaneeCharacterisitic.CurrentAvailableCredit,
                MissedPayments = loaneeCharacterisitic.MissedPayments,
                TotalMonthlyPaymentAmounts = loaneeCharacterisitic.TotalMonthlyPaymentAmounts
            };

            var risk = loanCalc.CalculateRisk(riskFactors);

            return Ok(risk);
        }

        [HttpGet("InterestRates")]
        public ActionResult<List<InterestRate>> GetInterestRates()
        {
            var dataAccess = new DataAccess();

            return Ok(dataAccess.GetInterestRates());
        }
    }
}
