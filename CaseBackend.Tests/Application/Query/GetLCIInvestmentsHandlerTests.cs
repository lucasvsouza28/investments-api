using System.Net.Http;
using CaseBackend.Application.Query.Handlers;
using CaseBackend.Application.Query.Queries;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using CaseBackend.Application.Domain.Settings;
using Microsoft.Extensions.Options;

namespace CaseBackend.Tests.Application.Query
{
    public class GetLCIInvestmentsHandlerTests
    {
        private GetLCIInvestmentsHandler _handler;

        [SetUp]
        public void Setup()
        {
            var logger = Substitute.For<ILogger<GetLCIInvestmentsHandler>>();
            var restClient = Substitute.For<IHttpClientFactory>();
            var options = Substitute.For<IOptions<EndpointsSettings>>();

            _handler = Substitute.For<GetLCIInvestmentsHandler>(logger, restClient, options);
        }

        [Test]
        public async Task Should_Return_Response_With_No_Exceptions()
        {
            var response = await _handler.Handle(new GetLCIInvestmentsQuery(), CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsTrue(response.IsValid);
                Assert.IsEmpty(response.Messages);
            });
        }
    }
}
