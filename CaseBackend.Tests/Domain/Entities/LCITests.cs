using System;
using System.Collections.Generic;
using System.Text;
using CaseBackend.Application.Domain.Entities;
using CaseBackend.Application.Domain.Enum;
using NUnit.Framework;

namespace CaseBackend.Tests.Domain.Entities
{
    public class LCITests
    {
        [Test]
        public void Should_Have_InvestmentType_Equals_LCI()
        {
            LCI entity = new LCI();

            Assert.Multiple(() => {
                Assert.IsNotNull(entity);
                Assert.AreEqual(entity.InvestmentType, TypeOfInvestment.LCI);
            });
        }

        [Test]
        public void Should_Have_IncomeTax_Equals_0_5_Percent()
        {
            LCI entity = new LCI();

            Assert.Multiple(() => {
                Assert.IsNotNull(entity);
                Assert.AreEqual(entity.IncomeTaxDiscountPercentage, 5d / 100d);
            });
        }
    }
}
