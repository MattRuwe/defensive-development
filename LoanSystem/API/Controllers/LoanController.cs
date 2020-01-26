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
            var loanCalc = new LoanCalculator(loanFundamentals.Principal, loanFundamentals.InterestRate, loanFundamentals.TermInMonths);

            var payments = loanCalc.GetAmoritization();

            return Ok(payments);
        }

        [HttpPost("Risk")]
        public ActionResult<decimal> CalculateRisk(LoaneeCharacterisitic loaneeCharacterisitic)
        {
            var loanCalc = new LoanCalculator(0, 0, 0);

            var risk = loanCalc.CalculateRisk(loaneeCharacterisitic.CurrentAvailableCredit,
                loaneeCharacterisitic.CurrentAvailableCredit, loaneeCharacterisitic.MissedPayments,
                loaneeCharacterisitic.TotalMonthlyPaymentAmounts, loaneeCharacterisitic.AnnualIncome,
                loaneeCharacterisitic.TotalAssets);

            return Ok(risk);
        }
    }
}
