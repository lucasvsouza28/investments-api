using System;
using CaseBackend.Application.Domain.Entities;
using CaseBackend.Application.Domain.Enum;
using NUnit.Framework;

namespace CaseBackend.Tests.Domain.Entities
{
    public class InvestmentTests
    {
        [Test]
        [TestCase(100d, 100d)]
        [TestCase(200d, 100d)]
        [TestCase(200d, 300d)]
        [TestCase(0d, 0d)]
        public void Should_Have_Profitability_Equal_TotalAmount_Minus_InvestedAmount(double totalAmount, double investedAmount)
        {
            Investment entity = new Investment();
            entity.TotalAmount = totalAmount;
            entity.InvestedAmount = investedAmount;

            double profitability = totalAmount - investedAmount;

            Assert.Multiple(() => {
                Assert.AreEqual(entity.Profitability, profitability);
            });
        }

        [Test]
        [TestCase(TypeOfInvestment.DirectTreasure, 300d, 310d)]
        [TestCase(TypeOfInvestment.Funds, 300d, 310d)]
        [TestCase(TypeOfInvestment.LCI, 300d, 310d)]
        public void Should_Have_Valid_IncomeTax(TypeOfInvestment investmentType, double totalAmount, double investedAmount)
        {
            Investment entity = new Investment();
            entity.InvestmentType = investmentType;
            entity.TotalAmount = totalAmount;
            entity.InvestedAmount = investedAmount;

            double rentability = totalAmount - investedAmount;
            double incomeTax = (rentability * (investmentType == TypeOfInvestment.DirectTreasure ? 10 :
                                                       investmentType == TypeOfInvestment.Funds ? 15 : 5) / 100);

            Assert.Multiple(() => {
                Assert.AreEqual(entity.IncomeTax, incomeTax);
            });
        }

        [Test]
        [TestCase(100d, 3)]
        [TestCase(100d, 2)]
        [TestCase(100d, 1)]
        public void Should_Discount_6_Percent_Of_InvestedAmount(double investedAmount, int monthsDifferenceFromDueDate)
        {
            Investment entity = new Investment();
            entity.InvestedAmount = investedAmount;
            entity.PurchaseDate = DateTime.Now.AddYears(-1);
            entity.DueDate = DateTime.Now.AddMonths(monthsDifferenceFromDueDate).AddDays(-1);

            double redemptionValue = entity.GetRedemptionValue();
            double expectedRedemptionValue = investedAmount - (investedAmount * (6d / 100d));

            Assert.AreEqual(redemptionValue, expectedRedemptionValue);
        }

        [Test]
        [TestCase(100d)]
        public void Should_Discount_15_Percent_Of_InvestedAmount(double investedAmount)
        {
            Investment entity = new Investment();
            entity.InvestedAmount = investedAmount;
            entity.PurchaseDate = DateTime.Now.AddMonths(-7);
            entity.DueDate = DateTime.Now.AddMonths(6);

            double redemptionValue = entity.GetRedemptionValue();
            double expectedRedemptionValue = investedAmount - (investedAmount * (15d / 100d));

            Assert.AreEqual(expectedRedemptionValue, redemptionValue);
        }

        [Test]
        [TestCase(100d, -7)]
        [TestCase(100d, -8)]
        [TestCase(100d, -9)]
        public void Should_Discount_30_Percent_Of_InvestedAmount(double investedAmount, int monthsDifferenceFromDueDate)
        {
            Investment entity = new Investment();
            entity.InvestedAmount = investedAmount;
            entity.PurchaseDate = DateTime.Now.AddYears(-1);
            entity.DueDate = DateTime.Now.AddMonths(monthsDifferenceFromDueDate);

            double redemptionValue = entity.GetRedemptionValue();
            double expectedRedemptionValue = investedAmount - (investedAmount * (30d / 100d));

            Assert.AreEqual(redemptionValue, expectedRedemptionValue);
        }
    }
}
