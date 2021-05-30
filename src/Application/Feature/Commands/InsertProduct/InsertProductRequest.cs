using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feature.Commands.InsertProduct
{
	public class InsertProductRequest : IRequest<InsertProductResponse>
	{
		public string Description { get; set; }

		public int TypeId { get; set; }

		public int StateId { get; set; }

		public double Stock { get; set; }

		public double Price { get; set; }
	}
}
