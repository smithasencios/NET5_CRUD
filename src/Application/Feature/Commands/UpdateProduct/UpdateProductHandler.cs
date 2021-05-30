using Application.Common;
using Application.Common.Exceptions;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Feature.Commands.UpdateProduct
{
	public class UpdateProductHandler : IRequestHandler<UpdateProductRequest, UpdateProductResponse>
	{
		private readonly IApplicationDbContext databaseContext;
		private readonly IElasticAdapter elasticAdapter;

		public UpdateProductHandler(IElasticAdapter elasticAdapter, IApplicationDbContext databaseContext)
		{
			this.elasticAdapter = elasticAdapter;
			this.databaseContext = databaseContext;
		}

		public async Task<UpdateProductResponse> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
		{
			var product = this.databaseContext.Products.FirstOrDefault(x => x.ProductId == request.Id);
			if (product is null)
			{
				throw new ProductNotFound("Product Not Found");
			}

			product.Description = request.Description;
			product.Price = request.Price;			
			product.Stock = request.Stock;

			await this.databaseContext.SaveChangesAsync(cancellationToken);
			await this.elasticAdapter.UpdateAsync(product);
			return new UpdateProductResponse() { ProductId = product.ProductId };
		}
	}
}
