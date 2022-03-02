using System.Collections.Generic;
using Spice.Models;

namespace Spice.Areas.Customer.ViewModels
{
	public class HomeIndexViewModel
	{
		public IEnumerable<MenuItem> MenuItems { get; set; }
		public IEnumerable<Category> Categories { get; set; }
		public IEnumerable<Coupon> Coupons { get; set; }
	}
}
