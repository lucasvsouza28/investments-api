using CaseBackend.Application.Domain.Enum;
using System;

namespace CaseBackend.Application.Domain.Entities
{
    public class TesouroDireto : BaseInvestment
    {
        public TesouroDireto()
        {
            this.InvestmentType = TypeOfInvestment.DirectTreasure;
        }

        public double ValorInvestido { get; set; }

        public double ValorTotal { get; set; }

        public DateTime DataDeCompra { get; set; }

        public DateTime Vencimento { get; set; }
    }
}
