using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common
{
	public interface IApplicationDbContext
	{
		DbSet<Product> Products { get; set; }
		DbSet<State> States { get; set; }
		DbSet<ProductType> ProductTypes { get; set; }
		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
		IQueryable<object> SetDbSet(Type t);
	}
}
