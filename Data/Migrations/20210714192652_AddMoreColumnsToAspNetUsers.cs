using Microsoft.EntityFrameworkCore.Migrations;

namespace Spice.Migrations
{
	public partial class AddMoreColumnsToAspNetUsers : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "City",
				table: "AspNetUsers",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "Name",
				table: "AspNetUsers",
				type: "nvarchar(25)",
				maxLength: 25,
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<int>(
				name: "PostalCode",
				table: "AspNetUsers",
				type: "int",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.AddColumn<string>(
				name: "State",
				table: "AspNetUsers",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "StreetAddress",
				table: "AspNetUsers",
				type: "nvarchar(200)",
				maxLength: 200,
				nullable: false,
				defaultValue: "");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "City",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "Name",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "PostalCode",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "State",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "StreetAddress",
				table: "AspNetUsers");
		}
	}
}
