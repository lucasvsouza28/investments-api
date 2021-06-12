using CaseBackend.Application.Domain.Entities;
using System.Collections.Generic;

namespace CaseBackend.Application.Query.Responses
{
    public class DirectTreasureResponse
    {
        public IEnumerable<DirectTreasure> Tds { get; set; }
    }
}
