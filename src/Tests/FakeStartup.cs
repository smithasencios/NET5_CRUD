using Api;
using Application;
using Microsoft.Extensions.DependencyInjection;

namespace Tests
{
	public class FakeStartup : Startup
	{
		public FakeStartup() : base()
		{

		}

		public override void ConfigureServices(IServiceCollection services)
		{
			services.AddRouting();
			services.AddMvc()
			        .AddApplicationPart(typeof(Startup).Assembly);
			services.AddHealthChecks();
			services.AddApplication();
			services.AddControllers();
			services.AddSwagger();
		}
	}
}
