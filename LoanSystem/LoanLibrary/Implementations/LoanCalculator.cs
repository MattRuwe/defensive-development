using System;
using System.Collections.Generic;
using System.Linq;
using LoanLibrary.Contracts;
using LoanLibrary.DataContracts;

namespace LoanLibrary.Implementations
{
    class LoanCalculator : ILoanCalculator
    {
        private IDataAccess _dataAccess;

        public LoanCalculator(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public List<Payment> GetAmoritization(decimal loanAmount, decimal interestRate, int termInMonths)
        {
            var currentPricinpalBalance = loanAmount;
            var returnValue = new List<Payment>();
            var paymentAmount = CalculateMonthlyPayment(loanAmount, interestRate, termInMonths);
            for (int i = 1; i <= termInMonths; i++)
            {
                var interestAmount = currentPricinpalBalance * (interestRate / 12);
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

        public decimal CalculateMonthlyPayment(decimal loanAmount, decimal interestRate, int termInMonths)
        {
            var monthlyInterestRate = (double)(interestRate / 12);
            var payment = (monthlyInterestRate * (double)loanAmount) / (1 - Math.Pow(1 + monthlyInterestRate, termInMonths * -1));
            return (decimal)Math.Round(payment, 2);
        }

        public decimal CalculateRisk(RiskFactors riskFactors)
        {
            //Check for credit utilized greater than current available credit


            decimal creditUtiltizationRatio = riskFactors.CurrentUtilizedCredit / riskFactors.CurrentAvailableCredit;
            decimal debtToIncomeRatio = riskFactors.TotalMonthlyPaymentAmounts / (riskFactors.AnnualIncome / 12);

            var creditRisk = 0m;
            if (creditUtiltizationRatio > .75m)
            {
                creditRisk += .1m;
                if (riskFactors.TotalMonthlyPaymentAmounts > 5000)
                {
                    creditRisk += .45m;
                    if (riskFactors.MissedPayments.Count() > 20)
                    {
                        creditRisk += .15m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 15)
                    {
                        creditRisk += .13m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 10)
                    {
                        creditRisk += .10m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 5)
                    {
                        creditRisk += .05m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 0)
                    {
                        creditRisk += .03m;
                    }
                }
                else if (riskFactors.TotalMonthlyPaymentAmounts > 2500)
                {
                    creditRisk += .30m;
                    if (riskFactors.MissedPayments.Count() > 20)
                    {
                        creditRisk += .15m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 15)
                    {
                        creditRisk += .13m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 10)
                    {
                        creditRisk += .10m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 5)
                    {
                        creditRisk += .05m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 0)
                    {
                        creditRisk += .03m;
                    }
                }
                else if (riskFactors.TotalMonthlyPaymentAmounts > 1500)
                {
                    creditRisk += .20m;
                    if (riskFactors.MissedPayments.Count() > 20)
                    {
                        creditRisk += .15m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 15)
                    {
                        creditRisk += .13m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 10)
                    {
                        creditRisk += .10m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 5)
                    {
                        creditRisk += .05m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 0)
                    {
                        creditRisk += .03m;
                    }
                }
                else if (riskFactors.TotalMonthlyPaymentAmounts > 500)
                {
                    creditRisk += .10m;
                    if (riskFactors.MissedPayments.Count() > 20)
                    {
                        creditRisk += .15m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 15)
                    {
                        creditRisk += .13m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 10)
                    {
                        creditRisk += .10m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 5)
                    {
                        creditRisk += .05m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 0)
                    {
                        creditRisk += .03m;
                    }
                }
                else
                {
                    if (riskFactors.MissedPayments.Count() > 20)
                    {
                        creditRisk += .15m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 15)
                    {
                        creditRisk += .13m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 10)
                    {
                        creditRisk += .10m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 5)
                    {
                        creditRisk += .05m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 0)
                    {
                        creditRisk += .03m;
                    }
                }
            }
            else if (creditUtiltizationRatio > .50m)
            {
                creditRisk += .1m;
                if (riskFactors.TotalMonthlyPaymentAmounts > 5000)
                {
                    creditRisk += .47m;
                    if (riskFactors.MissedPayments.Count() > 20)
                    {
                        creditRisk += .17m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 15)
                    {
                        creditRisk += .15m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 10)
                    {
                        creditRisk += .12m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 5)
                    {
                        creditRisk += .07m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 0)
                    {
                        creditRisk += .05m;
                    }
                }
                else if (riskFactors.TotalMonthlyPaymentAmounts > 2500)
                {
                    creditRisk += .31m;
                    if (riskFactors.MissedPayments.Count() > 20)
                    {
                        creditRisk += .16m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 15)
                    {
                        creditRisk += .14m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 10)
                    {
                        creditRisk += .11m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 5)
                    {
                        creditRisk += .06m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 0)
                    {
                        creditRisk += .04m;
                    }
                }
                else if (riskFactors.TotalMonthlyPaymentAmounts > 1500)
                {
                    creditRisk += .20m;
                    if (riskFactors.MissedPayments.Count() > 20)
                    {
                        creditRisk += .15m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 15)
                    {
                        creditRisk += .13m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 10)
                    {
                        creditRisk += .10m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 5)
                    {
                        creditRisk += .05m;
                    }
                    else if (riskFactors.MissedPayments.Count() > 0)
                    {
                        creditRisk += .03m;
                    }
                }
                else if (creditUtiltizationRatio > .25m)
                {
                    creditRisk += .1m;
                    if (riskFactors.TotalMonthlyPaymentAmounts > 5000)
                    {
                        creditRisk += .42m;
                        if (riskFactors.MissedPayments.Count() > 20)
                        {
                            creditRisk += .17m;
                        }
                        else if (riskFactors.MissedPayments.Count() > 15)
                        {
                            creditRisk += .15m;
                        }
                        else if (riskFactors.MissedPayments.Count() > 10)
                        {
                            creditRisk += .12m;
                        }
                        else if (riskFactors.MissedPayments.Count() > 5)
                        {
                            creditRisk += .07m;
                        }
                        else if (riskFactors.MissedPayments.Count() > 0)
                        {
                            creditRisk += .05m;
                        }
                    }
                    else if (riskFactors.TotalMonthlyPaymentAmounts > 2500)
                    {
                        creditRisk += .26m;
                        if (riskFactors.MissedPayments.Count() > 20)
                        {
                            creditRisk += .16m;
                        }
                        else if (riskFactors.MissedPayments.Count() > 15)
                        {
                            creditRisk += .14m;
                        }
                        else if (riskFactors.MissedPayments.Count() > 10)
                        {
                            creditRisk += .11m;
                        }
                        else if (riskFactors.MissedPayments.Count() > 5)
                        {
                            creditRisk += .06m;
                        }
                        else if (riskFactors.MissedPayments.Count() > 0)
                        {
                            creditRisk += .04m;
                        }
                    }
                    else if (riskFactors.TotalMonthlyPaymentAmounts > 1500)
                    {
                        creditRisk += .15m;
                        if (riskFactors.MissedPayments.Count() > 20)
                        {
                            creditRisk += .15m;
                        }
                        else if (riskFactors.MissedPayments.Count() > 15)
                        {
                            creditRisk += .13m;
                        }
                        else if (riskFactors.MissedPayments.Count() > 10)
                        {
                            creditRisk += .10m;
                        }
                        else if (riskFactors.MissedPayments.Count() > 5)
                        {
                            creditRisk += .05m;
                        }
                        else if (riskFactors.MissedPayments.Count() > 0)
                        {
                            creditRisk += .03m;
                        }
                    }
                }
            }

            return creditRisk;
        }


        public decimal GetInterestRateForUser(int userId)
        {

            var user = _dataAccess.GetUser(userId);
            var interestRates = _dataAccess.GetInterestRates();


            var riskFactors = new RiskFactors()
            {
                AnnualIncome = user.AnnualIncome,
                CurrentAvailableCredit = user.TotalCreditAvailable,
                CurrentUtilizedCredit = user.CreditUtilized,
                MissedPayments = Enumerable.Empty<MissedPayment>().ToList(),
                TotalMonthlyPaymentAmounts = user.TotalMonthlyPayments,
                TotalAssets = user.TotalAssets
            };

            var risk = CalculateRisk(riskFactors);

            var interestRate = interestRates.OrderBy(_ => _.MaxRiskRating).FirstOrDefault(_ => _.MaxRiskRating <= risk);

            return interestRate?.Rate ?? 1;
        }


    }
}