using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Areas.Customer.ViewModels;
using Spice.Data;
using Spice.Models;
using Spice.Utility;
using Stripe;

namespace Spice.Areas.Customer.Controllers
{
	[Area("Customer")]
	[Authorize]
	public class CartController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CartController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			IEnumerable<ShoppingCartItem> userCart = await _context.ShoppingCartItems
				.Where(s => s.ApplicationUserId == userId)
				.Select(sc => new ShoppingCartItem
				{
					Id = sc.Id,
					Count = sc.Count,
					MenuItem = new MenuItem
					{
						Name = sc.MenuItem.Name,
						Description = sc.MenuItem.Description.Length > 100 ? sc.MenuItem.Description.Substring(0, 119) + "..." : sc.MenuItem.Description,
						Image = sc.MenuItem.Image,
						Price = sc.MenuItem.Price
					}
				}).ToArrayAsync();

			var model = new CartViewModel { ShoppingCartItems = userCart };
			model.CalculateTotalPrice();
			await CalculateCouponDiscountAsync(model.OrderHeader);
			HttpContext.Session.SetInt32(StaticDetails.CartCount, userCart.Count());
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Summary()
		{
			var appUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserName == HttpContext.User.Identity.Name);
			IEnumerable<ShoppingCartItem> userCart = await _context.ShoppingCartItems
				.Where(s => s.ApplicationUserId == appUser.Id)
				.Select(sc => new ShoppingCartItem
				{
					Count = sc.Count,
					MenuItem = new MenuItem
					{
						Name = sc.MenuItem.Name,
						Price = sc.MenuItem.Price
					}
				}).ToArrayAsync();

			if (!userCart.Any())
				return RedirectToAction(nameof(Index));

			var model = new CartViewModel { ShoppingCartItems = userCart };
			model.OrderHeader.PickupName = appUser.Name;
			model.OrderHeader.PickupNumber = appUser.PhoneNumber;
			model.CalculateTotalPrice();
			await CalculateCouponDiscountAsync(model.OrderHeader);
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Summary(CartViewModel model, string stripeToken)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			IEnumerable<ShoppingCartItem> userCart = await _context.ShoppingCartItems
				.Where(sc => sc.ApplicationUserId == userId)
				.Select(sc => new ShoppingCartItem
				{
					Id = sc.Id,
					Count = sc.Count,
					MenuItem = new MenuItem
					{
						Id = sc.MenuItem.Id,
						Name = sc.MenuItem.Name,
						Price = sc.MenuItem.Price
					}
				}).ToArrayAsync();

			if (!userCart.Any())
				return RedirectToAction(nameof(Index));

			model.ShoppingCartItems = userCart;
			model.CalculateTotalPrice();
			model.PrepareOrderForSubmitting(userId);
			if (model.OrderHeader.PickupTime <= DateTime.Now.AddHours(1))
			{
				ModelState.AddModelError("", "Order Pickup time is not valid");
				return View(model);
			}

			model.InitializeOrderDetails();
			await CalculateCouponDiscountAsync(model.OrderHeader);

			await _context.OrderHeaders.AddAsync(model.OrderHeader);
			await _context.SaveChangesAsync();

			_context.ShoppingCartItems.RemoveRange(userCart);
			await _context.SaveChangesAsync();

			var options = new ChargeCreateOptions
			{
				Amount = Convert.ToInt32(model.OrderHeader.OrderTotal * 100),
				Currency = "usd",
				Description = "Order ID : " + model.OrderHeader.Id,
				Source = stripeToken
			};

			var service = new ChargeService();
			Charge charge = service.Create(options);

			if (charge.Status.ToLower() == "succeeded")
			{
				model.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusApproved;
				model.OrderHeader.TransactionId = charge.BalanceTransactionId;
			}
			else
				model.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusRejected;

			await _context.SaveChangesAsync();
			HttpContext.Session.SetString(StaticDetails.CouponCode, "");
			HttpContext.Session.SetInt32(StaticDetails.CartCount, 0);
			return RedirectToAction("Confirm", "Order", new { Id = model.OrderHeader.Id });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult AddCoupon(CartViewModel model)
		{
			if (!string.IsNullOrWhiteSpace(model.OrderHeader.CouponCode))
				HttpContext.Session.SetString(StaticDetails.CouponCode, model.OrderHeader.CouponCode);

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult removeCoupon()
		{
			HttpContext.Session.SetString(StaticDetails.CouponCode, "");
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Plus(int cartId)
		{
			var cart = await _context.ShoppingCartItems
				.Select(sc => new ShoppingCartItem
				{
					Id = sc.Id,
					Count = sc.Count
				}).FirstOrDefaultAsync(s => s.Id == cartId);

			_context.ShoppingCartItems.Attach(cart);
			if (cart.Count < 10)
				cart.Count += 1;

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Minus(int cartId)
		{
			var cart = await _context.ShoppingCartItems
				.Select(sc => new ShoppingCartItem
				{
					Id = sc.Id,
					Count = sc.Count
				}).FirstOrDefaultAsync(s => s.Id == cartId);

			if (cart.Count == 1)
				_context.ShoppingCartItems.Remove(cart);
			else
			{
				_context.ShoppingCartItems.Attach(cart);
				cart.Count -= 1;
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Add(int cartId, byte count)
		{
			if (count < 1 || count > 10)
				return BadRequest("Count must be less than or equal to 10");

			var dbMenuItem = await _context.MenuItems.AsNoTracking().FirstOrDefaultAsync(m => m.Id == cartId);
			if (dbMenuItem == null)
				return NotFound();

			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var dbShoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(sc => sc.ApplicationUserId == userId && sc.MenuItemId == cartId);
			if (dbShoppingCartItem == null)
			{
				await _context.ShoppingCartItems.AddAsync(new ShoppingCartItem
				{
					Count = count,
					MenuItemId = dbMenuItem.Id,
					ApplicationUserId = userId
				});
			}
			else
			{
				dbShoppingCartItem.Count += count;
				if (dbShoppingCartItem.Count > 10)
					dbShoppingCartItem.Count = 10;
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Remove(int cartId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var cart = await _context.ShoppingCartItems.Select(sc => new ShoppingCartItem
			{
				Id = sc.Id,
				ApplicationUserId = sc.ApplicationUserId
			}).FirstOrDefaultAsync(sc => sc.Id == cartId && sc.ApplicationUserId == userId);

			_context.ShoppingCartItems.Remove(cart);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		#region CommonMethods
		private async Task CalculateCouponDiscountAsync(OrderHeader order)
		{
			var ssCouponCode = HttpContext.Session.GetString(StaticDetails.CouponCode);
			if (!string.IsNullOrWhiteSpace(ssCouponCode))
			{
				order.CouponCode = ssCouponCode;
				var coupon = await _context.Coupons.Select(c => new Models.Coupon
				{
					Name = c.Name,
					MinimumAmount = c.MinimumAmount,
					IsActive = c.IsActive,
					Type = c.Type,
					Discount = c.Discount
				}).FirstOrDefaultAsync(c => c.Name.ToLower() == ssCouponCode.ToLower());
				order.OrderTotal = coupon?.DiscountedPrice(order.OrderTotalOriginal) ?? order.OrderTotalOriginal;
			}
			order.CouponDiscount = (short)(order.OrderTotalOriginal - order.OrderTotal);
		}
		#endregion
	}
}
