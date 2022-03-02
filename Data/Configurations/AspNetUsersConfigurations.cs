using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spice.Models;

namespace Spice.Data.Configurations
{
	public class AspNetUsersConfigurations : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			var manager = new ApplicationUser
			{
				Id = "c2e1bde7-08c4-4b35-b52d-140fbccbb441",
				Name = "Spice Manager",
				UserName = "manager@spice.com",
				NormalizedUserName = "MANAGER@SPICE.COM",
				Email = "manager@spice.com",
				NormalizedEmail = "MANAGER@SPICE.COM",
				PasswordHash = "AQAAAAEAACcQAAAAEE1uEpZnrTDWl6Vsm1SJ3+qqkKObhB2Mv/mLt7+sRMSHX/LD859A27RIRP276KtE1w==",
				PhoneNumber = "01068218987",
				City = "Cairo",
				PostalCode = 31962,
				State = "Cairo",
				StreetAddress = "Mahmoud Hammam st",
				SecurityStamp = "02312bea-cb12-4c11-ac8b-0ce56b746cc0",
				ConcurrencyStamp = "272e0c35-bcee-4405-9a06-784ed9b7a346"
			};

			//manager.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(manager, "manager@spice.com");
			builder.HasData(manager);
		}
	}
}
