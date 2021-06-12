using CaseBackend.Application.Domain.Entities;
using CaseBackend.Application.Query.Responses;
using MediatR;
using System.Collections.Generic;

namespace CaseBackend.Application.Query.Queries
{
    public class GetFundsInvestmentsQuery : IRequest<Response<IEnumerable<Funds>>>
    {
    }
}
