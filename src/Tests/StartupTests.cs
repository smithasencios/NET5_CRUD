using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
	public class StartupTests
	{
        public StartupTests()
        {
            Environment.SetEnvironmentVariable("NpgSqlConnection", "connection");
            Environment.SetEnvironmentVariable("NpgSqlPassword", "INDEXNAME");
            Environment.SetEnvironmentVariable("RedisServer", "demo:6379");
            Environment.SetEnvironmentVariable("ElasticSearchServer", "http://demotekton:9201");
        }

        [Fact]
        public async Task InvokeHealthCheckEndPoint_ReturnsHealthyStatus()
        {
            var webHostBuilder = new WebHostBuilder()
                                        .UseEnvironment("Development")
                                        .UseStartup<FakeStartup>();

            using var server = new TestServer(webHostBuilder);
            using var client = server.CreateClient();
            var result = await client.GetAsync(new Uri("health", UriKind.Relative)).ConfigureAwait(false);
            var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
