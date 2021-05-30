using Application;
using Infrastructure;
using Infrastructure.Common.GlobalExceptionMiddleware;
using Infrastructure.Common.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api
{
	public class Startup
	{
		public Startup()
		{
			this.Configuration = new ConfigurationBuilder()
									.AddEnvironmentVariables()
									.Build();
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public virtual void ConfigureServices(IServiceCollection services)
		{
			services.AddHealthChecks();
			services.AddApplication();
			services.AddInfrastructure(this.Configuration);
			services.AddControllers();
			services.AddSwagger();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();
			app.UseSwagger()
			.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("../swagger/v1/swagger.json", "My API V1");
			});

			app.UseMiddleware<MetricsMiddleware>();
			app.UseMiddleware<ExceptionMiddleware>();
			app.UseRouting();
			app.UseStaticFiles();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapHealthChecks("/health");
				endpoints.MapControllers();
			});
		}
	}
}
