using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Common
{
	public interface IElasticAdapter
	{
		Task IndexDocumentAsync(Product item);
		Task<Product> GetDocumentAsync(string field, int productId);
		Task UpdateAsync(Product item);
	}
}
