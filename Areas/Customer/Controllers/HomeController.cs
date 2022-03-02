using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Areas.Customer.ViewModels;
using Spice.Data;
using Spice.Models;

namespace Spice.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext _context;

		public HomeController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
			return View(new HomeIndexViewModel
			{
				MenuItems = await _context.MenuItems.Include(mi => mi.SubCategory).ToArrayAsync(),
				Categories = await _context.Categories.ToArrayAsync(),
				Coupons = await _context.Coupons.Where(c => c.IsActive == true).ToArrayAsync()
			});
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Details(int id)
		{
			var dbMenuItem = await _context.MenuItems.Include(mi => mi.SubCategory).ThenInclude(sc => sc.Category).AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(m => m.Id == id);
			var shoppingCart = new ShoppingCartItem
			{
				MenuItem = dbMenuItem,
				MenuItemId = dbMenuItem.Id
			};
			return View(shoppingCart);
		}

		public IActionResult Error()
		{
			return View();
		}
	}
}