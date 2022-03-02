using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Spice.Areas.Admin.ViewModels;
using Spice.Data;
using Spice.Models;
using Spice.Utility;

namespace Spice.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = StaticDetails.Manager)]
	public class SubCategoryController : Controller
	{
		private readonly ApplicationDbContext _context;

		public SubCategoryController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			IEnumerable<SubCategory> subCategories = await _context.SubCategories
				.Select(s => new SubCategory
				{
					Id = s.Id,
					Name = s.Name,
					Category = new Category { Name = s.Category.Name }
				}).ToArrayAsync();
			return View(subCategories);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var model = new SubCategoryViewModel { Categories = await GetCategoriesAsync() };
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(SubCategoryViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.Categories = await GetCategoriesAsync();
				return View(model);
			}

			if (await IsSubCategoryExistAsync(model.SubCategory.Name, model.SubCategory.CategoryId))
			{
				model.Categories = await GetCategoriesAsync();
				ModelState.AddModelError("", $"The sub category '{model.SubCategory.Name}' already exists under the same category");
				return View(model);
			}

			await _context.SubCategories.AddAsync(model.SubCategory);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));

		}

		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			var subCategory = await _context.SubCategories
				.Select(t => new SubCategory
				{
					Id = t.Id,
					Name = t.Name,
					Category = new Category { Name = t.Category.Name }
				}).FirstOrDefaultAsync(sc => sc.Id == id);
			return subCategory == null ? NotFound() : View(subCategory);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var subCategory = await _context.SubCategories
				.AsNoTracking()
				.Include(sc => sc.Category)
				.FirstOrDefaultAsync(sc => sc.Id == id);

			return subCategory == null ? NotFound() : View(subCategory);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(SubCategory model)
		{
			var subCategory = await _context.SubCategories.Include(sc => sc.Category).FirstOrDefaultAsync(sc => sc.Id == model.Id);
			if (!ModelState.IsValid)
			{
				model.Category = subCategory.Category;
				return View(model);
			}

			if (await IsSubCategoryExistAsync(model.Name, model.CategoryId))
			{
				model.Category = subCategory.Category;
				ModelState.AddModelError("", $"The sub category '{model.Name}' already exists under the same category");
				return View(model);
			}

			subCategory.Name = model.Name;
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var subCategory = await _context.SubCategories
				.Select(t => new SubCategory
				{
					Id = t.Id,
					Name = t.Name,
					Category = new Category { Name = t.Category.Name }
				}).FirstOrDefaultAsync(sc => sc.Id == id);
			return subCategory == null ? NotFound() : View(subCategory);
		}

		[HttpPost]
		[ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			try
			{
				_context.SubCategories.Remove(new SubCategory { Id = id });
				await _context.SaveChangesAsync();
			}
			catch (Exception)
			{ }

			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> GetSubCategory(int id)
		{
			return Json(await _context.SubCategories.AsNoTracking().Where(sc => sc.CategoryId == id).Select(sc => sc.Name).ToArrayAsync());
		}

		[HttpGet]
		public async Task<IActionResult> GetSubCategoriesIncludeId(int id)
		{
			IEnumerable<SubCategory> subCategories = await _context.SubCategories.AsNoTracking().Where(sc => sc.CategoryId == id).ToArrayAsync();
			return Json(new SelectList(subCategories, "Id", "Name"));
		}

		#region CommonMethods
		private async Task<IEnumerable<Category>> GetCategoriesAsync()
		{
			return await _context.Categories.AsNoTracking().ToArrayAsync();
		}

		private async Task<bool> IsSubCategoryExistAsync(string subCategoryName, int categoryId)
		{
			return string.IsNullOrWhiteSpace(subCategoryName) || categoryId < 1 ? false :
				await _context.SubCategories.AsNoTracking()
				.AnyAsync(sc => sc.Name == subCategoryName && sc.CategoryId == categoryId);
		}
		#endregion
	}
}