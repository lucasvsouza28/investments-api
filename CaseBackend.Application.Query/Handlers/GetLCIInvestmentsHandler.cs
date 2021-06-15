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
    public class GetLCIInvestmentsHandler : IRequestHandler<GetLCIInvestmentsQuery, Response<IEnumerable<LCI>>>
    {
        public GetLCIInvestmentsHandler(
            ILogger<GetLCIInvestmentsHandler> logger,
            IHttpClientFactory httpClientFactory,
            IOptions<EndpointsSettings> endpointOptions
            )
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _endpointSettings = endpointOptions.Value;
        }

        #region Fields

        private readonly ILogger<GetLCIInvestmentsHandler> _logger;
        private readonly HttpClient _httpClient;
        private readonly EndpointsSettings _endpointSettings;

        #endregion

        public async Task<Response<IEnumerable<LCI>>> Handle(GetLCIInvestmentsQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<IEnumerable<LCI>>();

            try
            {


                _logger.LogInformation("Obtendo investimentos em LCI de: {url}", _endpointSettings.LciUrl);

                using var restResponse = await _httpClient.GetAsync(_endpointSettings.LciUrl, cancellationToken);

                if (restResponse.IsSuccessStatusCode)
                {

                    var resp = await JsonSerializer.DeserializeAsync<LCIResponse>(await restResponse.Content.ReadAsStreamAsync(), cancellationToken: cancellationToken, options: _jsonOptions);

                    _logger.LogInformation("{qty} investimentos em LCI retornados", resp.Lcis.Count());
                    response.AddValue(resp.Lcis);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro obter investimentos em LCI");
            }

            return response;
        }

        private static JsonSerializerOptions _jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

    }
}
