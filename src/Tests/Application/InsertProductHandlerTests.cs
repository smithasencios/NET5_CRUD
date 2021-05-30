using Application.Common;
using Application.Common.Exceptions;
using Application.Feature.Commands.InsertProduct;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
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
	public class InsertProductHandlerTests
	{
		private readonly Mock<IApplicationDbContext> applicationDbContext = new();
		private readonly Mock<IElasticAdapter> elasticAdapter = new();

		[Fact]
		public async Task Handle_InvalidState_ThrowsInvalidParameterException()
		{
			// Arrange
			var handler = new InsertProductHandler(applicationDbContext.Object, elasticAdapter.Object);
			var types = new List<ProductType>
			{
				new ProductType { Description = "description", Id = 1 },
			};
			var states = new List<State>
			{
				new State { Description = "description", Id = 1 },
			};

			this.applicationDbContext.SetupGet(x => x.ProductTypes).Returns(TestFunctions.GetDbSet(types.AsQueryable()).Object);
			this.applicationDbContext.SetupGet(x => x.States).Returns(TestFunctions.GetDbSet(states.AsQueryable()).Object);

			// Act			
			// Assert
			var cancellationTokenSource = new CancellationTokenSource(3);
			await Assert.ThrowsAsync<InvalidParameter>(async () => await handler.Handle(new InsertProductRequest() { StateId = 2 }, cancellationTokenSource.Token).ConfigureAwait(false)).ConfigureAwait(false);
			await Assert.ThrowsAsync<InvalidParameter>(async () => await handler.Handle(new InsertProductRequest() { StateId = 1, TypeId = 3 }, cancellationTokenSource.Token).ConfigureAwait(false)).ConfigureAwait(false);
		}

		[Fact]
		public async Task Handle_ValidRequest_DoesnotThrowException()
		{
			var expectedProduct = new InsertProductRequest() { Description = "description", Price = 10, Stock = 12, TypeId = 1, StateId = 1 };
			var cancellationTokenSource = new CancellationTokenSource(3);

			// Arrange			
			string dbName = Guid.NewGuid().ToString();
			DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
							.UseInMemoryDatabase(databaseName: dbName).Options;
			var databaseContext = new ApplicationDbContext(options);
			await databaseContext.States.AddAsync(new State { Description = "description", Id = 1 });
			await databaseContext.ProductTypes.AddAsync(new ProductType { Description = "description", Id = 1 });
			await databaseContext.SaveChangesAsync(cancellationTokenSource.Token);
			var handler = new InsertProductHandler(databaseContext, elasticAdapter.Object);

			// Act				
			var response = await handler.Handle(expectedProduct, cancellationTokenSource.Token).ConfigureAwait(false);

			// Assert			
			Assert.NotNull(response);
			Assert.NotNull(response.ProductId);
		}
	}
}
