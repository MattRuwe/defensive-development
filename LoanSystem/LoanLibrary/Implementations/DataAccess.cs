using System.Collections.Generic;
using System.Linq;
using LoanLibrary.Contracts;
using LoanLibrary.DataModel;

namespace LoanLibrary.Implementations
{
    internal class DataAccess : IDataAccess
    {
        private readonly LoanContext _loanContext;

        public DataAccess(LoanContext loanContext)
        {
            _loanContext = loanContext;
        }

        public IEnumerable<InterestRate> GetInterestRates()
        {
            return _loanContext.InterestRates;
        }

        public User GetUser(int userId)
        {
            return _loanContext.Users.FirstOrDefault(_ => _.Id == userId);
        }

        public void CreateDatabase()
        {
            _loanContext.Database.EnsureCreated();
        }
    }
}