using System;
using System.Collections.Generic;
using System.Text;
using CaseBackend.Application.Domain.Entities;
using CaseBackend.Application.Domain.Enum;
using NUnit.Framework;


namespace CaseBackend.Tests.Domain.Entities
{
    public class TesouroDiretoTests
    {
        [Test]
        public void Should_Have_InvestmentType_Equals_DirectTreasure()
        {
            DirectTreasure entity = new DirectTreasure();

            Assert.Multiple(() => {
                Assert.IsNotNull(entity);
                Assert.AreEqual(entity.InvestmentType, TypeOfInvestment.DirectTreasure);
            });
        }

        [Test]
        public void Should_Have_IncomeTax_Equals_0_10_Percent()
        {
            DirectTreasure entity = new DirectTreasure();

            Assert.Multiple(() => {
                Assert.IsNotNull(entity);
                Assert.AreEqual(entity.IncomeTaxDiscountPercentage, 10d / 100d);
            });
        }
    }
}
