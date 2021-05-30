using Infrastructure.Common.Middlewares;
using MediatR;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Infrastructure
{
	public class MetricsMiddlewareTests
	{
        /// <summary>
        /// Tests InvokeAsync methods with null HttpContext.
        /// </summary>
        [Fact]
        public async void InvokeAsync_WithNullHttpContext_ThrowsException()
        {
            var mockedMediator = new Mock<IMediator>();
            var metricsMiddleware = new MetricsMiddleware(innerHttpContext => Task.FromResult(0), mockedMediator.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await metricsMiddleware.InvokeAsync(null).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
    }
}
