using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Api
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();
			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;

				try
				{
					var context = services.GetRequiredService<ApplicationDbContext>();
					if (await context.Database.CanConnectAsync().ConfigureAwait(false))
					{
						context.Database.Migrate();
					}

					await ApplicationDbContextSeed.SeedDataAsync(context).ConfigureAwait(false);
				}
				catch (Exception)
				{
					throw;
				}
			}
			await host.RunAsync();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
			 .ConfigureLogging((configLogging) =>
			 {
				 configLogging.AddSerilog();
			 })
		     .UseSerilog((hostBuilderContext, configLogger) =>
			 {
				configLogger.WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u} {Message:lj}{NewLine}{Exception}");
			 })
			 .ConfigureWebHostDefaults(webBuilder =>
			 {
				webBuilder.UseStartup<Startup>();
			 });
	}
}
