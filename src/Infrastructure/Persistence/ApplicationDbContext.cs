using Application.Common;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
	/// dotnet ef migrations add InitialMigration -s Api -p Infrastructure --context ApplicationDbContext
	public class ApplicationDbContext : DbContext, IApplicationDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		   : base(options)
		{
		}

		public DbSet<Product> Products { get; set; }
		public DbSet<State> States { get; set; }
		public DbSet<ProductType> ProductTypes { get; set; }

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			foreach (var entry in this.ChangeTracker.Entries<AuditableEntity>())
			{
				switch (entry.State)
				{
					case EntityState.Added:
						entry.Entity.CreatedBy = string.Empty;
						entry.Entity.CreatedTime = DateTime.UtcNow;
						break;
					case EntityState.Modified:
						entry.Entity.LastModifiedBy = string.Empty;
						entry.Entity.LastModifiedTime = DateTime.UtcNow;
						break;
				}
			}

			return base.SaveChangesAsync(cancellationToken);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			if (modelBuilder is null)
			{
				throw new ArgumentNullException(nameof(modelBuilder));
			}
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			base.OnModelCreating(modelBuilder);
		}

		public IQueryable<object> SetDbSet(Type t)
		{
			return (IQueryable<object>)this.GetType().GetMethod(nameof(DbContext.Set), Type.EmptyTypes).MakeGenericMethod(t).Invoke(this, null);
		}
	}
}

