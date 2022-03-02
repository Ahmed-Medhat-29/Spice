using System.ComponentModel.DataAnnotations;

namespace Spice.Models
{
	public class Category
	{
		public int Id { get; set; }

		[Required]
		[StringLength(25, MinimumLength = 3)]
		public string Name { get; set; }
	}
}