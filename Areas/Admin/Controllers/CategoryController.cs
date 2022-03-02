using System;
using System.Collections.Generic;
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
	public class CategoryController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CategoryController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			IEnumerable<Category> categories = await _context.Categories.AsNoTracking().ToArrayAsync();
			return View(categories);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Category category)
		{
			if (!ModelState.IsValid)
				return View(category);

			if (await IsCategoryExistAsync(category.Name))
			{
				ModelState.AddModelError("", $"The category '{category.Name}' already exists");
				return View(category);
			}

			await _context.Categories.AddAsync(category);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			var category = await GetCategoryByIdAsync(id);
			return category == null ? NotFound() : View(category);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var category = await GetCategoryByIdAsync(id);
			return category == null ? NotFound() : View(category);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Category category)
		{
			if (!ModelState.IsValid)
				return View(category);

			if (await IsCategoryExistAsync(category.Name))
			{
				ModelState.AddModelError("", $"The category '{category.Name}' already exists");
				return View(category);
			}

			var dbCategory = await GetCategoryByIdAsync(category.Id);
			if (dbCategory == null)
				return NotFound();

			_context.Categories.Attach(dbCategory);
			dbCategory.Name = category.Name;
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var category = await GetCategoryByIdAsync(id);
			return category == null ? NotFound() : View(category);
		}

		[HttpPost]
		[ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			try
			{
				_context.Categories.Remove(new Category { Id = id });
				await _context.SaveChangesAsync();
			}
			catch (Exception)
			{ }

			return RedirectToAction(nameof(Index));
		}

		#region CommonMethods
		private async Task<Category> GetCategoryByIdAsync(int id)
		{
			return id < 1 ? null :
				await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
		}

		private async Task<bool> IsCategoryExistAsync(string name)
		{
			return string.IsNullOrWhiteSpace(name) ? false :
				await _context.Categories.AnyAsync(c => c.Name == name);
		}
		#endregion
	}
}