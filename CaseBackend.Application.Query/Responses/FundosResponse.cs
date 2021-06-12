using CaseBackend.Application.Domain.Entities;
using System.Collections.Generic;

namespace CaseBackend.Application.Query.Responses
{
    public class FundosResponse
    {
        public IEnumerable<Fundos> Fundos { get; set; }
    }
}
