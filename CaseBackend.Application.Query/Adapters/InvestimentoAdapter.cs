using CaseBackend.Application.Domain.Entities;
using CaseBackend.Application.Query.Dtos;
using System.Collections.Generic;

namespace CaseBackend.Application.Query.Adapters
{
    internal static class InvestimentoAdapter
    {
        /// <summary>
        /// Retorna DTO
        /// </summary>
        /// <param name="entity">Investimento</param>
        /// <returns>Retorna DTO de Investimento</returns>
        internal static InvestimentoDTO ToDTO(this Investment entity)
        {
            if (entity == null) return null;

            return new InvestimentoDTO
            {
                Nome = entity.Nome,
                ValorInvestido = entity.InvestedAmount,
                ValorTotal = entity.TotalAmount,
                Vencimento = entity.DueDate,
                Ir = entity.IncomeTax,
                ValorResgate = entity.GetRedemptionValue()
            };
        }

        /// <summary>
        /// Converte lista de investimentos para DTO
        /// </summary>
        /// <param name="entities">Lista de investimentos</param>
        /// <returns>Lista de DTO</returns>
        internal static IEnumerable<InvestimentoDTO> ToDTO(this IEnumerable<Investment> entities)
        {
            if (entities == null) yield return null;

            foreach (var item in entities)
            {
                yield return item.ToDTO();
            }
        }
    }
}
