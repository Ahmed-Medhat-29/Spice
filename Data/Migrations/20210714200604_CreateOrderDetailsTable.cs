using Microsoft.EntityFrameworkCore.Migrations;

namespace Spice.Migrations
{
	public partial class CreateOrderDetailsTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "OrderDetails",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
					Count = table.Column<byte>(type: "tinyint", nullable: false),
					Price = table.Column<double>(type: "float", nullable: false),
					OrderHeaderId = table.Column<int>(type: "int", nullable: false),
					MenuItemId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_OrderDetails", x => x.Id);
					table.ForeignKey(
						name: "FK_OrderDetails_MenuItems_MenuItemId",
						column: x => x.MenuItemId,
						principalTable: "MenuItems",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_OrderDetails_OrderHeaders_OrderHeaderId",
						column: x => x.OrderHeaderId,
						principalTable: "OrderHeaders",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_OrderDetails_MenuItemId",
				table: "OrderDetails",
				column: "MenuItemId");

			migrationBuilder.CreateIndex(
				name: "IX_OrderDetails_OrderHeaderId",
				table: "OrderDetails",
				column: "OrderHeaderId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "OrderDetails");
		}
	}
}
