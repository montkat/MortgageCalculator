using Spectre.Console;
using static Spectre.Console.AnsiConsole;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;
using MortgageCalculator;
using System.Linq.Expressions;
using Spectre.Console.Cli;

namespace MortgageCalculatorApp
{
    class MortgageCalcApp
    {
        public static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                // Interactive mode
                return InteractiveMode();
            }
            else
            {
                // Command-line mode
                var app = new CommandApp();
                app.Configure(config =>
                {
                    config.AddCommand<CalculateMortgageCommand>("calculate");
                });
                return app.Run(args);
            }
        }
        public static int InteractiveMode()
        {
            // Display the application title
            Write(new FigletText("Mortgage Calculator").Centered().Color(Color.Plum4));

            double amountLoaned;
            string formattedAmountLoaned;
            while (true)
            {
                string input = Ask<string>("[aquamarine1_1]Enter the loan amount[/]:");
                if (!string.IsNullOrEmpty(input))
                {
                    input = Regex.Replace(input, @"[^\d.]", ""); // Remove non-numeric characters
                    if (double.TryParse(input, out amountLoaned) && amountLoaned > 0)
                    {
                        formattedAmountLoaned = amountLoaned.ToString("C0", CultureInfo.CurrentCulture); // Format as currency
                        break;
                    }
                }
                MarkupLine("[deeppink3]Invalid input. Please enter a valid loan amount.[/]");
            }

            double interestRate;
            string formattedInterestRate;
            while (true)
            {
                string input = Ask<string>("[aquamarine1_1]Enter the annual interest rate[/]:");
                if (!string.IsNullOrEmpty(input))
                {
                    input = Regex.Replace(input, @"[^\d.]", "");
                    if (double.TryParse(input, out interestRate) && interestRate > 0 && interestRate <= 100)
                    {
                        formattedInterestRate = interestRate.ToString("0.0", CultureInfo.CurrentCulture) + "%"; // Format as percentage
                        break;
                    }
                }
                MarkupLine("[deeppink3]Invalid input. Please enter a valid annual interest rate.[/]");
            }

            double numberOfYears;
            while (true)
            {
                string input = Ask<string>("[aquamarine1_1]Enter number of years[/]:");
                if (!string.IsNullOrEmpty(input) && double.TryParse(input, out numberOfYears) && numberOfYears > 0)
                {
                    break;
                }
                MarkupLine("[deeppink3]Invalid number of years. Please enter a valid number greater than zero.[/]");
            }

            DateTime loanStartDate;
            while (true)
            {
                try
                {
                    string dateInput = Ask<string>("[aquamarine1_1]Loan start date (yyyy-mm-dd)[/]:");
                    if (!string.IsNullOrEmpty(dateInput))
                    {
                        loanStartDate = DateTime.Parse(dateInput);
                        if (loanStartDate > DateTime.Now)
                        {
                            throw new ArgumentOutOfRangeException(nameof(loanStartDate), "Loan start date cannot be in the future.");
                        }
                        break;
                    }
                    MarkupLine("[deeppink3]Invalid input. Please enter a valid date.[/]");
                }
                catch (FormatException)
                {
                    MarkupLine("[deeppink3]Invalid date format. Please enter the date in the format yyyy-mm-dd.[/]");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    MarkupLine($"[deeppink3]{ex.Message}[/]");
                }
            }
            try
            {
                var calculator = new Mortgage_Calc(amountLoaned, interestRate, numberOfYears, loanStartDate);
                Progress()
                    .Start(ctx =>
                    {
                        var task = ctx.AddTask("[aquamarine1_1]Calculating mortgage details...[/]");
                        for (int i = 0; i < 12; i++)
                        {
                            if (ctx.IsFinished)
                                break;

                            calculator.PerformMonthlyCalculations();
                            task.Increment(100 / 12.0); //Use 12.0 to ensure floating-point division
                            Thread.Sleep(100); //Simulate work
                        }
                        task.Value = 100; // Ensures the progress bar reaches 100%
                    });
                var payOffDate = calculator.CalculatePayoffDate();

                // Create a table
                var table = new Table();
                table.AddColumn("Description");
                table.AddColumn("Amount");

                // Add rows to the table
                table.AddRow("[aquamarine1_1]Total Monthly Payment[/]", $"{calculator.TotalMonthlyPayment:C}");
                table.AddRow("[aquamarine1_1]Interest Payment[/]", $"{calculator.InterestPayment:C}");
                table.AddRow("[aquamarine1_1]Principal Payment[/]", $"{calculator.PrincipalPayment:C}");
                table.AddRow("[aquamarine1_1]Remaining Balance[/]", $"{calculator.RemainingBalance:C}");
                table.AddRow("[aquamarine1_1]Estimated Payoff Date[/]", $"{payOffDate:yyyy-MM-dd}");
                table.AddRow("[aquamarine1_1]Amount Loaned[/]", formattedAmountLoaned);
                table.AddRow("[aquamarine1_1]Annual Interest Rate[/]", formattedInterestRate);

                // Render the table
                Write(table);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MarkupLine($"[deeppink3]Error: {ex.Message}[/]");
            }
            return 0;
        }
    }
}


