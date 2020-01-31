using System.Collections.Generic;
using LoanLibrary.DataModel;

namespace LoanLibrary.Contracts
{
    public interface IDataAccess
    {
        IEnumerable<InterestRate> GetInterestRates();
        User GetUser(int userId);
        void CreateDatabase();
    }
}