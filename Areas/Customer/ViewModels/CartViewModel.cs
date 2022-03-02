using System;
using System.Collections.Generic;
using Spice.Models;
using Spice.Utility;

namespace Spice.Areas.Customer.ViewModels
{
	public class CartViewModel
	{
		public IEnumerable<ShoppingCartItem> ShoppingCartItems { get; set; }
		public OrderHeader OrderHeader { get; set; }

		public CartViewModel()
		{
			OrderHeader = new OrderHeader();
		}

		public void CalculateTotalPrice()
		{
			OrderHeader.OrderTotalOriginal = 0;
			foreach (var item in ShoppingCartItems)
				OrderHeader.OrderTotalOriginal += (item.MenuItem.Price * item.Count);

			OrderHeader.OrderTotal = OrderHeader.OrderTotalOriginal;
		}

		public void PrepareOrderForSubmitting(string userId)
		{
			OrderHeader.ApplicationUserId = userId;
			OrderHeader.OrderDate = DateTime.Now;
			OrderHeader.OrderStatus = StaticDetails.StatusSubmitted;
			OrderHeader.PaymentStatus = StaticDetails.PaymentStatusPending;
			OrderHeader.PickupTime = Convert.ToDateTime(OrderHeader.PickupDate.ToShortDateString() + " " + OrderHeader.PickupTime.ToShortTimeString());
			OrderHeader.TransactionId = "0";
		}

		public void InitializeOrderDetails()
		{
			OrderHeader.OrderDetails = new List<OrderDetails>();
			foreach (var item in ShoppingCartItems)
				OrderHeader.OrderDetails.Add(new OrderDetails
				{
					Name = item.MenuItem.Name,
					Count = item.Count,
					Price = item.MenuItem.Price,
					OrderHeaderId = OrderHeader.Id,
					MenuItemId = item.MenuItem.Id
				});
		}
	}
}