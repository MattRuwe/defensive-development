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
        private ILoanCalculator _loanCalculator;
        private IDataAccess _dataAccess;

        public LoanController(ILoanCalculator loanCalculator, IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _loanCalculator = loanCalculator;
        }

        [HttpPost("AmortizationSchedule")]
        public ActionResult<IEnumerable<Payment>> AmortizationSchedule(LoanFundamentals loanFundamentals)
        {
            var payments = _loanCalculator.GetAmoritization(loanFundamentals.Principal, loanFundamentals.InterestRate, loanFundamentals.TermInMonths);

            return Ok(payments);
        }

        [HttpPost("Risk")]
        public ActionResult<decimal> CalculateRisk(LoaneeCharacterisitic loaneeCharacterisitic)
        {

            var riskFactors = new RiskFactors()
            {
                AnnualIncome = loaneeCharacterisitic.AnnualIncome,
                TotalAssets = loaneeCharacterisitic.TotalAssets,
                CurrentUtilizedCredit = loaneeCharacterisitic.CurrentUtilizedCredit,
                CurrentAvailableCredit = loaneeCharacterisitic.CurrentAvailableCredit,
                MissedPayments = loaneeCharacterisitic.MissedPayments,
                TotalMonthlyPaymentAmounts = loaneeCharacterisitic.TotalMonthlyPaymentAmounts
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
