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
    public class GetLCIInvestmentsHandler : IRequestHandler<GetLCIInvestmentsQuery, Response<IEnumerable<LCI>>>
    {
        public GetLCIInvestmentsHandler(
            ILogger<GetLCIInvestmentsHandler> logger,
            IRestClient restClient
            )
        {
            _logger = logger;
            _restClient = restClient;
        }

        #region Fields

        private readonly ILogger<GetLCIInvestmentsHandler> _logger;
        private readonly IRestClient _restClient; 

        #endregion

        public async Task<Response<IEnumerable<LCI>>> Handle(GetLCIInvestmentsQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<LCI>>();

            try
            {
                string url = "http://www.mocky.io/v2/5e3429a33000008c00d96336";

                _logger.LogInformation($"Obtendo investimentos em LCI de: {url}");

                var restResponse = await _restClient.GetAsync<LCIResponse>(new RestRequest(url));

                if (restResponse != null)
                {
                    _logger.LogInformation($"{restResponse.Lcis.Count()} investimentos em LCI retornados");
                    response.AddValue(restResponse.Lcis);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro obter investimentos em LCI");
            }

            return response;
        }
    }
}
