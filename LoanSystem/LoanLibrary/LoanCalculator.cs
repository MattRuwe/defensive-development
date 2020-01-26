using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Bogus;

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
            var paymentAmount = CalculateMonthlyPayment(_loanAmount, _termInMonths, _interestRate);
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

        public decimal CalculateMonthlyPayment(decimal loanAmount, int termInMonths, decimal interestRate)
        {
            var monthlyInterestRate = (double)(interestRate / 12);
            var payment = (monthlyInterestRate * (double)loanAmount) / (1 - Math.Pow(1 + monthlyInterestRate, termInMonths * -1));
            return (decimal)Math.Round(payment, 2);
        }


        public decimal CalculateRisk(decimal currentAvailableCredit, decimal currentUtilizedCredit, List<MissedPayment> missedPayments, decimal totalMonthlyPaymentAmounts, decimal annualIncome, decimal totalAssets)
        {
            //Check for credit utilized greater than current available credit


            decimal creditUtiltizationRatio = currentUtilizedCredit / currentAvailableCredit;
            decimal debtToIncomeRatio = totalMonthlyPaymentAmounts / (annualIncome / 12);

            var creditRisk = 0m;
            if (creditUtiltizationRatio > .75m)
            {
                creditRisk += .1m;
                if (totalMonthlyPaymentAmounts > 5000)
                {
                    creditRisk += .45m;
                    if (missedPayments.Count() > 20)
                    {
                        creditRisk += .15m;
                    }
                    else if (missedPayments.Count() > 15)
                    {
                        creditRisk += .13m;
                    }
                    else if (missedPayments.Count() > 10)
                    {
                        creditRisk += .10m;
                    }
                    else if (missedPayments.Count() > 5)
                    {
                        creditRisk += .05m;
                    }
                    else if (missedPayments.Count() > 0)
                    {
                        creditRisk += .03m;
                    }
                }
                else if (totalMonthlyPaymentAmounts > 2500)
                {
                    creditRisk += .30m;
                    if (missedPayments.Count() > 20)
                    {
                        creditRisk += .15m;
                    }
                    else if (missedPayments.Count() > 15)
                    {
                        creditRisk += .13m;
                    }
                    else if (missedPayments.Count() > 10)
                    {
                        creditRisk += .10m;
                    }
                    else if (missedPayments.Count() > 5)
                    {
                        creditRisk += .05m;
                    }
                    else if (missedPayments.Count() > 0)
                    {
                        creditRisk += .03m;
                    }
                }
                else if (totalMonthlyPaymentAmounts > 1500)
                {
                    creditRisk += .20m;
                    if (missedPayments.Count() > 20)
                    {
                        creditRisk += .15m;
                    }
                    else if (missedPayments.Count() > 15)
                    {
                        creditRisk += .13m;
                    }
                    else if (missedPayments.Count() > 10)
                    {
                        creditRisk += .10m;
                    }
                    else if (missedPayments.Count() > 5)
                    {
                        creditRisk += .05m;
                    }
                    else if (missedPayments.Count() > 0)
                    {
                        creditRisk += .03m;
                    }
                }
                else if (totalMonthlyPaymentAmounts > 500)
                {
                    creditRisk += .10m;
                    if (missedPayments.Count() > 20)
                    {
                        creditRisk += .15m;
                    }
                    else if (missedPayments.Count() > 15)
                    {
                        creditRisk += .13m;
                    }
                    else if (missedPayments.Count() > 10)
                    {
                        creditRisk += .10m;
                    }
                    else if (missedPayments.Count() > 5)
                    {
                        creditRisk += .05m;
                    }
                    else if (missedPayments.Count() > 0)
                    {
                        creditRisk += .03m;
                    }
                }
                else
                {
                    if (missedPayments.Count() > 20)
                    {
                        creditRisk += .15m;
                    }
                    else if (missedPayments.Count() > 15)
                    {
                        creditRisk += .13m;
                    }
                    else if (missedPayments.Count() > 10)
                    {
                        creditRisk += .10m;
                    }
                    else if (missedPayments.Count() > 5)
                    {
                        creditRisk += .05m;
                    }
                    else if (missedPayments.Count() > 0)
                    {
                        creditRisk += .03m;
                    }
                }
            }
            else if (creditUtiltizationRatio > .50m)
            {
                creditRisk += .1m;
                if (totalMonthlyPaymentAmounts > 5000)
                {
                    creditRisk += .47m;
                    if (missedPayments.Count() > 20)
                    {
                        creditRisk += .17m;
                    }
                    else if (missedPayments.Count() > 15)
                    {
                        creditRisk += .15m;
                    }
                    else if (missedPayments.Count() > 10)
                    {
                        creditRisk += .12m;
                    }
                    else if (missedPayments.Count() > 5)
                    {
                        creditRisk += .07m;
                    }
                    else if (missedPayments.Count() > 0)
                    {
                        creditRisk += .05m;
                    }
                }
                else if (totalMonthlyPaymentAmounts > 2500)
                {
                    creditRisk += .31m;
                    if (missedPayments.Count() > 20)
                    {
                        creditRisk += .16m;
                    }
                    else if (missedPayments.Count() > 15)
                    {
                        creditRisk += .14m;
                    }
                    else if (missedPayments.Count() > 10)
                    {
                        creditRisk += .11m;
                    }
                    else if (missedPayments.Count() > 5)
                    {
                        creditRisk += .06m;
                    }
                    else if (missedPayments.Count() > 0)
                    {
                        creditRisk += .04m;
                    }
                }
                else if (totalMonthlyPaymentAmounts > 1500)
                {
                    creditRisk += .20m;
                    if (missedPayments.Count() > 20)
                    {
                        creditRisk += .15m;
                    }
                    else if (missedPayments.Count() > 15)
                    {
                        creditRisk += .13m;
                    }
                    else if (missedPayments.Count() > 10)
                    {
                        creditRisk += .10m;
                    }
                    else if (missedPayments.Count() > 5)
                    {
                        creditRisk += .05m;
                    }
                    else if (missedPayments.Count() > 0)
                    {
                        creditRisk += .03m;
                    }
                }
                else if (creditUtiltizationRatio > .25m)
                {
                    creditRisk += .1m;
                    if (totalMonthlyPaymentAmounts > 5000)
                    {
                        creditRisk += .42m;
                        if (missedPayments.Count() > 20)
                        {
                            creditRisk += .17m;
                        }
                        else if (missedPayments.Count() > 15)
                        {
                            creditRisk += .15m;
                        }
                        else if (missedPayments.Count() > 10)
                        {
                            creditRisk += .12m;
                        }
                        else if (missedPayments.Count() > 5)
                        {
                            creditRisk += .07m;
                        }
                        else if (missedPayments.Count() > 0)
                        {
                            creditRisk += .05m;
                        }
                    }
                    else if (totalMonthlyPaymentAmounts > 2500)
                    {
                        creditRisk += .26m;
                        if (missedPayments.Count() > 20)
                        {
                            creditRisk += .16m;
                        }
                        else if (missedPayments.Count() > 15)
                        {
                            creditRisk += .14m;
                        }
                        else if (missedPayments.Count() > 10)
                        {
                            creditRisk += .11m;
                        }
                        else if (missedPayments.Count() > 5)
                        {
                            creditRisk += .06m;
                        }
                        else if (missedPayments.Count() > 0)
                        {
                            creditRisk += .04m;
                        }
                    }
                    else if (totalMonthlyPaymentAmounts > 1500)
                    {
                        creditRisk += .15m;
                        if (missedPayments.Count() > 20)
                        {
                            creditRisk += .15m;
                        }
                        else if (missedPayments.Count() > 15)
                        {
                            creditRisk += .13m;
                        }
                        else if (missedPayments.Count() > 10)
                        {
                            creditRisk += .10m;
                        }
                        else if (missedPayments.Count() > 5)
                        {
                            creditRisk += .05m;
                        }
                        else if (missedPayments.Count() > 0)
                        {
                            creditRisk += .03m;
                        }
                    }
                }
            }

            return creditRisk;
        }

        public IEnumerable<InterestRate> GetInterestRates()
        {
            var interestRates = new List<InterestRate>();
            CreateDatabaseIfNotExists();
            using (var conn = GetDbConnection())
            {
                var command = conn.CreateCommand();
                command.CommandText = "SELECT * FROM InterestRates";
                command.CommandType = CommandType.Text;


                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var interestRate = new InterestRate()
                        {
                            MaxRiskRating = (decimal)(double)reader[1],
                            Rate = (decimal)(double)reader[2]
                        };

                        interestRates.Add(interestRate);
                    }
                }
            }

            return interestRates;
        }

        public decimal GetInterestRateForUser(int userId)
        {
            CreateDatabaseIfNotExists();

            using (var conn = GetDbConnection())
            {
                var command = conn.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM Users WHERE UserId = @UserId";
                command.Parameters.AddWithValue("@UserId", userId);

                using (var userReader = command.ExecuteReader())
                {
                    if (userReader.Read())
                    {
                        var user = new User()
                        {
                            UserId = (int)userReader["Id"],
                            Name = (string)userReader["Name"],
                            TotalCreditAvailable = (decimal)userReader["TotalAvailableCredit"],
                            CreditUtilized = (decimal)userReader["CreditUtilized"],
                            AnnualIncome = (decimal)userReader["AnnualIncome"]
                        };

                        var risk = CalculateRisk(user.TotalCreditAvailable, user.CreditUtilized,
                            Enumerable.Empty<MissedPayment>().ToList(), user.TotalMonthlyPayments, user.AnnualIncome,
                            user.TotalAssets);

                        return risk;
                    }

                    return -1;
                }
            }
        }

        //Ben didn't write this code, but it's necessary to preload the data
        private void CreateDatabaseIfNotExists()
        {
            var databaseFileNamePath = GetDatabaseFileNamePath();
            if (!File.Exists(databaseFileNamePath))
            {
                SQLiteConnection.CreateFile(databaseFileNamePath);
                using (var connection = GetDbConnection())
                {
                    var command = new SQLiteCommand("CREATE TABLE InterestRates (Id INTEGER PRIMARY KEY AUTOINCREMENT, MaxRiskRating REAL, Rate REAL)", connection);
                    command.ExecuteNonQuery();

                    command = new SQLiteCommand("CREATE TABLE Users (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name Text, TotalAvailableCredit REAL, CreditUtilized REAL, AnnualIncome REAL, TotalMonthlyPayments REAL, TotalAssets REAL)", connection);
                    command.ExecuteNonQuery();

                    var random = new Random();

                    for (var i = 1m; i <= 100; i++)
                    {
                        command = connection.CreateCommand();

                        command.CommandText = "INSERT INTO InterestRates (MaxRiskRating, Rate) VALUES (@MaxRiskRating, @Rate)";
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@MaxRiskRating", i / 100);
                        command.Parameters.AddWithValue("@Rate", i / 2 / 100);
                        command.ExecuteNonQuery();
                    }

                    var fakeUsers = new Faker<User>()
                        .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
                        .RuleFor(u => u.TotalCreditAvailable, f => Math.Round(f.Random.Decimal(0, 20000), 2))
                        .RuleFor(u => u.CreditUtilized,
                            (f, u) => Math.Round(f.Random.Decimal(0, u.TotalCreditAvailable), 2))
                        .RuleFor(u => u.AnnualIncome, f => Math.Round(f.Random.Decimal(10000, 200000), 2))
                        .RuleFor(u => u.TotalMonthlyPayments, f => Math.Round(f.Random.Decimal(0, 2500), 2))
                        .RuleFor(u => u.TotalAssets, f => Math.Round(f.Random.Decimal(10000, 2000000), 2));
                    for (var i = 0; i < 100; i++)
                    {
                        var user = fakeUsers.Generate();
                        command.CommandText = "INSERT INTO Users (Name, TotalAvailableCredit, CreditUtilized, AnnualIncome, TotalMonthlyPayments, TotalAssets) VALUES (@Name, @TotalAvailableCredit, @CreditUtilized, @AnnualIncome, @TotalMonthlyPayments, @TotalAssets)";
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Name", user.Name);
                        command.Parameters.AddWithValue("@TotalAvailableCredit", user.TotalCreditAvailable);
                        command.Parameters.AddWithValue("@CreditUtilized", user.CreditUtilized);
                        command.Parameters.AddWithValue("@AnnualIncome", user.AnnualIncome);
                        command.Parameters.AddWithValue("@TotalMonthlyPayments", user.TotalMonthlyPayments);
                        command.Parameters.AddWithValue("@TotalAssets", user.TotalAssets);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private static SQLiteConnection GetDbConnection()
        {
            var databaseFileNamePath = GetDatabaseFileNamePath();
            var connection = new SQLiteConnection($"Data Source={databaseFileNamePath};version=3");
            connection.Open();
            return connection;
        }

        private static string GetDatabaseFileNamePath()
        {
            var databaseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LoanCalc");
            if (!Directory.Exists(databaseDirectory))
                Directory.CreateDirectory(databaseDirectory);

            var databaseFileNamePath = Path.Combine(databaseDirectory, "Loan.sqlite");
            Console.WriteLine(databaseFileNamePath);
            return databaseFileNamePath;
        }
    }
}