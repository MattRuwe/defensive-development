namespace LoanLibrary
{
    public class Payment
    {
        public int PaymentNumber { get; set; }
        public decimal Principal { get; set; }
        public decimal Interest { get; set; }
        public decimal PrincipalBalance { get; set; }
    }
}