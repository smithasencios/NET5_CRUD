using MediatR;

namespace Application.Feature.Queries.GetProductById
{
	public class GetProductByIdRequest : IRequest<GetProductByIdResponse>
	{
		public int ProductId { get; set; }
	}
}
