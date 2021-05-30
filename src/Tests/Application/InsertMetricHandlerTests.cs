using Application.Common;
using Application.Feature.Commands.InsertMetrics;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Application
{
	public class InsertMetricHandlerTests
	{
		private readonly Mock<ILogger<InsertMetricHandler>> logger = new();

		[Fact]
		public async Task Handle_ValidRequest_DoesnotThrowException()
		{
			var expectedProduct = new InsertMetricRequest() { Method = "endpoint", RequestDate = DateTime.UtcNow, RequestDuration = 12 };
			var cancellationTokenSource = new CancellationTokenSource(3);

			// Arrange			
			var handler = new InsertMetricHandler(logger.Object);

			// Act				
			var response = await handler.Handle(expectedProduct, cancellationTokenSource.Token).ConfigureAwait(false);

			// Assert			
			Assert.NotNull(response);
		}
	}
}
