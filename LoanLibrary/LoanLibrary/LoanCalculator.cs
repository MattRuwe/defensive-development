using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace LoanLibrary
{
    public class LoanCalculator
    {
        private readonly decimal _loanAmount;
        private readonly decimal _interestRate;
        private readonly int _termInMonths;

        public LoanCalculator(decimal loanAmount, decimal interestRate, int termInMonths)
        {
            _termInMonths = termInMonths;
            _interestRate = interestRate;
            _loanAmount = loanAmount;
        }

        public IEnumerable<Payment> GetAmoritization()
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

        public decimal CalculateMonthlyPayment(decimal principleBalance, int term, decimal interestRate)
        {
            var monthlyInterestRate = (double)(interestRate / 12);
            var payment = (monthlyInterestRate * (double)principleBalance) / (1 - Math.Pow(1 + monthlyInterestRate, term * -1));
            return (decimal)Math.Round(payment, 2);
        }


        //This method was added later and doesn't work well with the current constructor

        public decimal CalculateRisk(decimal currentAvailableCredit, decimal currentUtilizedCredit, IEnumerable<MissedPayment> missedPayments, decimal totalMonthlyPaymentAmounts, decimal annualIncome, decimal totalAssets)
        {
            //Check for credit utilized greater than current available credit


            decimal creditUtiltizationRatio = currentUtilizedCredit / currentAvailableCredit;
            decimal debtToIncomeRatio = totalMonthlyPaymentAmounts / (annualIncome / 12);

            var creditRisk = 0m;
            if (creditUtiltizationRatio > .75m)
            {
                creditRisk += .1m;
                if(totalMonthlyPaymentAmounts > 5000)
                {
                    creditRisk += .45m;
                    if(missedPayments.Count() > 20)
                    {
                        creditRisk += .15m;
                    }
                    else if(missedPayments.Count() > 15)
                    {
                        creditRisk += .13m;
                    }
                    else if(missedPayments.Count() > 10)
                    {
                        creditRisk += .10m;
                    }
                    else if(missedPayments.Count() > 5)
                    {
                        creditRisk += .05m;
                    }
                    else if (missedPayments.Count() > 0)
                    {
                        creditRisk += .03m;
                    }
                }
                else if(totalMonthlyPaymentAmounts > 2500)
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
            }
            else if(creditUtiltizationRatio > .50m)
            {

            }


            return creditRisk;
        }

        public IEnumerable<InterestRate> GetInterestRates()
        {
            var interestRates = new List<InterestRate>();
            CreateDatabaseIfNotExists();
            using(var conn = GetDbConnection())
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

        //Abstract data access

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

                    var random = new Random();

                    for (var i = 0; i < 100; i++)
                    {
                        command = connection.CreateCommand();

                        command.CommandText = "INSERT INTO InterestRates (MaxRiskRating, Rate) VALUES (@MaxRiskRating, @Rate)";
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@MaxRiskRating", random.NextDouble());
                        command.Parameters.AddWithValue("@Rate", random.NextDouble());
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