namespace API.DataContracts
{
    public class ApiLoanFundamentals
    {
        public decimal Principal { get; set; }
        public decimal InterestRate { get; set; }
        public int TermInMonths { get; set; }
    }
}