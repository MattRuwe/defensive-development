using System;
using System.IO;
using LoanLibrary.Contracts;
using LoanLibrary.DataModel;
using LoanLibrary.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LoanLibrary
{
    public static class ServiceProviderExtensions
    {
        public static IServiceCollection AddLoanServices(this IServiceCollection value)
        {
            value.AddDbContext<LoanContext>(o =>
            {
                o.UseSqlite($"Data Source={GetDatabaseFileNamePath()}");
                o.EnableSensitiveDataLogging();
            });

            value.AddTransient<ILoanCalculator, LoanCalculator>();
            value.AddTransient<IDataAccess, DataAccess>();

            return value;
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