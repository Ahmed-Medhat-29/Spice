using System;
using System.ComponentModel.DataAnnotations;

namespace Spice.Models
{
	public class MenuItem
	{
		private double _price;

		public int Id { get; set; }

		[Required]
		[StringLength(25, MinimumLength = 3)]
		public string Name { get; set; }

		[StringLength(250, MinimumLength = 3)]
		public string Description { get; set; }

		public byte Spicyness { get; set; }

		[StringLength(255, MinimumLength = 1)]
		public string Image { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Price must be greater than one dollar")]
		public double Price
		{
			get { return _price; }
			set { _price = Math.Round(value, 2); }
		}

		public int SubCategoryId { get; set; }
		public SubCategory SubCategory { get; set; }
	}
}