using CaseBackend.Application.Query.Handlers;
using CaseBackend.Application.Query.Queries;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using RestSharp;
using System.Threading;
using System.Threading.Tasks;

namespace CaseBackend.Tests.Application.Query
{
    public class GetFundsInvestmentsHandlerTests
    {
        private GetFundsInvestmentsHandler _handler;

        [SetUp]
        public void Setup()
        {
            var logger = Substitute.For<ILogger<GetFundsInvestmentsHandler>>();
            var restClient = Substitute.For<IRestClient>();
            _handler = Substitute.For<GetFundsInvestmentsHandler>(logger, restClient);
        }

        [Test]
        public async Task Should_Return_Response_With_No_Exceptions()
        {
            var response = await this._handler.Handle(new GetFundsInvestmentsQuery(), CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsTrue(response.IsValid);
                Assert.IsEmpty(response.Messages);
            });
        }
    }
}
