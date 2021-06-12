using CaseBackend.Application.Domain.Entities;
using CaseBackend.Application.Domain.Enum;
using System.Collections.Generic;

namespace CaseBackend.Application.Query.Adapters
{
    internal static class LCIAdapter
    {
        internal static Investment ToInvestment(this LCI entity)
        {
            if (entity == null) return null;

            return new Investment
            {
                PurchaseDate = entity.DataOperacao,
                DueDate = entity.Vencimento,
                InvestedAmount = entity.CapitalInvestido,
                TotalAmount = entity.CapitalAtual,
                Nome = entity.Nome,
                InvestmentType = TypeOfInvestment.LCI
            };
        }

        internal static IEnumerable<Investment> ToInvestment(this IEnumerable<LCI> entities)
        {
            if (entities == null) yield return null;

            foreach (var item in entities)
            {
                yield return item.ToInvestment();
            }
        }
    }
}
