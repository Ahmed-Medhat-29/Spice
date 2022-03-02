using System;
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

namespace Spice.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = StaticDetails.Manager)]
	public class UserController : Controller
	{
		private readonly ApplicationDbContext _context;

		public UserController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			IEnumerable<ApplicationUser> users = await _context.Users
				.Select(u => new ApplicationUser
				{
					Id = u.Id,
					Name = u.Name,
					Email = u.Email,
					PhoneNumber = u.PhoneNumber,
					LockoutEnabled = u.LockoutEnabled,
					LockoutEnd = u.LockoutEnd
				}).Where(u => u.Id != userId).ToArrayAsync();

			return View(users);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Lock(string id)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
			if (user == null)
				return NotFound();

			user.LockoutEnd = DateTimeOffset.Now.UtcDateTime.AddYears(200);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UnLock(string id)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
			if (user == null)
				return NotFound();

			user.LockoutEnd = null;
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}