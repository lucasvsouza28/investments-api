using CaseBackend.Application.Domain.Entities;
using CaseBackend.Application.Query.Adapters;
using CaseBackend.Application.Query.Dtos;
using CaseBackend.Application.Query.Queries;
using CaseBackend.Application.Query.Responses;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CaseBackend.Application.Query.Handlers
{
    public class GetInvestmentsHandler : IRequestHandler<GetInvestmentsQuery, Response<GetInvestmentsResponse>>
    {
        public GetInvestmentsHandler(
            ILogger<GetInvestmentsHandler> logger,
            IMediator mediator,
            IDistributedCache cache
            )
        {
            _logger = logger;
            _mediator = mediator;
            _cache = cache;
        }

        #region Fields

        private readonly ILogger<GetInvestmentsHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IDistributedCache _cache;

        #endregion

        #region Constants

        const string cacheKey = "Investments";

        #endregion

        public async Task<Response<GetInvestmentsResponse>> Handle(GetInvestmentsQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<GetInvestmentsResponse>();

            try
            {
                // Verifica se resultado já está no cache, se estiver, retorna

                var fromCache = await _cache.GetAsync(cacheKey, cancellationToken);

                if (fromCache?.Any() == true)
                {

                    var cachedResponse = JsonSerializer
                        .Deserialize<Response<GetInvestmentsResponse>>(fromCache, _jsonOptions);

                    _logger.LogInformation("{qty} registros foram encontrados no cache",
                        cachedResponse.Value.Investimentos.Count());

                    return cachedResponse;

                }

                // Obtem investimentos de todos as fontes (Fundos, LCI e Tesouro Direto)
                var investiments = await GetAllInvestments();

                // Consolida as informações
                IEnumerable<InvestimentoDTO> investimentoDTOs = investiments.ToDTO();

                response.AddValue(new GetInvestmentsResponse
                {
                    ValorTotal = investimentoDTOs.Sum(i => i.ValorTotal),
                    Investimentos = investimentoDTOs
                });

                // Insere resultado no cache
                await _cache.SetAsync(cacheKey, JsonSerializer.SerializeToUtf8Bytes(response, _jsonOptions), _cacheOptions, cancellationToken);

            }
            catch (Exception ex)
            {
                string errorMessage = "Ocorreu um erro ao buscar os investimentos";
                _logger.LogError(ex, errorMessage);

                response.AddMessages(errorMessage);
            }

            return response;
        }

        /// <summary>
        /// Returns all investments
        /// </summary>
        /// <returns>List of investments</returns>
        private async Task<IEnumerable<Investment>> GetAllInvestments()
        {
            var directTreasureResponse = await _mediator.Send(new GetDirectTreasureInvestmentsQuery());
            var lciResponse = await _mediator.Send(new GetLCIInvestmentsQuery());
            var fundsResponse = await _mediator.Send(new GetFundsInvestmentsQuery());

            var investments = new List<Investment>();

            if (directTreasureResponse?.Value != null) investments.AddRange(directTreasureResponse.Value.ToInvestment());

            if (lciResponse?.Value != null) investments.AddRange(lciResponse.Value.ToInvestment());

            if (fundsResponse?.Value != null) investments.AddRange(fundsResponse.Value.ToInvestment());

            _logger.LogInformation("{qty} investimentos foram retornados no total", investments.Count);

            return investments;
        }
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

        private static readonly DistributedCacheEntryOptions _cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Today.AddDays(1).Date
        };

    }
}
