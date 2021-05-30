using Application.Common;
using Domain.Entities;
using Infrastructure.Common;
using Moq;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Infrastructure
{
	public class ElasticAdapterTests
	{
		private readonly Mock<IElasticClient> elasticClient = new();

		[Fact]
		public async Task GetDocumentAsync_ValidDocument_DoesnotThrowException()
		{
			// Arrange
			var products = new List<Product>
			{
				new Product{ProductId=1, Description="descripcion"},
			};
			var searchResponse = new Mock<ISearchResponse<Product>>();
			searchResponse.SetupGet(x => x.Documents).Returns(products);
			this.elasticClient.Setup(x => x.SearchAsync(It.IsAny<Func<SearchDescriptor<Product>, ISearchRequest>>(), It.IsAny<CancellationToken>()))
							  .ReturnsAsync(searchResponse.Object);

			// Act			
			var elasticAdapter = new ElasticAdapter(this.elasticClient.Object);
			var response = await elasticAdapter.GetDocumentAsync("productId", 1);

			// Assert
			Assert.Equal(products.First().ProductId, response.ProductId);
		}

		[Fact]
		public async Task InsertDocumentAsync_ValidDocument_DoesnotThrowException()
		{
			// Arrange
			var product = new Product { ProductId = 1, Description = "descripcion" };

			this.elasticClient.Setup(x => x.IndexDocumentAsync(product, It.IsAny<CancellationToken>()));

			// Act			
			var elasticAdapter = new ElasticAdapter(this.elasticClient.Object);
			await elasticAdapter.IndexDocumentAsync(product);

			// Assert
			this.elasticClient.Verify();
		}

		[Fact]
		public async Task UpdateDocumentAsync_ValidDocument_DoesnotThrowException()
		{
			// Arrange
			var product = new Product { ProductId = 1, Description = "descripcion" };

			this.elasticClient.Setup(x => x.UpdateAsync<Product>(product, It.IsAny<Func<UpdateDescriptor<Product, Product>, IUpdateRequest<Product, Product>>>(), It.IsAny<CancellationToken>()));

			// Act			
			var elasticAdapter = new ElasticAdapter(this.elasticClient.Object);
			await elasticAdapter.UpdateAsync(product);

			// Assert
			this.elasticClient.Verify();
		}
	}
}
