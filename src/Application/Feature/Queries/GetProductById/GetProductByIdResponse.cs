namespace Application.Feature.Queries.GetProductById
{
	public class GetProductByIdResponse
	{
		public int ProductId { get; set; }
		public string Description { get; set; }
		public string Type { get; set; }
		public string State { get; set; }
		public double Stock { get; set; }
		public double Price { get; set; }
	}
}
