using CaseBackend.Application.Domain.Entities;
using CaseBackend.Application.Domain.Enum;
using System.Collections.Generic;

namespace CaseBackend.Application.Query.Adapters
{
    internal static class DirectTreasureAdapter
    {
        internal static Investment ToInvestment(this DirectTreasure entity)
        {
            if (entity == null) return null;

            return new Investment
            {
                PurchaseDate = entity.DataDeCompra,
                DueDate = entity.Vencimento,
                InvestedAmount = entity.ValorInvestido,
                TotalAmount = entity.ValorTotal,
                Nome = entity.Nome,
                InvestmentType = TypeOfInvestment.DirectTreasure
            };
        }

        internal static IEnumerable<Investment> ToInvestment(this IEnumerable<DirectTreasure> entities)
        {
            if (entities == null) yield return null;

            foreach (var item in entities)
            {
                yield return item.ToInvestment();
            }
        }
    }
}
