using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Utility;

namespace Spice.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = StaticDetails.Manager)]
	public class CouponController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CouponController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			IEnumerable<Coupon> coupons = await _context.Coupons
				.Select(c => new Coupon
				{
					Id = c.Id,
					Name = c.Name,
					Discount = c.Discount,
					MinimumAmount = c.MinimumAmount,
					IsActive = c.IsActive
				}).ToArrayAsync();
			return View(coupons);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Coupon coupon, IFormFile file)
		{
			if (!ModelState.IsValid)
				return View(coupon);

			if (file != null)
			{
				using (var fileStream = file.OpenReadStream())
				{
					using (var memoryStream = new MemoryStream())
					{
						fileStream.CopyTo(memoryStream);
						coupon.Image = memoryStream.ToArray();
					}
				}
			}

			await _context.Coupons.AddAsync(coupon);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			var coupon = await GetCouponByIdAsync(id);
			return coupon == null ? NotFound() : View(coupon);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var coupon = await GetCouponByIdAsync(id);
			return coupon == null ? NotFound() : View(coupon);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Coupon coupon, IFormFile file)
		{
			if (!ModelState.IsValid)
				return View(coupon);

			var dbcoupon = await _context.Coupons
				.Select(c => new Coupon
				{
					Id = c.Id,
					Name = c.Name,
					Type = c.Type,
					Discount = c.Discount,
					MinimumAmount = c.MinimumAmount,
					IsActive = c.IsActive
				}).FirstOrDefaultAsync(c => c.Id == coupon.Id);
			if (dbcoupon == null)
				return NotFound();

			_context.Coupons.Attach(dbcoupon);
			dbcoupon.Update(coupon, file);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var coupon = await GetCouponByIdAsync(id);
			return coupon == null ? NotFound() : View(coupon);
		}

		[HttpPost]
		[ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			try
			{
				_context.Coupons.Remove(new Coupon { Id = id });
				await _context.SaveChangesAsync();
			}
			catch (Exception)
			{ }

			return RedirectToAction(nameof(Index));
		}

		#region CommonMethods
		private async Task<Coupon> GetCouponByIdAsync(int id)
		{
			return id < 1 ? null :
				await _context.Coupons.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
		}
		#endregion
	}
}