using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Spice.Models
{
	public class Coupon
	{
		public int Id { get; set; }

		[Required]
		[StringLength(25, MinimumLength = 3)]
		public string Name { get; set; }

		public short Discount { get; set; }

		public short MinimumAmount { get; set; }

		public byte Type { get; set; }
		public enum EType : byte { Percent, Dollar }

		public bool IsActive { get; set; }

		public byte[] Image { get; set; }

		public void Update(Coupon coupon, IFormFile file)
		{
			Name = coupon.Name;
			Type = coupon.Type;
			Discount = coupon.Discount;
			MinimumAmount = coupon.MinimumAmount;
			IsActive = coupon.IsActive;

			if (file != null)
			{
				using (var fileStream = file.OpenReadStream())
				{
					using (var memoryStream = new MemoryStream())
					{
						fileStream.CopyTo(memoryStream);
						Image = memoryStream.ToArray();
					}
				}
			}
		}

		public double DiscountedPrice(double originalPrice)
		{
			if (MinimumAmount > originalPrice || !IsActive)
				return originalPrice;

			if (Type == (byte)Coupon.EType.Dollar)
				return (originalPrice - Discount);

			if (Type == (byte)Coupon.EType.Percent)
				return (originalPrice - (originalPrice * Discount / 100));

			// Something wrong happened
			return originalPrice;
		}
	}
}
