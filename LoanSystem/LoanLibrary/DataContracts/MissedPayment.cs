using System;

namespace LoanLibrary.DataContracts
{
    public class MissedPayment
    {
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
    }
}