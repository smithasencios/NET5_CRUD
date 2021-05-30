using Application.Common.Models;
using Infrastructure.Common.GlobalExceptionMiddleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Infrastructure
{
	public class ExceptionMiddlewareTests
	{
		private readonly Mock<ILogger<ExceptionMiddleware>> mockedLogger = new Mock<ILogger<ExceptionMiddleware>>();

		[Fact]
		public async Task InvokeAsync_ThrowsException_ReturnsCorrectMessage()
		{
			var expectedErrorMessage = "New Exception";

			// Arrange
			var middleware = new ExceptionMiddleware(
				innerHttpContext => throw new Exception(expectedErrorMessage),
				this.mockedLogger.Object);

			var context = new DefaultHttpContext();
			context.Response.Body = new MemoryStream();

			// Act
			await middleware.InvokeAsync(context).ConfigureAwait(false);
			context.Response.Body.Seek(0, SeekOrigin.Begin);
			using var reader = new StreamReader(context.Response.Body);
			var result = await reader.ReadToEndAsync().ConfigureAwait(false);
			var objResponse = JsonConvert.DeserializeObject<ErrorDetails>(result);

			// Assert
			Assert.Equal(expectedErrorMessage, objResponse.Message);
		}

		[Fact]
		public async Task InvokeAsync_WithEmptyStream_DoesntThrowException()
		{
			// Arrange
			var middleware = new ExceptionMiddleware(
				innerHttpContext => Task.CompletedTask,
				this.mockedLogger.Object);

			var context = new DefaultHttpContext();
			context.Response.Body = new MemoryStream();

			// Act
			await middleware.InvokeAsync(context).ConfigureAwait(false);
			context.Response.Body.Seek(0, SeekOrigin.Begin);
			using var reader = new StreamReader(context.Response.Body);

			// Assert
			Assert.Empty(await reader.ReadToEndAsync().ConfigureAwait(false));
		}
	}
}
