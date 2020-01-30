using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Bogus;

namespace LoanLibrary
{
    public class DataAccess
    {
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

        public User GetUser(int userId)
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

                        return user;
                    }
                }
            }

            return null;
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