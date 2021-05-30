using Api.Controllers;
using Application.Feature.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Moq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Controllers
{
	public class ProductTests
	{
		[Fact]
		public async Task InsertProduct_NullDescription_ReturnsBadRequest()
		{
			using var server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
			var content = new StringContent("{\"stock\":\"23\"}", Encoding.UTF8, MediaTypeNames.Application.Json);
			var httpClient = server.CreateClient();
			var result = await httpClient.PostAsync($"product", content).ConfigureAwait(false);
			Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
		}

		[Fact]
		public async Task UpdateProduct_NullDescription_ReturnsBadRequest()
		{
			using var server = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
			var content = new StringContent("{\"stock\":\"23\"}", Encoding.UTF8, MediaTypeNames.Application.Json);
			var httpClient = server.CreateClient();
			var result = await httpClient.PutAsync($"product/1", content).ConfigureAwait(false);
			Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
		}

		[Fact]
		public async Task GetDocument_WithResult_StatusOk()
		{
			// Arrange
			var mediator = new Mock<IMediator>();
			mediator.Setup(x => x.Send(It.IsAny<GetProductByIdRequest>(), It.IsAny<CancellationToken>()))
					.ReturnsAsync(new GetProductByIdResponse());

			using var controller = new ProductController(mediator.Object);

			// Act
			var response = await controller.Get(1).ConfigureAwait(false);

			// Assert
			Assert.Equal((int)HttpStatusCode.OK, ((OkObjectResult)response).StatusCode);
			mediator.Verify();
		}
	}
}
