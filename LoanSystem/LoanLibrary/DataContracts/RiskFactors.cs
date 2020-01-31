using System.Collections.Generic;

namespace LoanLibrary.DataContracts
{
    public class RiskFactors
    {
        public decimal CurrentAvailableCredit { get; set; }
        public decimal CurrentUtilizedCredit { get; set; }
        public List<MissedPayment> MissedPayments { get; set; }
        public decimal TotalMonthlyPaymentAmounts { get; set; }
        public decimal AnnualIncome { get; set; }
        public decimal TotalAssets { get; set; }
    }
}