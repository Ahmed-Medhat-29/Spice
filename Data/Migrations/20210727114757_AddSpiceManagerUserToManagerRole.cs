using Microsoft.EntityFrameworkCore.Migrations;

namespace Spice.Migrations
{
	public partial class AddSpiceManagerUserToManagerRole : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.InsertData(
				table: "AspNetUserRoles",
				columns: new[] { "RoleId", "UserId" },
				values: new object[] { "b58626db-6e5d-4fa9-8b0c-0ff3ede6000d", "c2e1bde7-08c4-4b35-b52d-140fbccbb441" });
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DeleteData(
				table: "AspNetUserRoles",
				keyColumns: new[] { "RoleId", "UserId" },
				keyValues: new object[] { "b58626db-6e5d-4fa9-8b0c-0ff3ede6000d", "c2e1bde7-08c4-4b35-b52d-140fbccbb441" });
		}
	}
}
