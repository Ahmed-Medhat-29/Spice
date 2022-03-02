using System;
using System.ComponentModel.DataAnnotations;

namespace Spice.Models
{
	public class ShoppingCartItem
	{
		public int Id { get; set; }

		[Range(1, 10, ErrorMessage = "Value must be between 1 and 10")]
		public byte Count { get; set; }

		[Required]
		public string ApplicationUserId { get; set; }
		public ApplicationUser ApplicationUser { get; set; }

		public int MenuItemId { get; set; }
		public MenuItem MenuItem { get; set; }

		public ShoppingCartItem()
		{
			Count = 1;
		}
	}
}