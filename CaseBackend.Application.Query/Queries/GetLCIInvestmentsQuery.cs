using CaseBackend.Application.Domain.Entities;
using CaseBackend.Application.Query.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseBackend.Application.Query.Queries
{
    public class GetLCIInvestmentsQuery : IRequest<Response<IEnumerable<LCI>>>
    {
    }
}
