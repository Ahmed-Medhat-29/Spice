using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Utility;

namespace Spice.Areas.Customer.Controllers
{
	[Area("Customer")]
	[Authorize]
	public class OrderController : Controller
	{
		private readonly ApplicationDbContext _context;

		public OrderController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> Confirm(int id)
		{
			OrderHeader model = await GetOrderDetailsToView(id);
			return model == null ? NotFound() : View(model);
		}

		[HttpGet]
		public async Task<IActionResult> History()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			IEnumerable<OrderHeader> userOrdereHeaders = await _context.OrderHeaders
				.Where(o => o.ApplicationUserId == userId)
				.Select(o => new OrderHeader
				{
					Id = o.Id,
					PickupName = o.PickupName,
					PickupTime = o.PickupTime,
					OrderTotal = o.OrderTotal,
					OrderStatus = o.OrderStatus,
					ApplicationUser = new ApplicationUser { Email = o.ApplicationUser.Email }
				}).ToArrayAsync();

			return View(userOrdereHeaders);
		}

		[HttpGet]
		public async Task<IActionResult> OrderDetails(int id)
		{
			OrderHeader model = await GetOrderDetailsToView(id);
			return model == null ? NotFound() : PartialView("_IndividualOrderDetails", model);
		}

		[HttpGet]
		[Authorize(Roles = StaticDetails.Kitchen + "," + StaticDetails.Manager)]
		public async Task<IActionResult> ManageOrders()
		{
			OrderHeader[] model = await _context.OrderHeaders
				.Where(o => o.OrderStatus == StaticDetails.StatusSubmitted || o.OrderStatus == StaticDetails.StatusInProgress)
				.OrderBy(o => o.PickupTime)
				.Select(o => new OrderHeader
				{
					Id = o.Id,
					PickupTime = o.PickupTime,
					Comments = o.Comments,
					OrderStatus = o.OrderStatus,
					OrderDetails = o.OrderDetails
						.Where(od => od.OrderHeaderId == o.Id)
						.Select(od => new OrderDetails
						{
							Name = od.Name,
							Count = od.Count
						}).ToArray()
				}).ToArrayAsync();

			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = StaticDetails.Kitchen + "," + StaticDetails.Manager)]
		public async Task<IActionResult> PrepareOrder(int id)
		{
			await ChangeOrderStateAsync(id, StaticDetails.StatusInProgress);
			return RedirectToAction(nameof(ManageOrders));
		}

		[HttpPost]
		[Authorize(Roles = StaticDetails.Kitchen + "," + StaticDetails.Manager)]
		public async Task<IActionResult> ReadyOrder(int id)
		{
			await ChangeOrderStateAsync(id, StaticDetails.StatusReady);
			return RedirectToAction(nameof(ManageOrders));
		}

		[HttpPost]
		[Authorize(Roles = StaticDetails.Kitchen + "," + StaticDetails.Manager)]
		public async Task<IActionResult> CancelOrder(int id)
		{
			await ChangeOrderStateAsync(id, StaticDetails.StatusCancelled);
			return RedirectToAction(nameof(ManageOrders));
		}

		[HttpGet]
		[Authorize(Roles = StaticDetails.FrontDesk + "," + StaticDetails.Manager)]
		public async Task<IActionResult> OrderPickup(string searchName = null, string searchEmail = null, string searchNumber = null)
		{
			IEnumerable<OrderHeader> userOrdereHeaders = await _context.OrderHeaders
				.Where(o => o.OrderStatus == StaticDetails.StatusReady &&
					(!string.IsNullOrWhiteSpace(searchName) ? o.PickupName.Contains(searchName) : true) &&
					(!string.IsNullOrWhiteSpace(searchEmail) ? o.ApplicationUser.Email.Contains(searchEmail) : true) &&
					(!string.IsNullOrWhiteSpace(searchNumber) ? o.PickupNumber.Contains(searchNumber) : true))
				.Select(o => new OrderHeader
				{
					Id = o.Id,
					PickupName = o.PickupName,
					PickupNumber = o.PickupNumber,
					PickupTime = o.PickupTime,
					OrderTotal = o.OrderTotal
				}).ToArrayAsync();

			return View(userOrdereHeaders);
		}

		[HttpPost]
		[Authorize(Roles = StaticDetails.Manager + "," + StaticDetails.FrontDesk)]
		public async Task<IActionResult> ConfirmOrderPickup(int id)
		{
			await ChangeOrderStateAsync(id, StaticDetails.StatusCompleted);
			return RedirectToAction(nameof(OrderPickup));
		}

		#region CommonMethods
		private async Task ChangeOrderStateAsync(int orderId, string state)
		{
			var order = await _context.OrderHeaders
				.Select(o => new OrderHeader
				{
					Id = o.Id,
					OrderStatus = o.OrderStatus
				}).FirstOrDefaultAsync(o => o.Id == orderId);

			if (order != null)
			{
				_context.OrderHeaders.Attach(order);
				order.OrderStatus = state;
				await _context.SaveChangesAsync();
			}
		}

		private async Task<OrderHeader> GetOrderDetailsToView(int id)
		{
			return await _context.OrderHeaders
				.Select(o => new OrderHeader
				{
					Id = o.Id,
					PickupName = o.PickupName,
					PickupNumber = o.PickupNumber,
					PickupTime = o.PickupTime,
					OrderTotal = o.OrderTotal,
					Comments = o.Comments,
					CouponCode = o.CouponCode,
					CouponDiscount = o.CouponDiscount,
					OrderStatus = o.OrderStatus,
					ApplicationUserId = o.ApplicationUserId,
					ApplicationUser = new ApplicationUser { Email = o.ApplicationUser.Email },
					OrderDetails = o.OrderDetails
						.Where(od => od.OrderHeaderId == o.Id)
						.Select(od => new OrderDetails
						{
							Name = od.Name,
							Price = od.Price,
							Count = od.Count
						}).ToArray()
				}).FirstOrDefaultAsync(o => o.Id == id);
		}
		#endregion
	}
}