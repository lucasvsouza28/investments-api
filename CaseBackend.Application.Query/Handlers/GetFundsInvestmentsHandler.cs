using CaseBackend.Application.Domain.Entities;
using CaseBackend.Application.Query.Queries;
using CaseBackend.Application.Query.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseBackend.Application.Query.Handlers
{
    public class GetFundsInvestmentsHandler : IRequestHandler<GetFundsInvestmentsQuery, Response<IEnumerable<Funds>>>
    {
        public GetFundsInvestmentsHandler(
            ILogger<GetFundsInvestmentsHandler> logger,
            IRestClient restClient
            )
        {
            _logger = logger;
            _restClient = restClient;
        }

        #region Fields

        private readonly ILogger<GetFundsInvestmentsHandler> _logger;
        private readonly IRestClient _restClient; 

        #endregion

        public async Task<Response<IEnumerable<Funds>>> Handle(GetFundsInvestmentsQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<Funds>>();

            try
            {
                string url = "http://www.mocky.io/v2/5e342ab33000008c00d96342";

                _logger.LogInformation($"Obtendo investimentos em Fundos de: {url}");

                var restResponse = await _restClient.GetAsync<FundsResponse>(new RestRequest(url));

                if (restResponse != null)
                {
                    _logger.LogInformation($"{restResponse.Fundos.Count()} investimentos em Fundos retornados");

                    response.AddValue(restResponse.Fundos);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro obter investimentos em Fundos");
            }

            return response;
        }
    }
}
