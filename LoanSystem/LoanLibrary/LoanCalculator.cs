using System;
using System.Collections.Generic;

namespace LoanLibrary
{
    public class LoanCalculator
    {
        private decimal _loanAmount;
        private decimal _interestRate;
        private int _termInMonths;

        public LoanCalculator(decimal loanAmount, decimal interestRate, int termInMonths)
        {
            _termInMonths = termInMonths;
            _interestRate = interestRate;
            _loanAmount = loanAmount;
        }
        public List<Payment> GetAmoritization()
        {
            var currentPricinpalBalance = _loanAmount;
            var returnValue = new List<Payment>();
            var paymentAmount = CalculateMonthlyPayment();
            for (int i = 1; i <= _termInMonths; i++)
            {
                var interestAmount = currentPricinpalBalance * (_interestRate / 12);
                var principleAmount = paymentAmount - interestAmount;
                returnValue.Add(new Payment()
                {
                    PaymentNumber = i,
                    Interest = Math.Round(interestAmount, 2),
                    Principal = Math.Round(principleAmount, 2),
                    PrincipalBalance = Math.Round(currentPricinpalBalance, 2)
                });

                currentPricinpalBalance -= principleAmount;
            }

            return returnValue;
        }

        public decimal CalculateMonthlyPayment()
        {
            var monthlyInterestRate = (double)(_interestRate / 12);
            var payment = (monthlyInterestRate * (double)_loanAmount) / (1 - Math.Pow(1 + monthlyInterestRate, _termInMonths * -1));
            return (decimal)Math.Round(payment, 2);
        }
    }
}