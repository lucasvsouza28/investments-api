using CaseBackend.Application.Domain.Entities;
using CaseBackend.Application.Domain.Enum;
using System.Collections.Generic;

namespace CaseBackend.Application.Query.Adapters
{
    internal static class FundosAdapter
    {
        internal static Investment ToInvestment(this Fundos entity)
        {
            if (entity == null) return null;

            return new Investment
            {
                PurchaseDate = entity.DataCompra,
                DueDate = entity.DataResgate,
                InvestedAmount = entity.CapitalInvestido,
                TotalAmount = entity.ValorAtual,
                Nome = entity.Nome,
                InvestmentType = TypeOfInvestment.Funds
            };
        }

        internal static IEnumerable<Investment> ToInvestment(this IEnumerable<Fundos> entities)
        {
            if (entities == null) yield return null;

            foreach (var item in entities)
            {
                yield return item.ToInvestment();
            }
        }
    }
}
