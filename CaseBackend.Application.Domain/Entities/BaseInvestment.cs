using CaseBackend.Application.Domain.Enum;

namespace CaseBackend.Application.Domain.Entities
{
    public class BaseInvestment
    {
        public BaseInvestment()
        {
        }

        public string Nome { get; set; }

        public TypeOfInvestment InvestmentType { get; set; }

        public double IncomeTaxDiscountPercentage
        {
            get
            {
                double taxPercentage = 0d;

                switch (this.InvestmentType)
                {
                    case TypeOfInvestment.Funds:
                        taxPercentage = 15d / 100d;
                        break;
                    case TypeOfInvestment.LCI:
                        taxPercentage = 5d / 100d;
                        break;
                    case TypeOfInvestment.DirectTreasure:
                        taxPercentage = 10d / 100d;
                        break;
                    default:
                        break;
                }

                return taxPercentage;
            }
        }
    }
}
