using Application.Common;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
	public class RedisAdapter : IRedisAdapter
	{
		private readonly IDistributedCache distributedCache;

		public RedisAdapter(IDistributedCache distributedCache)
		{
			this.distributedCache = distributedCache;
		}

		public async Task<string> GetAsync(string cacheKey)
		{
			var list = await distributedCache.GetAsync(cacheKey);
			if (list == null)
			{
				return null;
			}

			return Encoding.UTF8.GetString(list);
		}

		public async Task SetAsync(string cacheKey, string serializedList)
		{
			var redisList = Encoding.UTF8.GetBytes(serializedList);
			var options = new DistributedCacheEntryOptions()
								.SetAbsoluteExpiration(DateTime.Now.AddMinutes(10));
			await distributedCache.SetAsync(cacheKey, redisList, options);
		}
	}
}
