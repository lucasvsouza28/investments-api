using CaseBackend.Application.Domain.Entities;
using CaseBackend.Application.Query.Queries;
using CaseBackend.Application.Query.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CaseBackend.Application.Domain.Settings;
using Microsoft.Extensions.Options;

namespace CaseBackend.Application.Query.Handlers
{
    public class GetFundsInvestmentsHandler : IRequestHandler<GetFundsInvestmentsQuery, Response<IEnumerable<Funds>>>
    {
        public GetFundsInvestmentsHandler(
            ILogger<GetFundsInvestmentsHandler> logger,
            IHttpClientFactory httpClientFactory,
            IOptions<EndpointsSettings> endpointOptions
            )
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _endpointSettings = endpointOptions.Value;
        }

        #region Fields

        private readonly ILogger<GetFundsInvestmentsHandler> _logger;
        private readonly HttpClient _httpClient;
        private readonly EndpointsSettings _endpointSettings;

        #endregion

        public async Task<Response<IEnumerable<Funds>>> Handle(GetFundsInvestmentsQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<Funds>>();

            try
            {


                _logger.LogInformation("Obtendo investimentos em Fundos de: {qty}", _endpointSettings.FundosInvestimentoUrl);

                using var restResponse = await _httpClient.GetAsync(_endpointSettings.FundosInvestimentoUrl, cancellationToken);

                if (restResponse.IsSuccessStatusCode)
                {
                    var fundos = await JsonSerializer.DeserializeAsync<FundsResponse>(await restResponse.Content.ReadAsStreamAsync(), cancellationToken: cancellationToken, options: _jsonOptions);
                    _logger.LogInformation("{qty} investimentos em Fundos retornados", fundos.Fundos.Count());
                    response.AddValue(fundos.Fundos);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro obter investimentos em Fundos");
            }

            return response;
        }

        private static JsonSerializerOptions _jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);


    }
}
