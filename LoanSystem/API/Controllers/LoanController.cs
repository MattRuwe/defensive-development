using System.Collections.Generic;
using LoanLibrary;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        [HttpPost]
        public ActionResult<decimal> GetPayment(LoanFundamentals loanFundamentals)
        {
            var loanCalc = new LoanCalculator(0, 0, 0);

            var payment = loanCalc.CalculateMonthlyPayment(loanFundamentals.Principal, loanFundamentals.TermInMonths, loanFundamentals.InterestRate);

            return Ok(payment);
        }

        [HttpPost("AmortizationSchedule")]
        public ActionResult<IEnumerable<Payment>> AmortizationSchedule(LoanFundamentals loanFundamentals)
        {
            var loanCalc = new LoanCalculator(loanFundamentals.Principal, loanFundamentals.InterestRate, loanFundamentals.TermInMonths);

            var payments = loanCalc.GetAmoritization();

            return Ok(payments);
        }
    }
}
