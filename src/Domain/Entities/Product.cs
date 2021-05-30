using Domain.Common;
using Nest;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
	[ElasticsearchType(IdProperty = nameof(ProductId))]
	public class Product : AuditableEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ProductId { get; set; }

		[MaxLength(255)]
		public string Description { get; set; }

		[ForeignKey("TypeId")]
		public ProductType ProductType { get; set; }

		public int TypeId { get; set; }

		[ForeignKey("StateId")]
		public State State { get; set; }

		public int StateId { get; set; }

		public double Stock { get; set; }

		public double Price { get; set; }
	}
}
