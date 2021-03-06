using CaseBackend.Application.Domain.Enum;
using System;

namespace CaseBackend.Application.Domain.Entities
{
    public class Funds : BaseInvestment
    {
        public Funds()
        {
            this.InvestmentType = TypeOfInvestment.Funds;
        }

        public DateTime DataCompra { get; set; }

        public DateTime DataResgate { get; set; }

        public double CapitalInvestido { get; set; }

        public double ValorAtual { get; set; }
    }
}
