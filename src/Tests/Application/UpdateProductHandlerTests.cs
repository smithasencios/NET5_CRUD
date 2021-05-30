using Application.Common;
using Application.Common.Exceptions;
using Application.Feature.Commands.UpdateProduct;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tests.Application.Helpers;
using Xunit;

namespace Tests.Application
{
	public class UpdateProductHandlerTests
	{
		private readonly Mock<IElasticAdapter> elasticAdapter = new();
		private readonly Mock<IApplicationDbContext> applicationDbContext = new();

		[Fact]
		public async Task Handle_ProductNotFound_ThrowsProductNotFoundException()
		{
			// Arrange
			var handler = new UpdateProductHandler(elasticAdapter.Object, applicationDbContext.Object);
			var expectedProduct = new Product() { ProductId = 1, Description = "description", Price = 10, Stock = 12, TypeId = 1, StateId = 1 };
			var products = new List<Product> { expectedProduct };

			this.applicationDbContext.SetupGet(x => x.Products).Returns(TestFunctions.GetDbSet(products.AsQueryable()).Object);

			// Act			
			// Assert
			var cancellationTokenSource = new CancellationTokenSource(3);
			await Assert.ThrowsAsync<ProductNotFound>(async () => await handler.Handle(new UpdateProductRequest() { Id = 12 }, cancellationTokenSource.Token).ConfigureAwait(false)).ConfigureAwait(false);
		}

		[Fact]
		public async Task Handle_ValidRequest_DoesnotThrowException()
		{
			var expectedProduct = new UpdateProductRequest() { Id = 1, Description = "description", Price = 10, Stock = 12 };
			var cancellationTokenSource = new CancellationTokenSource(3);

			// Arrange			
			string dbName = Guid.NewGuid().ToString();
			DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
							.UseInMemoryDatabase(databaseName: dbName).Options;
			var databaseContext = new ApplicationDbContext(options);
			await databaseContext.Products.AddAsync(new Product { Description = "description", ProductId = 1 });
			await databaseContext.SaveChangesAsync(cancellationTokenSource.Token);
			var handler = new UpdateProductHandler(elasticAdapter.Object, databaseContext);

			// Act				
			var response = await handler.Handle(expectedProduct, cancellationTokenSource.Token).ConfigureAwait(false);

			// Assert			
			Assert.NotNull(response);
			Assert.Equal(expectedProduct.Id.ToString(), response.ProductId.ToString());
		}
	}
}
