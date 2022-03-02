using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spice.Models
{
	public class OrderHeader
	{
		private double _orderTotal;
		private double _orderTotalOriginal;

		public int Id { get; set; }

		[Required]
		[Display(Name = "Pickup Name")]
		[StringLength(25, MinimumLength = 3)]
		public string PickupName { get; set; }

		[Required]
		[Phone]
		[Display(Name = "Pickup Number")]
		[StringLength(25, MinimumLength = 3)]
		public string PickupNumber { get; set; }

		[NotMapped]
		[Display(Name = "Pickup Date")]
		public DateTime PickupDate { get; set; }

		[Display(Name = "Pickup Time")]
		public DateTime PickupTime { get; set; }

		public DateTime OrderDate { get; set; }

		public double OrderTotalOriginal
		{
			get { return _orderTotalOriginal; }
			set { _orderTotalOriginal = Math.Round(value, 2); }
		}

		public double OrderTotal
		{
			get { return _orderTotal; }
			set { _orderTotal = Math.Round(value, 2); }
		}

		[MinLength(1)]
		public string CouponCode { get; set; }

		public short? CouponDiscount { get; set; }

		[Required]
		[StringLength(25, MinimumLength = 3)]
		public string OrderStatus { get; set; }

		[Required]
		[StringLength(25, MinimumLength = 3)]
		public string PaymentStatus { get; set; }

		[MaxLength(250)]
		public string Comments { get; set; }

		[Required]
		[MaxLength(250)]
		public string TransactionId { get; set; }

		[Required]
		public string ApplicationUserId { get; set; }
		public ApplicationUser ApplicationUser { get; set; }

		public ICollection<OrderDetails> OrderDetails { get; set; }
	}
}