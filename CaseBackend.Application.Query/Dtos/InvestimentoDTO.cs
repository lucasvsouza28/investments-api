using System;

namespace CaseBackend.Application.Query.Dtos
{
    /// <summary>
    /// Representa um investimento
    /// </summary>
    public class InvestimentoDTO
    {
        /// <summary>
        /// Nome do investimento
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Valor investido
        /// </summary>
        public double ValorInvestido { get; set; }

        /// <summary>
        /// Valor total
        /// </summary>
        public double ValorTotal { get; set; }

        /// <summary>
        /// Data do vencimento
        /// </summary>
        public DateTime Vencimento { get; set; }

        /// <summary>
        /// Valor total de IR sobre a rentabilidade
        /// </summary>
        public double Ir { get; set; }

        /// <summary>
        /// Valor total de resgate
        /// </summary>
        public double ValorResgate { get; set; }
    }
}
