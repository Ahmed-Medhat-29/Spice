using System;
using System.ComponentModel.DataAnnotations;

namespace Spice.Models
{
	public class OrderDetails
	{
		private double _price;

		public int Id { get; set; }

		[Required]
		[StringLength(25, MinimumLength = 3)]
		public string Name { get; set; }

		[Range(1, 10, ErrorMessage = "Value must be between 1 and 10")]
		public byte Count { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Price must be greater than one dollar")]
		public double Price
		{
			get { return _price; }
			set { _price = Math.Round(value, 2); }
		}

		public int OrderHeaderId { get; set; }
		public OrderHeader OrderHeader { get; set; }

		public int MenuItemId { get; set; }
		public MenuItem MenuItem { get; set; }

		public OrderDetails()
		{
			Count = 1;
		}
	}
}