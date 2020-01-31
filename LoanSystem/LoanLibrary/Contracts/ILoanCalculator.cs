using System.Collections.Generic;
using LoanLibrary.DataContracts;

namespace LoanLibrary.Contracts
{
    public interface ILoanCalculator
    {
        List<Payment> GetAmoritization(decimal loanAmount, decimal interestRate, int termInMonths);
        decimal CalculateMonthlyPayment(decimal loanAmount, decimal interestRate, int termInMonths);
        decimal CalculateRisk(RiskFactors riskFactors);
        decimal GetInterestRateForUser(int userId);
    }
}