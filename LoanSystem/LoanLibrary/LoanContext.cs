using System;
using Bogus;
using Bogus.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LoanLibrary
{
    internal class LoanContext : DbContext
    {
        public LoanContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<InterestRate> InterestRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var userId = 0;
            var fakeUsers = new Faker<User>()
                .RuleFor(u => u.Id, f => ++userId)
                .RuleFor(u => u.Name, (f, u) => f.Name.FullName())
                .RuleFor(u => u.TotalCreditAvailable, f => Math.Round(f.Random.Decimal(0, 20000), 2))
                .RuleFor(u => u.CreditUtilized, (f, u) => Math.Round(f.Random.Decimal(0, u.TotalCreditAvailable), 2))
                .RuleFor(u => u.AnnualIncome, f => Math.Round(f.Random.Decimal(10000, 200000), 2))
                .RuleFor(u => u.TotalMonthlyPayments, f => Math.Round(f.Random.Decimal(0, 2500), 2))
                .RuleFor(u => u.TotalAssets, f => Math.Round(f.Random.Decimal(10000, 2000000), 2));
            modelBuilder.Entity<User>().HasData(fakeUsers.GenerateBetween(25, 100));


            var idValue = 0;
            var fakeInterestRate = new Faker<InterestRate>()
                .RuleFor(i => i.Id, f => ++idValue)
                .RuleFor(i => i.MaxRiskRating, f => f.Random.Decimal())
                .RuleFor(i => i.Rate, f => f.Random.Decimal(0, .5m));
            modelBuilder.Entity<InterestRate>().HasData(fakeInterestRate.GenerateBetween(5, 10));
        }
    }
}