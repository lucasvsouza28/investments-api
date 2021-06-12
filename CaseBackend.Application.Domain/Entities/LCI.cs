using CaseBackend.Application.Domain.Enum;
using System;

namespace CaseBackend.Application.Domain.Entities
{
    public class LCI : BaseInvestment
    {
        public LCI()
        {
            this.InvestmentType = TypeOfInvestment.LCI;
        }

        public DateTime DataOperacao { get; set; }

        public DateTime Vencimento { get; set; }

        public double CapitalInvestido { get; set; }

        public double CapitalAtual { get; set; }
    }
}
