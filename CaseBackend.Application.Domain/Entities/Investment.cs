using System;

namespace CaseBackend.Application.Domain.Entities
{
    public class Investment : BaseInvestment
    {
        public Investment()
        {

        }

        public DateTime PurchaseDate { get; set; }

        public DateTime DueDate { get; set; }

        public double InvestedAmount { get; set; }

        public double TotalAmount { get; set; }

        public double Profitability => this.TotalAmount - this.InvestedAmount;

        public double IncomeTax
        {
            get
            {
                // Calcular IR
                double incomeTax = (this.Profitability * this.IncomeTaxDiscountPercentage);

                return incomeTax;
            }
        }

        /// <summary>
        /// Retorna valor de resgate do investimento
        /// </summary>
        /// <returns>Valor de resgate</returns>
        public double GetRedemptionValue()
        {
            TimeSpan totalCustodyTime = this.DueDate.Subtract(this.PurchaseDate);            
            TimeSpan currentCustoryTime = DateTime.Now.Date.Subtract(this.PurchaseDate);

            double monthsToExpire = this.DueDate.Subtract(DateTime.Now.Date).Days / (365.25 / 12);
            double totalMonths = totalCustodyTime.Days / (365.25 / 12);
            double currentMonths = currentCustoryTime.Days / (365.25 / 12);

            double discountPercentage;

            if (currentMonths >= (totalMonths / 2)
                && monthsToExpire > 3)
            {
                // Investimento com mais da metade do tempo em custódia: Perde 15% do valor investido
                discountPercentage = 15d / 100d;
            }
            else if (monthsToExpire >= 0 && monthsToExpire <= 3)
            {
                // Investimento com até 3 meses para vencer: Perde 6% do valor investido
                discountPercentage = 6d / 100d;
            }
            else
            {
                // Outros: Perde 30% do valor investido
                discountPercentage = 30d / 100d;
            }

            return this.InvestedAmount - (this.InvestedAmount * discountPercentage);
        }
    }
}
