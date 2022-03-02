using Microsoft.EntityFrameworkCore.Migrations;

namespace Spice.Migrations
{
	public partial class CreateMenuItemsTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "MenuItems",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
					Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
					Spicyness = table.Column<byte>(type: "tinyint", nullable: false),
					Image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
					Price = table.Column<double>(type: "float", nullable: false),
					SubCategoryId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_MenuItems", x => x.Id);
					table.ForeignKey(
						name: "FK_MenuItems_SubCategories_SubCategoryId",
						column: x => x.SubCategoryId,
						principalTable: "SubCategories",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_MenuItems_SubCategoryId",
				table: "MenuItems",
				column: "SubCategoryId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "MenuItems");
		}
	}
}
