using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;

namespace Infrastructure.Common
{
	public static class ElasticsearchExtensions
	{
		public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
		{
			var defaultIndex = "products";

			var settings = new ConnectionSettings(new Uri(configuration["ElasticSearchServer"]))
						 .DefaultIndex(defaultIndex);
#if DEBUG
			settings.DisableDirectStreaming(); // Show request in response.DebugInformation
#endif
			var client = new ElasticClient(settings);

			services.AddSingleton<IElasticClient>(client);

			CreateIndex(client, defaultIndex);
		}

		private static void CreateIndex(IElasticClient client, string indexName)
		{
			client.Indices.Create(indexName);
		}
	}
}
