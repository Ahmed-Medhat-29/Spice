using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spice.Utility;

namespace Spice.Data.Configurations
{
	public class AspNetRolesConfigurations : IEntityTypeConfiguration<IdentityRole>
	{
		public void Configure(EntityTypeBuilder<IdentityRole> builder)
		{
			builder.HasData(
					new IdentityRole
					{
						Id = "b58626db-6e5d-4fa9-8b0c-0ff3ede6000d",
						Name = StaticDetails.Manager,
						NormalizedName = StaticDetails.Manager.ToUpper(),
						ConcurrencyStamp = "37040b7a-b861-45de-aa4d-bb214fa48a4b"
					},
					new IdentityRole
					{
						Id = "87afc828-0745-42bd-924a-9a4363efd687",
						Name = StaticDetails.Kitchen,
						NormalizedName = StaticDetails.Kitchen.ToUpper(),
						ConcurrencyStamp = "a310211f-0bbd-4643-9627-4572b4d67470"
					},
					new IdentityRole
					{
						Id = "a232e9f8-2e0c-4384-8e61-6d4b28a289a6",
						Name = StaticDetails.FrontDesk,
						NormalizedName = StaticDetails.FrontDesk.ToUpper(),
						ConcurrencyStamp = "cf8eb28f-de60-4a60-a095-fef1b070184e"
					}
				);
		}
	}
}
