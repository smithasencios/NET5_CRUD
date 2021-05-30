using Application.Common;
using Application.Common.Exceptions;
using Domain.Entities;
using MediatR;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Feature.Queries.GetProductById
{
	public class GetProductByIdHandler : IRequestHandler<GetProductByIdRequest, GetProductByIdResponse>
	{
		private readonly IApplicationDbContext databaseContext;
		private readonly IRedisAdapter redisAdapter;
		private readonly IElasticAdapter elasticAdapter;

		public GetProductByIdHandler(IApplicationDbContext databaseContext, IRedisAdapter redisAdapter, IElasticAdapter elasticAdapter)
		{
			this.databaseContext = databaseContext;
			this.redisAdapter = redisAdapter;
			this.elasticAdapter = elasticAdapter;
		}

		public async Task<GetProductByIdResponse> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
		{
			var product = (from p in this.databaseContext.Products
						   where p.ProductId == request.ProductId
						   select new
						   {
							   ProductId = p.ProductId,
							   p.Description,
							   p.StateId,
							   p.TypeId,
						   }).FirstOrDefault();

			if (product is null)
			{
				throw new ProductNotFound("Product Not Found");
			}

			var productTypes = await GetList<ProductType>(nameof(ProductType));
			var states = await GetList<State>(nameof(State));
			var details = await this.elasticAdapter.GetDocumentAsync("productId", request.ProductId);			

			return new GetProductByIdResponse()
			{
				ProductId = product.ProductId,
				Description = product.Description,
				State = states.FirstOrDefault(x => x.Id == product.StateId).Description,
				Type = productTypes.FirstOrDefault(x => x.Id == product.TypeId).Description,
				Stock = details.Stock,
				Price = details.Price,
			};
		}

		public async Task<IEnumerable<T>> GetList<T>(string cacheKey) where T : class
		{
			var list = await this.redisAdapter.GetAsync(cacheKey);
			if (list == null)
			{
				var productTypes = this.databaseContext.SetDbSet(typeof(T));
				await this.redisAdapter.SetAsync(cacheKey, JsonConvert.SerializeObject(productTypes.AsQueryable()));
				return (IQueryable<T>)productTypes;
			}

			return JsonConvert.DeserializeObject<IList<T>>(list).AsQueryable();
		}

	}
}
