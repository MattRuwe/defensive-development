using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoanLibrary;
using Microsoft.AspNetCore.Mvc;
using LoanFundamentals = API.DataContracts.LoanFundamentals;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        [HttpPost]
        public ActionResult<decimal> GetPayment(LoanFundamentals loanFundamentals)
        {
            var loanCalc = new LoanCalculator(loanFundamentals.Principal, loanFundamentals.InterestRate, loanFundamentals.TermInMonths);

            var payment = loanCalc.CalculateMonthlyPayment();

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
