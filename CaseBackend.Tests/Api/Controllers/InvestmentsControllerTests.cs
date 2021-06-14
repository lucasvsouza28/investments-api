using CaseBackend.Api.Controllers;
using CaseBackend.Application.Query.Queries;
using CaseBackend.Application.Query.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using Serilog;
using System.Threading.Tasks;

namespace CaseBackend.Tests.Api.Controllers
{
    public class InvestmentsControllerTests
    {
        private IMediator _mediator;
        private InvestmentsController _controller;

        [SetUp]
        public void Setup()
        {
            var logger = Substitute.For<ILogger>();
            _mediator = Substitute.For<IMediator>();
            _controller = new InvestmentsController(_mediator, logger);
        }

        [Test]
        public async Task Should_Return_Response_When_Get_Investments_Is_Valid()
        {
            var getRequest = new GetInvestmentsQuery();
            var getResponse = new GetInvestmentsResponse();

            _mediator.Send(getRequest)
                .ReturnsForAnyArgs(new Response<GetInvestmentsResponse>(getResponse));

            var response = await _controller.GetInvestments();

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsTrue(response is OkObjectResult);
            });
        }

        [Test]
        public async Task Should_Return_Response_When_Get_Investments_Is_Not_Valid()
        {
            var getRequest = new GetInvestmentsQuery();

            _mediator.Send(getRequest)
                .ReturnsForAnyArgs((a) =>
                {
                    var mediatorResponse = new Response<GetInvestmentsResponse>();
                    mediatorResponse.AddMessages("Simulando retorno com mensagem de erro");

                    return mediatorResponse;
                });

            var response = await _controller.GetInvestments();

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsTrue(response is BadRequestObjectResult);
            });
        }
    }
}
