using System.Security.Principal;

namespace MortgageCalculator
{
    public class Mortgage_Calc
    {
        // The total amount of the loan
        public double AmountLoaned { get; set; }

        // The annual interest rate
        public double InterestRate { get; set; }

        // The total number of years for the loan term
        public double NumberOfYears { get; set; }

        // The calculated monthly Payment
        public double TotalMonthlyPayment { get; private set; }

        // The remaining balance of the loan
        public double RemainingBalance { get; private set; }

        // The interest portion of the monthly payment
        public double InterestPayment { get; private set; }

        // The principal portion of the monthly payment
        public double PrincipalPayment { get; private set; }

        // The loan first payment date
        public DateTime FirstPayment {  get; private set; }


        // Constructor to initialize the properties
        public Mortgage_Calc(double amountLoaned, double interestRate, double numberOfYears, DateTime firstPayment)
        {
            // Validate inputs
            if (amountLoaned <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amountLoaned), "Amount loaned must be greater than zero.");
            }
            if (interestRate <= 0 || interestRate > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(interestRate), "Interest rate must be between 0 and 100.");
            }
            if (numberOfYears <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfYears), "Number of years must be greater than zero.");
            }
            if (firstPayment.Month < 1 || firstPayment.Month > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(firstPayment), "Invalid month. Please enter a valid month (1-12)");
            }
            if (firstPayment.Day < 1 || firstPayment.Day > 31)
            {
                throw new ArgumentOutOfRangeException(nameof(firstPayment), "Invalid day. Please enter a valid day (1-31).");
            }

            AmountLoaned = amountLoaned;
            InterestRate = interestRate;
            NumberOfYears = numberOfYears;
            RemainingBalance = amountLoaned;
            FirstPayment = firstPayment;
        }


        public void CalculateTotalMonthlyPayment()
        {
            double monthlyRate = InterestRate / 1200;
            double numberOfMonths = (NumberOfYears * 12);
            TotalMonthlyPayment = (AmountLoaned * monthlyRate) / (1 - Math.Pow(1 + monthlyRate, -numberOfMonths));
        }
        public void CalculateInterestPayment()
        {
            InterestPayment = RemainingBalance * (InterestRate / 1200);
        }

        public void CalculatePrincipalPayment()
        {
            PrincipalPayment = TotalMonthlyPayment - InterestPayment;
        }

        public void UpdateRemainingBalance()
        {
            RemainingBalance -= PrincipalPayment;
        }

        public DateTime CalculatePayoffDate()
        {
            return FirstPayment.AddYears((int) NumberOfYears);
        }

        public void PerformMonthlyCalculations()
        {
            CalculateTotalMonthlyPayment();
            CalculateInterestPayment();
            CalculatePrincipalPayment();
            UpdateRemainingBalance();
        }
    }
}
