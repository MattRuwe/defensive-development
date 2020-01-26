using System.Collections.Generic;
using LoanLibrary;

namespace API.DataContracts
{
    public class LoaneeCharacterisitic
    {
        public decimal CurrentAvailableCredit { get; set; }
        public decimal CurrentUtilizedCredit { get; set; }
        public List<MissedPayment> MissedPayments { get; set; }
        public decimal TotalMonthlyPaymentAmounts { get; set; }
        public decimal AnnualIncome { get; set; }
        public decimal TotalAssets { get; set; }
    }
}