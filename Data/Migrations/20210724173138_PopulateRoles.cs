using Microsoft.EntityFrameworkCore.Migrations;

namespace Spice.Migrations
{
	public partial class PopulateRoles : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.InsertData(
				table: "AspNetRoles",
				columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
				values: new object[] { "b58626db-6e5d-4fa9-8b0c-0ff3ede6000d", "37040b7a-b861-45de-aa4d-bb214fa48a4b", "Manager", "MANAGER" });

			migrationBuilder.InsertData(
				table: "AspNetRoles",
				columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
				values: new object[] { "87afc828-0745-42bd-924a-9a4363efd687", "a310211f-0bbd-4643-9627-4572b4d67470", "Kitchen", "KITCHEN" });

			migrationBuilder.InsertData(
				table: "AspNetRoles",
				columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
				values: new object[] { "a232e9f8-2e0c-4384-8e61-6d4b28a289a6", "cf8eb28f-de60-4a60-a095-fef1b070184e", "FrontDesk", "FRONTDESK" });
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DeleteData(
				table: "AspNetRoles",
				keyColumn: "Id",
				keyValue: "87afc828-0745-42bd-924a-9a4363efd687");

			migrationBuilder.DeleteData(
				table: "AspNetRoles",
				keyColumn: "Id",
				keyValue: "a232e9f8-2e0c-4384-8e61-6d4b28a289a6");

			migrationBuilder.DeleteData(
				table: "AspNetRoles",
				keyColumn: "Id",
				keyValue: "b58626db-6e5d-4fa9-8b0c-0ff3ede6000d");
		}
	}
}
