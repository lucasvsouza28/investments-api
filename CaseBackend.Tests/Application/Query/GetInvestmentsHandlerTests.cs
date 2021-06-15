using CaseBackend.Application.Query.Handlers;
using CaseBackend.Application.Query.Queries;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace CaseBackend.Tests.Application.Query
{
    public class GetInvestmentsHandlerTests
    {
        private GetInvestmentsHandler _handler;

        [SetUp]
        public void Setup()
        {
            var logger = Substitute.For<ILogger<GetInvestmentsHandler>>();
            var mediator = Substitute.For<IMediator>();
            var cache = Substitute.For<IDistributedCache>();
            _handler = Substitute.For<GetInvestmentsHandler>(logger, mediator, cache);
        }

        [Test]
        public async Task Should_Return_Response_With_No_Exceptions()
        {
            var response = await _handler.Handle(new GetInvestmentsQuery(), CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsTrue(response.IsValid);
                Assert.IsEmpty(response.Messages);
            });
        }
    }
}
