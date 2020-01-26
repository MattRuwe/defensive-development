namespace LoanLibrary
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public decimal TotalCreditAvailable { get; set; }
        public decimal CreditUtilized { get; set; }
        public decimal AnnualIncome { get; set; }
        public decimal TotalMonthlyPayments { get; set; }
        public decimal TotalAssets { get; set; }
    }
}