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
    public class GetDirectTreasureInvestmentsHandler : IRequestHandler<GetDirectTreasureInvestmentsQuery, Response<IEnumerable<DirectTreasure>>>
    {
        public GetDirectTreasureInvestmentsHandler(
            ILogger<GetDirectTreasureInvestmentsHandler> logger,
            IRestClient restClient
            )
        {
            _logger = logger;
            _restClient = restClient;
        }

        #region Fields

        private readonly ILogger<GetDirectTreasureInvestmentsHandler> _logger;
        private readonly IRestClient _restClient; 

        #endregion

        public async Task<Response<IEnumerable<DirectTreasure>>> Handle(GetDirectTreasureInvestmentsQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<DirectTreasure>>();

            try
            {
                string url = "http://www.mocky.io/v2/5e3428203000006b00d9632a";

                _logger.LogInformation($"Obtendo investimentos em tesouro direto de: {url}");

                var restResponse = await _restClient.GetAsync<DirectTreasureResponse>(new RestRequest(url));

                if (restResponse != null)
                {
                    _logger.LogInformation($"{restResponse.Tds.Count()} investimentos em tesouro direto retornados");
                    response.AddValue(restResponse.Tds);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro obter investimentos em tesouro direto");
            }

            return response;
        }
    }
}
