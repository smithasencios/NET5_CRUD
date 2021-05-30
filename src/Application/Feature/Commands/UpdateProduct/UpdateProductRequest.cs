using MediatR;

namespace Application.Feature.Commands.UpdateProduct
{
	public class UpdateProductRequest : IRequest<UpdateProductResponse>
	{
		public int Id { get; set; }
		public string Description { get; set; }

		public double Stock { get; set; }

		public double Price { get; set; }

		public UpdateProductRequest SetId(int id)
		{
			this.Id = id;
			return this;
		}
	}
}
