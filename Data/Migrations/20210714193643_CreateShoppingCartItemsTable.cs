using Microsoft.EntityFrameworkCore.Migrations;

namespace Spice.Migrations
{
	public partial class CreateShoppingCartItemsTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "ShoppingCartItems",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Count = table.Column<byte>(type: "tinyint", nullable: false),
					ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					MenuItemId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ShoppingCartItems", x => x.Id);
					table.ForeignKey(
						name: "FK_ShoppingCartItems_AspNetUsers_ApplicationUserId",
						column: x => x.ApplicationUserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_ShoppingCartItems_MenuItems_MenuItemId",
						column: x => x.MenuItemId,
						principalTable: "MenuItems",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_ShoppingCartItems_ApplicationUserId",
				table: "ShoppingCartItems",
				column: "ApplicationUserId");

			migrationBuilder.CreateIndex(
				name: "IX_ShoppingCartItems_MenuItemId",
				table: "ShoppingCartItems",
				column: "MenuItemId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "ShoppingCartItems");
		}
	}
}
