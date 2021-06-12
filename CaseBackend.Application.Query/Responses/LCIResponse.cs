using CaseBackend.Application.Domain.Entities;
using System.Collections.Generic;

namespace CaseBackend.Application.Query.Responses
{
    public class LCIResponse
    {
        public IEnumerable<LCI> Lcis { get; set; }
    }
}
