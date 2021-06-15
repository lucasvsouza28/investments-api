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
    public class GetDirectTreasureInvestmentsHandler : IRequestHandler<GetDirectTreasureInvestmentsQuery, Response<IEnumerable<DirectTreasure>>>
    {
        public GetDirectTreasureInvestmentsHandler(
            ILogger<GetDirectTreasureInvestmentsHandler> logger,
            IHttpClientFactory httpClientFactory,
            IOptions<EndpointsSettings> endpointsOptions
            )
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _endpointsSettings = endpointsOptions.Value;
        }

        #region Fields

        private readonly ILogger<GetDirectTreasureInvestmentsHandler> _logger;
        private readonly HttpClient _httpClient;
        private readonly EndpointsSettings _endpointsSettings;

        #endregion

        public async Task<Response<IEnumerable<DirectTreasure>>> Handle(GetDirectTreasureInvestmentsQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<DirectTreasure>>();

            try
            {
                _logger.LogInformation("Obtendo investimentos em tesouro direto de: {url}", _endpointsSettings.TesouroDiretoUrl);
                using var restResponse = await _httpClient.GetAsync(_endpointsSettings.TesouroDiretoUrl, cancellationToken);
                if (restResponse.IsSuccessStatusCode)
                {
                    
                    var dtr = await JsonSerializer.DeserializeAsync<DirectTreasureResponse>(await restResponse.Content.ReadAsStreamAsync(), cancellationToken: cancellationToken, options: _jsonOptions);
                    _logger.LogInformation("{qty} investimentos em tesouro direto retornados", dtr.Tds.Count());
                    response.AddValue(dtr.Tds);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro obter investimentos em tesouro direto");
            }

            return response;
        }

        private static JsonSerializerOptions _jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

    }
}
