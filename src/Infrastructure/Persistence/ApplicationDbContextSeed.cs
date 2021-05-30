using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
	public static class ApplicationDbContextSeed
    {
        /// <summary>
        /// Seed data.
        /// </summary>
        /// <param name="context">Database context.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public static async Task SeedDataAsync(ApplicationDbContext context)
        {
            if (context is null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            if (!context.States.Any())
            {
                context.States.Add(new Domain.Entities.State() { Id = 1,Description="Activo" });
                context.States.Add(new Domain.Entities.State() { Id = 2, Description = "Inactivo" });
                await context.SaveChangesAsync().ConfigureAwait(false);
            }

            if (!context.ProductTypes.Any())
            {
                context.ProductTypes.Add(new Domain.Entities.ProductType() { Id = 1, Description = "Organico" });
                context.ProductTypes.Add(new Domain.Entities.ProductType() { Id = 2, Description = "Inorganico" });
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
