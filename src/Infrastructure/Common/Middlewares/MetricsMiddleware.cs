using Application.Feature.Commands.InsertMetrics;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Infrastructure.Common.Middlewares
{
	public class MetricsMiddleware
	{
		private readonly RequestDelegate next;
		private readonly IMediator mediator;

		/// <summary>
		/// Initializes a new instance of the <see cref="MetricsMiddleware"/> class.
		/// </summary>
		/// <param name="next">A function that can process an HTTP request.</param>
		/// <param name="mediator">Defines a mediator to encapsulate request/response.</param>
		public MetricsMiddleware(RequestDelegate next, IMediator mediator)
		{
			this.next = next;
			this.mediator = mediator;
		}

		/// <summary>
		/// This method is going to intercept the request.
		/// </summary>
		/// <param name="httpContext">Encapsulates all HTTP-specific information about an individual HTTP request.</param>
		/// <returns>A representing the asynchronous operation.</returns>
		public async Task InvokeAsync(HttpContext httpContext)
		{
			if (httpContext == null)
			{
				throw new ArgumentNullException(nameof(httpContext));
			}
			var watch = Stopwatch.StartNew();
			await this.next(httpContext).ConfigureAwait(false);
			await this.RecordMetrics(httpContext, watch).ConfigureAwait(false);
		}

		private async Task RecordMetrics(HttpContext httpContext, Stopwatch stopwatch)
		{
			stopwatch.Stop();
			var metricsRequest = new InsertMetricRequest()
			{
				Method = $"{httpContext.Request.Method} - {httpContext.Request.Path}",
				RequestDate = DateTime.UtcNow,
				RequestDuration = stopwatch.ElapsedMilliseconds
			};

			_ = this.mediator.Send(metricsRequest);
		}
	}
}

