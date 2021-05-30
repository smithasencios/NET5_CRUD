using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Feature.Commands.InsertMetrics
{
	public class InsertMetricHandler : IRequestHandler<InsertMetricRequest, InsertMetricResponse>
	{
		private readonly ILogger<InsertMetricHandler> logger;
		public InsertMetricHandler(ILogger<InsertMetricHandler> logger)
		{
			this.logger = logger;
		}

		public async Task<InsertMetricResponse> Handle(InsertMetricRequest request, CancellationToken cancellationToken)
		{
			this.logger.LogInformation($"{JsonConvert.SerializeObject(request)}");
			await File.AppendAllTextAsync("metrics.txt", $"{JsonConvert.SerializeObject(request)}{Environment.NewLine}");

			return new InsertMetricResponse();
		}
	}
}
