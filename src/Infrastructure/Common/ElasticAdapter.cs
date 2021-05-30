using Application.Common;
using Domain.Entities;
using Nest;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
	public class ElasticAdapter : IElasticAdapter
	{
		private readonly IElasticClient elasticClient;

		public ElasticAdapter(IElasticClient elasticClient)
		{
			this.elasticClient = elasticClient;
		}

		public async Task<Product> GetDocumentAsync(string field, int productId)
		{
			var record = await this.elasticClient.SearchAsync<Product>(
				s => s.Query(q => q.Match(d => d.Field(field).Query(productId.ToString())))
				);

			return record.Documents.First();
		}

		public async Task IndexDocumentAsync(Product item)
		{
			await this.elasticClient.IndexDocumentAsync(item);
		}

		public async Task UpdateAsync(Product item)
		{
			await this.elasticClient.UpdateAsync<Product>(item, u => u.Doc(item));
		}
	}
}
