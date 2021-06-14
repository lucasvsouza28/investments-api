using CaseBackend.Application.Query.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace CaseBackend.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvestmentsController : Controller
    {
        public InvestmentsController(
            IMediator mediator,
            ILogger logger
            )
        {
            _mediator = mediator;
            _logger = logger;
        }

        #region Fields

        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        #endregion

        [HttpGet]
        [Route("/investments")]
        public async Task<IActionResult> GetInvestments()
        {
            try
            {
                var response = await _mediator.Send(new GetInvestmentsQuery());

                if (!response.IsValid)
                    return BadRequest(response);

                return Ok(response.Value);
            }
            catch (Exception ex)
            {
                string message = "Ocorreu um erro ao buscar investimentos";
                _logger.Error(ex, message);

                return BadRequest(message);
            }
        }
    }
}
