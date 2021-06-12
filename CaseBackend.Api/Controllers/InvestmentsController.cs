using CaseBackend.Application.Query.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CaseBackend.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvestmentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<InvestmentsController> _logger;

        public InvestmentsController(
            IMediator mediator,
            ILogger<InvestmentsController> logger
            )
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
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
                string message = "There was an error to retrieve user investments";
                _logger.LogError(ex, message);

                return BadRequest(message);
            }
        }
    }
}
