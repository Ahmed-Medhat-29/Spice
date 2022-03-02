using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Areas.Admin.ViewModels;
using Spice.Data;
using Spice.Models;
using Spice.Utility;

namespace Spice.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = StaticDetails.Manager)]
	public class MenuItemController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public MenuItemController(ApplicationDbContext context, IWebHostEnvironment env)
		{
			_context = context;
			_webHostEnvironment = env;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			IEnumerable<MenuItem> menuItems = await _context.MenuItems
				.Select(s => new MenuItem
				{
					Id = s.Id,
					Name = s.Name,
					Price = s.Price,
					SubCategory = new SubCategory
					{
						Name = s.SubCategory.Name,
						Category = new Category { Name = s.SubCategory.Category.Name }
					},
				}).ToArrayAsync();
			return View(menuItems);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var model = new MenuItemViewModel { Categories = await GetCategoriesAsync() };
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(MenuItemViewModel model, IFormFile file)
		{
			if (!ModelState.IsValid)
			{
				model.Categories = await GetCategoriesAsync();
				return View(model);
			}

			await _context.MenuItems.AddAsync(model.MenuItem);
			await _context.SaveChangesAsync();

			if (file != null)
			{
				var fileName = model.MenuItem.Id + Path.GetExtension(file.FileName);
				using (var stream = System.IO.File.Create(Path.Combine(_webHostEnvironment.WebRootPath, "img", fileName)))
					file.CopyTo(stream);

				model.MenuItem.Image = @"\img\" + fileName;
				await _context.SaveChangesAsync();
			}

			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			var menuItem = await _context.MenuItems.AsNoTracking()
				.Select(mi => new MenuItem
				{
					Id = mi.Id,
					Name = mi.Name,
					Description = mi.Description,
					Price = mi.Price,
					Image = mi.Image,
					Spicyness = mi.Spicyness,
					SubCategory = new SubCategory
					{
						Name = mi.SubCategory.Name,
						Category = new Category
						{
							Name = mi.SubCategory.Category.Name
						}
					},

				}).SingleOrDefaultAsync(mi => mi.Id == id);

			if (menuItem == null)
				return NotFound();

			var model = new MenuItemViewModel { MenuItem = menuItem };
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var menuItem = await _context.MenuItems
				.AsNoTracking()
				.Include(mi => mi.SubCategory)
				.FirstOrDefaultAsync(mi => mi.Id == id);

			if (menuItem == null)
				return NotFound();

			var model = new MenuItemViewModel
			{
				Categories = await GetCategoriesAsync(),
				MenuItem = menuItem
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(MenuItemViewModel model, IFormFile file)
		{
			if (!ModelState.IsValid)
			{
				model.Categories = await GetCategoriesAsync();
				return View(model);
			}

			var menuItem = await _context.MenuItems.FirstOrDefaultAsync(mi => mi.Id == model.MenuItem.Id);
			if (menuItem == null)
				return NotFound();

			menuItem.Name = model.MenuItem.Name;
			menuItem.Description = model.MenuItem.Description;
			menuItem.Price = model.MenuItem.Price;
			menuItem.SubCategoryId = model.MenuItem.SubCategoryId;
			menuItem.Spicyness = model.MenuItem.Spicyness;

			if (file != null)
			{
				var fileName = menuItem.Id + Path.GetExtension(file.FileName);
				using (var stream = System.IO.File.Create(Path.Combine(_webHostEnvironment.WebRootPath, "img", fileName)))
					file.CopyTo(stream);

				menuItem.Image = @"\img\" + fileName;
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var menuItem = await _context.MenuItems.AsNoTracking()
				.Select(mi => new MenuItem
				{
					Id = mi.Id,
					Name = mi.Name,
					Description = mi.Description,
					Price = mi.Price,
					Image = mi.Image,
					Spicyness = mi.Spicyness,
					SubCategory = new SubCategory
					{
						Name = mi.SubCategory.Name,
						Category = new Category { Name = mi.SubCategory.Category.Name }
					},

				}).FirstOrDefaultAsync(mi => mi.Id == id);

			if (menuItem == null)
				return NotFound();

			var model = new MenuItemViewModel { MenuItem = menuItem };
			return View(model);
		}

		[HttpPost]
		[ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var menuItem = await _context.MenuItems
				.Select(m => new MenuItem
				{
					Id = m.Id,
					Image = m.Image
				}).SingleOrDefaultAsync(mi => mi.Id == id);

			if (menuItem == null)
				return NotFound();

			if (menuItem.Image != null)
				System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, menuItem.Image.TrimStart('\\')));

			_context.MenuItems.Remove(menuItem);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		#region CommonMethods
		private async Task<IEnumerable<Category>> GetCategoriesAsync()
		{
			return await _context.Categories.AsNoTracking().ToArrayAsync();
		}
		#endregion
	}
}