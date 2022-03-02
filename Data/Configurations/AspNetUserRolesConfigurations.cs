using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Spice.Data.Configurations
{
	public class AspNetUserRolesConfigurations : IEntityTypeConfiguration<IdentityUserRole<string>>
	{
		public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
		{
			builder.HasData(
					new IdentityUserRole<string>
					{
						RoleId = "b58626db-6e5d-4fa9-8b0c-0ff3ede6000d",
						UserId = "c2e1bde7-08c4-4b35-b52d-140fbccbb441"
					}
				);
		}
	}
}
