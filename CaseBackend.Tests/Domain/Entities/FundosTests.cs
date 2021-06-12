using System;
using System.Collections.Generic;
using System.Text;
using CaseBackend.Application.Domain.Entities;
using CaseBackend.Application.Domain.Enum;
using NUnit.Framework;

namespace CaseBackend.Tests.Domain.Entities
{
    public class FundosTests
    {
        [Test]
        public void Should_Have_InvestmentType_Equals_Funds()
        {
            Fundos entity = new Fundos();

            Assert.Multiple(() => {
                Assert.IsNotNull(entity);
                Assert.AreEqual(entity.InvestmentType, TypeOfInvestment.Funds);
            });
        }

        [Test]
        public void Should_Have_IncomeTax_Equals_0_15_Percent()
        {
            Fundos entity = new Fundos();

            Assert.Multiple(() => {
                Assert.IsNotNull(entity);
                Assert.AreEqual(entity.IncomeTaxDiscountPercentage, 15d / 100d);
            });
        }
    }
}
