using CaseBackend.Application.Query.Dtos;
using System.Collections.Generic;

namespace CaseBackend.Application.Query.Responses
{
    public class GetInvestmentsResponse
    {
        public double ValorTotal { get; set; }

        public IEnumerable<InvestimentoDTO> Investimentos { get; set; }
    }
}
