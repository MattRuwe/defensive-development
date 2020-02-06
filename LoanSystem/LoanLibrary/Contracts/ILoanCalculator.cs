using System.Collections.Generic;
using LoanLibrary.DataContracts;

namespace LoanLibrary.Contracts
{
    public interface ILoanCalculator
    {
        List<Payment> GetAmoritization(LoanFundamentals loanFundamentals);
        decimal CalculateMonthlyPayment(LoanFundamentals loanFundamentals);
        decimal CalculateRisk(RiskFactors riskFactors);
        decimal GetInterestRateForUser(int userId);
    }
}