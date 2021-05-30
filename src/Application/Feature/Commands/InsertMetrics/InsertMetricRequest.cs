using MediatR;
using System;

namespace Application.Feature.Commands.InsertMetrics
{
	public class InsertMetricRequest: IRequest<InsertMetricResponse>
	{
		public string Method { get; set; }

		public DateTime RequestDate { get; set; }

		public long RequestDuration { get; set; }
	}
}
