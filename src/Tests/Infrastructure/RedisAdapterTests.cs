using Infrastructure.Common;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Infrastructure
{
	public class RedisAdapterTests
	{
		private readonly Mock<IDistributedCache> cache = new();

		[Fact]
		public async Task GetAsync_ValidDocument_DoesnotThrowException()
		{
			var expectedResult = "expectedResult";
			// Arrange
			this.cache.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
					.ReturnsAsync(Encoding.UTF8.GetBytes(expectedResult));

			// Act			
			var adapter = new RedisAdapter(this.cache.Object);
			var response = await adapter.GetAsync("string");
			// Assert
			Assert.Equal(expectedResult, response);
		}

		[Fact]
		public async Task SetAsync_ValidDocument_DoesnotThrowException()
		{
			var expectedResult = "expectedResult";
			// Arrange
			this.cache.Setup(x => x.SetAsync(It.IsAny<string>(), Encoding.UTF8.GetBytes(expectedResult), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()));

			// Act					
			var adapter = new RedisAdapter(this.cache.Object);
			await adapter.SetAsync("string", expectedResult);

			// Assert	
			this.cache.Verify();
		}
	}
}
