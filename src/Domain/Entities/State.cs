using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
	public class State
	{
		[Key]
		public int Id { get; set; }
		public string Description { get; set; }
	}
}
