using CaseBackend.Application.Query.Responses;
using MediatR;

namespace CaseBackend.Application.Query.Queries
{
    public class GetInvestmentsQuery : IRequest<Response<GetInvestmentsResponse>>
    {
    }
}
