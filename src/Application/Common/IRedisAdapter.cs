using System.Threading.Tasks;

namespace Application.Common
{
	public interface IRedisAdapter
	{
		Task SetAsync(string cacheKey, string items);

		Task<string> GetAsync(string cacheKey);
	}
}
