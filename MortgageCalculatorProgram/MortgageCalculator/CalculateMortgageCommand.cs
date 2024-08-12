using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console.Cli;
using static Spectre.Console.AnsiConsole;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace MortgageCalculator
{
    public class CalculateMortgageCommand : Command<CalculateMortgageCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandArgument(0, "[amountLoaned]")]
            [Description("The total amount of the loan.")]
            public double AmountLoaned { get; set; }

            [CommandArgument(1, "[interestRate]")]
            [Description("The annual interest rate.")]
            public double InterestRate { get; set; }

            [CommandArgument(2, "[numberOfYears]")]
            [Description("The total number of years for the loan term.")]
            public double NumberOfYears { get; set; }

            [CommandArgument(3, "[loanStartDate]")]
            [Description("The start date of the loan (yyyy-MM-dd).")]
            public DateTime LoanStartDate { get; set; }
        }

        public override int Execute(CommandContext context, Settings settings)
        {
            var monthlyRate = settings.InterestRate / 12 / 100;
            var numberOfPayments = settings.NumberOfYears * 12;
            var monthlyPayment = settings.AmountLoaned * monthlyRate / (Math.Pow(1 + monthlyRate, numberOfPayments) - 1);
            var payoffDate = settings.LoanStartDate.AddYears((int)settings.NumberOfYears);

            MarkupLine($"[aquamarine1_1]Amount Loaned:[/] {settings.AmountLoaned:C}");
            MarkupLine($"[aquamarine1_1]Interest Rate:[/] {settings.InterestRate:C}");
            MarkupLine($"[aquamarine1_1]Monthly Payment:[/] {monthlyPayment:C}");
            MarkupLine($"[aquamarine1_1]Start Date:[/] {settings.LoanStartDate:yyyy-MM-dd}");
            MarkupLine($"[aquamarine1_1]Payoff Date:[/] {payoffDate:yyyy-MM-dd}");

            return 0;

        }
    }

}
