using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spice.Data;
using Spice.Models;
using Spice.Utility;
using Stripe;

namespace Spice
{
	public class Startup
	{
		private readonly IConfiguration _config;

		public Startup(IConfiguration config)
		{
			_config = config;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContextPool<ApplicationDbContext>(options =>
				options.UseSqlServer(_config.GetConnectionString("DefaultConnection")));

			services.AddIdentity<ApplicationUser, IdentityRole>(setup =>
				{
					setup.Lockout.MaxFailedAccessAttempts = 3;
					setup.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
					setup.Password.RequireDigit = false;
					setup.Password.RequireUppercase = false;
				})
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.Services.ConfigureApplicationCookie(config =>
					config.LoginPath = "/Identity/Account/Login");

			services.AddControllersWithViews();

			services.AddSession(options =>
				{
					options.Cookie.IsEssential = true;
					options.IdleTimeout = TimeSpan.FromMinutes(30);
				});

			services.Configure<StripeSettings>(_config.GetSection("Stripe"));
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();
			else
			{
				app.UseExceptionHandler("/Customer/Home/Error");
				//app.UseHsts();
			}

			//app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseSession();
			app.UseAuthentication();
			app.UseAuthorization();

			StripeConfiguration.ApiKey = _config.GetSection("Stripe")["Secretkey"];

			app.UseEndpoints(endpoints =>
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}")
			);
		}
	}
}