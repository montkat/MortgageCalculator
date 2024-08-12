using NUnit.Framework;
using MortgageCalculator;

namespace MortgageCalcTest
{
    [TestFixture]
    public class Mortgage_CalcTest
    {
        [Test]
        public void CalculateTotalMonthlyPayment_ShouldReturnCorrectValue()
        {
            // Arrange
            double amountLoaned = 100000;
            double interestRate = 3.75;
            double numberOfYears = 30;
            DateTime firstPayment = new DateTime(2024, 08, 08);
            var calculator = new Mortgage_Calc(amountLoaned, interestRate, numberOfYears, firstPayment);

            // Act
            calculator.CalculateTotalMonthlyPayment();
            calculator.CalculatePayoffDate();

            // Assert
            Assert.AreEqual(463.12, calculator.TotalMonthlyPayment, 0.01);
            Assert.AreEqual(firstPayment, calculator.FirstPayment);
        }


        [Test]
        public void CalculateInterestPayment_ShouldReturnCorrectValue()
        {
            // Arrange
            double amountLoaned = 100000;
            double interestRate = 5;
            int numberOfYears = 30;
            DateTime firstPayment = new DateTime(2025, 08, 08);
            var calculator = new Mortgage_Calc(amountLoaned, interestRate, numberOfYears, firstPayment);
            calculator.CalculateTotalMonthlyPayment();

            // Act
            calculator.CalculateInterestPayment();
            calculator.CalculatePayoffDate();

            // Assert
            Assert.AreEqual(416.67, calculator.InterestPayment, 0.01);
            Assert.AreEqual(firstPayment, calculator.FirstPayment);
        }

        [Test]
        public void CalculatePrincipalPayment_ShouldReturnCorrectValue()
        {
            // Arrange
            double amountLoaned = 100000;
            double interestRate = 5;
            int numberOfYears = 30;
            DateTime firstPayment = new DateTime(2026, 08, 08);
            var calculator = new Mortgage_Calc(amountLoaned, interestRate, numberOfYears, firstPayment);
            calculator.CalculateTotalMonthlyPayment();
            calculator.CalculateInterestPayment();

            // Act
            calculator.CalculatePrincipalPayment();
            calculator.CalculatePayoffDate();

            // Assert
            Assert.AreEqual(120.15, calculator.PrincipalPayment, 0.01);
            Assert.AreEqual(firstPayment, calculator.FirstPayment);
        }

        [Test]
        public void UpdateRemainingBalance_ShouldReturnCorrectValue()
        {
            // Arrange
            double amountLoaned = 100000;
            double interestRate = 5;
            int numberOfYears = 30;
            DateTime firstPayment = new DateTime(2027, 08, 08);
            var calculator = new Mortgage_Calc(amountLoaned, interestRate, numberOfYears, firstPayment);
            calculator.CalculateTotalMonthlyPayment();
            calculator.CalculateInterestPayment();
            calculator.CalculatePrincipalPayment();

            // Act
            calculator.UpdateRemainingBalance();
            calculator.CalculatePayoffDate();

            // Assert
            Assert.AreEqual(99879.85, calculator.RemainingBalance, 0.01);
            Assert.AreEqual(firstPayment, calculator.FirstPayment);
        }
    }
}