using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Areas.Identity.ViewModels;
using Spice.Data;
using Spice.Models;
using Spice.Utility;

namespace Spice.Areas.Identity.Controllers
{
	[Area("Identity")]
	public class AccountController : Controller
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly ApplicationDbContext _context;

		public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_roleManager = roleManager;
			_context = context;
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model, string role)
		{
			if (!ModelState.IsValid)
				return View(model);

			var user = new ApplicationUser
			{
				Name = model.Name,
				UserName = model.Email,
				Email = model.Email,
				PhoneNumber = model.PhoneNumber,
				StreetAddress = model.StreetAddress,
				City = model.City,
				State = model.State,
				PostalCode = model.PostalCode
			};
			var result = await _userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
					ModelState.AddModelError("", error.Description);
				return View(model);
			}

			if (role != null && User.IsInRole(StaticDetails.Manager))
				await _userManager.AddToRoleAsync(user, role);

			return RedirectToAction(nameof(Login));
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
		{
			var dbUser = await _userManager.FindByEmailAsync(model.Email);
			if (dbUser == null)
			{
				ModelState.AddModelError("", "Please register first");
				return View(model);
			}

			var result = await _signInManager.PasswordSignInAsync(dbUser, model.Password, false, true);
			if (result.Succeeded)
			{
				int shoppingCartsNumber = await _context.ShoppingCartItems.CountAsync(s => s.ApplicationUserId == dbUser.Id);
				HttpContext.Session.SetInt32(StaticDetails.CartCount, shoppingCartsNumber);
				if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
					return Redirect(returnUrl);

				return RedirectToAction("Index", "Home", new { area = "Customer" });
			}


			if (result.IsLockedOut)
			{
				ModelState.AddModelError("", "Account has been locked");
				return View(model);
			}

			ModelState.AddModelError("", "Invalid login attempt");
			return View(model);
		}

		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout(ApplicationUser user, string password)
		{
			await _signInManager.SignOutAsync();
			HttpContext.Session.SetInt32(StaticDetails.CartCount, 0);
			return RedirectToAction("Index", "Home", new { area = "Customer" });
		}
	}
}