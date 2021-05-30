using Application.Common;
using Infrastructure.Common;
using Infrastructure.Common.Settings;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<ApplicationSettings>(configuration);
			services.AddTransient<IStartupFilter, SettingValidationStartupFilter>();
			services.AddSingleton(_ => _.GetRequiredService<IOptions<ApplicationSettings>>().Value);
			services.AddSingleton<IValidatable>(_ => _.GetRequiredService<IOptions<ApplicationSettings>>().Value);			

			services.AddDbContext<ApplicationDbContext>((s, options) =>
			{
				var applicationSettings = s.GetRequiredService<IOptions<ApplicationSettings>>().Value;
				var connectionString = $"{applicationSettings.NpgSqlConnection}Password ={applicationSettings.NpgSqlPassword};";
				options.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
			});
			services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
			services.AddStackExchangeRedisCache(options =>
			{
				options.Configuration = configuration["RedisServer"];
			});
			services.AddScoped<IRedisAdapter, RedisAdapter>();
			services.AddElasticsearch(configuration);
			services.AddScoped<IElasticAdapter, ElasticAdapter>();
			return services;
		}
	}
}
