using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
	public class ProductType
	{
		[Key]
		public int Id { get; set; }
		public string Description { get; set; }
	}
}
