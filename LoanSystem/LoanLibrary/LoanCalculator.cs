﻿using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}