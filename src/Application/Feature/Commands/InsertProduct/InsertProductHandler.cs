using Application.Common;
using Application.Common.Exceptions;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Feature.Commands.InsertProduct
{
	public class InsertProductHandler : IRequestHandler<InsertProductRequest, InsertProductResponse>
	{
		private readonly IApplicationDbContext databaseContext;
		private readonly IElasticAdapter elasticAdapter;

		public InsertProductHandler(IApplicationDbContext databaseContext, IElasticAdapter elasticAdapter)
		{
			this.databaseContext = databaseContext;
			this.elasticAdapter = elasticAdapter;
		}

		public async Task<InsertProductResponse> Handle(InsertProductRequest request, CancellationToken cancellationToken)
		{
			ValidateForeignKeys(request);
			var product = new Product()
			{
				Description = request.Description,
				TypeId = request.TypeId,
				Price = request.Price,
				StateId = request.StateId,
				Stock = request.Stock,
			};

			await this.databaseContext.Products.AddAsync(product);
			await this.databaseContext.SaveChangesAsync(cancellationToken);
			await this.elasticAdapter.IndexDocumentAsync(product);
			return new InsertProductResponse() { ProductId = product.ProductId };
		}

		private void ValidateForeignKeys(InsertProductRequest request)
		{
			if (!this.databaseContext.States.Where(x=>x.Id==request.StateId).Any())
			{
				throw new InvalidParameter("Invalid State");
			}

			if (!this.databaseContext.ProductTypes.Where(x => x.Id == request.TypeId).Any())
			{
				throw new InvalidParameter("Invalid Product Type");
			}
		}
	}
}
