using Microsoft.EntityFrameworkCore.Migrations;

namespace Spice.Migrations
{
	public partial class SeedManagerData : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.InsertData(
				table: "AspNetUsers",
				columns: new[] { "Id", "AccessFailedCount", "City", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PostalCode", "SecurityStamp", "State", "StreetAddress", "TwoFactorEnabled", "UserName" },
				values: new object[] { "c2e1bde7-08c4-4b35-b52d-140fbccbb441", 0, "Cairo", "272e0c35-bcee-4405-9a06-784ed9b7a346", "manager@spice.com", false, false, null, "Spice Manager", "MANAGER@SPICE.COM", "MANAGER@SPICE.COM", "AQAAAAEAACcQAAAAEE1uEpZnrTDWl6Vsm1SJ3+qqkKObhB2Mv/mLt7+sRMSHX/LD859A27RIRP276KtE1w==", "01068218987", false, 31962, "02312bea-cb12-4c11-ac8b-0ce56b746cc0", "Cairo", "Mahmoud Hammam st", false, "manager@spice.com" });
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DeleteData(
				table: "AspNetUsers",
				keyColumn: "Id",
				keyValue: "c2e1bde7-08c4-4b35-b52d-140fbccbb441");
		}
	}
}
