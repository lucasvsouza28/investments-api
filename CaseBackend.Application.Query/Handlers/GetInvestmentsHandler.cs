using CaseBackend.Application.Domain.Entities;
using CaseBackend.Application.Query.Adapters;
using CaseBackend.Application.Query.Dtos;
using CaseBackend.Application.Query.Queries;
using CaseBackend.Application.Query.Responses;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseBackend.Application.Query.Handlers
{
    public class GetInvestmentsHandler : IRequestHandler<GetInvestmentsQuery, Response<GetInvestmentsResponse>>
    {
        public GetInvestmentsHandler(
            ILogger<GetInvestmentsHandler> logger,
            IMediator mediator,
            IMemoryCache cache
            )
        {
            _logger = logger;
            _mediator = mediator;
            _cache = cache;
        }

        #region Fields

        private readonly ILogger<GetInvestmentsHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache;

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
                if (_cache.TryGetValue(cacheKey, out Response<GetInvestmentsResponse> cachedResponse))
                {
                    return cachedResponse;
                }

                // Obtem investimentos de todos as fontes (Fundos, LCI e Tesouro Direto)
                var investiments = await this.GetAllInvestments();

                // Consolida as informações
                IEnumerable<InvestimentoDTO> investimentoDTOs = investiments.ToDTO();

                response.AddValue(new GetInvestmentsResponse
                {
                    ValorTotal = investimentoDTOs.Sum(i => i.ValorTotal),
                    Investimentos = investimentoDTOs
                });

                // Insere resultado no cache
                _cache.Set(cacheKey, response, DateTime.Now.AddDays(1).Date);
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

            _logger.LogInformation($"{investments.Count} foram retornados no total");

            return investments;
        }
    }
}
