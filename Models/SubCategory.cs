using System.ComponentModel.DataAnnotations;

namespace Spice.Models
{
	public class SubCategory
	{
		public int Id { get; set; }

		[Required]
		[StringLength(25, MinimumLength = 3)]
		public string Name { get; set; }

		public int CategoryId { get; set; }
		public Category Category { get; set; }
	}
}