using CaseBackend.Application.Domain.Entities;
using System.Collections.Generic;

namespace CaseBackend.Application.Query.Responses
{
    public class FundsResponse
    {
        public IEnumerable<Funds> Fundos { get; set; }
    }
}
