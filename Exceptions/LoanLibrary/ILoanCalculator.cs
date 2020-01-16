using System.Collections.Generic;

namespace MyLibrary
{
    public interface ILoanCalculator
    {
        IEnumerable<Payment> GetAmoritization();
        decimal CalculateMonthlyPayment(decimal principleBalance, int term, decimal interestRate);
    }
}