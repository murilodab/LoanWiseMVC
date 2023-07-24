using LoanWiseMVC.Models;

namespace LoanWiseMVC.Helpers
{
    public class LoanHelper
    {
        public Loan GetPayments(Loan loan)
        {

            //Calculate Monthly Payment

            loan.Payment = CalcPayment(loan.Amount, loan.Rate, loan.Term);

            //Create a Loop from, 1 to the Term

            var balance = loan.Amount;
            var totalInterest = 0.0m;
            var monthlyInterest = 0.0m;
            var monthlyPrincipal = 0.0m;
            var monthlyRate = CalcMonthlyRate(loan.Rate);

            //Loop over each month until hit the term of the loan
            for (int month = 1; month < loan.Term; month++)
            {
                monthlyInterest = CalcMonthlyInterest(balance, monthlyRate);
                totalInterest += monthlyInterest;
                monthlyPrincipal = loan.Payment - monthlyInterest;
                balance -= monthlyPrincipal;

                LoanPayment loanPayment = new();

                loanPayment.Month = month;
                loanPayment.Payment = loan.Payment;
                loanPayment.MonthlyPrincipal= monthlyPrincipal;
                loanPayment.MonthlyInterest = monthlyInterest;
                loanPayment.TotalInterest = totalInterest;
                loanPayment.Balance = balance;

                //push the object into the loan model
                loan.Payments.Add(loanPayment);


            }

            loan.TotalInterest = totalInterest;
            loan.TotalCost = loan.Amount + totalInterest;

          

            //return the loan to the view

            return loan;
        }

        private decimal CalcPayment(decimal amount, decimal rate, int term)
        {

            decimal monthlyRate = CalcMonthlyRate(rate);

            var rateD = Convert.ToDouble(monthlyRate);

            var amountD = Convert.ToDouble(amount);

            var paymentD = (amountD * rateD) / (1 - Math.Pow(1 + rateD, -term));

            return Convert.ToDecimal(paymentD);
        }

        private decimal CalcMonthlyRate(decimal rate)
        {

            return rate / 1200;
        }

        private decimal CalcMonthlyInterest(decimal balance, decimal monthlyRate)
        {
            return balance * monthlyRate;
        }
    }
}
