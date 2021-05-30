using Application.Common;
using Application.Common.Exceptions;
using Application.Feature.Queries.GetProductById;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tests.Application.Helpers;
using Xunit;

namespace Tests.Application
{
	public class GetProductByIdHandlerTests
	{
		private readonly Mock<IApplicationDbContext> applicationDbContext = new();
		private readonly Mock<IRedisAdapter> redisAdapter = new();
		private readonly Mock<IElasticAdapter> elasticAdapter = new();

		[Fact]
		public async Task Handle_ProductNotFound_ThrowsProductNotFoundException()
		{
			// Arrange
			var products = new List<Product>();
			this.applicationDbContext.SetupGet(x => x.Products).Returns(TestFunctions.GetDbSet(products.AsQueryable()).Object);
			var handler = new GetProductByIdHandler(applicationDbContext.Object, redisAdapter.Object, elasticAdapter.Object);

			// Act			
			// Assert
			var cancellationTokenSource = new CancellationTokenSource(3);
			await Assert.ThrowsAsync<ProductNotFound>(async () => await handler.Handle(new GetProductByIdRequest(), cancellationTokenSource.Token).ConfigureAwait(false)).ConfigureAwait(false);
		}

		[Fact]
		public async Task Handle_ProductFound_ReturnsData()
		{
			// Arrange
			var expectedProduct = new Product() { ProductId = 1, Description = "description", Price = 10, Stock = 12, TypeId = 1, StateId = 1 };
			var products = new List<Product>() { expectedProduct, };
			var types = new List<ProductType>
			{
				new ProductType { Description = "description", Id = 1 },
			};
			var states = new List<State>
			{
				new State { Description = "description", Id = 1 },
			};

			this.applicationDbContext.SetupGet(x => x.Products).Returns(TestFunctions.GetDbSet(products.AsQueryable()).Object);
			this.applicationDbContext.SetupGet(x => x.ProductTypes).Returns(TestFunctions.GetDbSet(types.AsQueryable()).Object);
			this.applicationDbContext.SetupGet(x => x.States).Returns(TestFunctions.GetDbSet(states.AsQueryable()).Object);
			this.applicationDbContext.Setup(x => x.SetDbSet(typeof(ProductType))).Returns(types.AsQueryable());
			this.applicationDbContext.Setup(x => x.SetDbSet(typeof(State))).Returns(states.AsQueryable());
			this.elasticAdapter.Setup(x => x.GetDocumentAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(expectedProduct);
			this.redisAdapter.Setup(x => x.GetAsync(nameof(ProductType))).ReturnsAsync("[{'Id':2,'Description':'Inorganico'},{'Id':1,'Description':'Organico'}]");
			var handler = new GetProductByIdHandler(applicationDbContext.Object, redisAdapter.Object, elasticAdapter.Object);

			// Act		
			var cancellationTokenSource = new CancellationTokenSource(3);
			var response = await handler.Handle(new GetProductByIdRequest() { ProductId = 1 }, cancellationTokenSource.Token);

			// Assert
			Assert.NotNull(response);
			Assert.Equal(expectedProduct.Description, response.Description);
			Assert.NotNull(response.State);
			Assert.NotNull(response.ProductId);
			Assert.NotNull(response.Price);
			Assert.NotNull(response.Type);
			Assert.NotNull(response.Stock);
		}
	}
}
