using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Spice.Data.Configurations;
using Spice.Models;

namespace Spice.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public DbSet<Category> Categories { get; set; }
		public DbSet<SubCategory> SubCategories { get; set; }
		public DbSet<MenuItem> MenuItems { get; set; }
		public DbSet<Coupon> Coupons { get; set; }
		public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
		public DbSet<OrderHeader> OrderHeaders { get; set; }
		public DbSet<OrderDetails> OrderDetails { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfiguration(new AspNetRolesConfigurations());
			builder.ApplyConfiguration(new AspNetUsersConfigurations());
			builder.ApplyConfiguration(new AspNetUserRolesConfigurations());
			base.OnModelCreating(builder);
		}
	}
}