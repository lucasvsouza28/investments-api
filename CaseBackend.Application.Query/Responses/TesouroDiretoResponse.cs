using CaseBackend.Application.Domain.Entities;
using System.Collections.Generic;

namespace CaseBackend.Application.Query.Responses
{
    public class TesouroDiretoResponse
    {
        public IEnumerable<TesouroDireto> Tds { get; set; }
    }
}
