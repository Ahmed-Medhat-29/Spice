using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Spice.Migrations
{
	public partial class CreateOrderHeadersTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "OrderHeaders",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					PickupName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
					PickupNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
					PickupTime = table.Column<DateTime>(type: "datetime2", nullable: false),
					OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					OrderTotalOriginal = table.Column<double>(type: "float", nullable: false),
					OrderTotal = table.Column<double>(type: "float", nullable: false),
					CouponCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
					CouponDiscount = table.Column<short>(type: "smallint", nullable: true),
					OrderStatus = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
					PaymentStatus = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
					Comments = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
					TransactionId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
					ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_OrderHeaders", x => x.Id);
					table.ForeignKey(
						name: "FK_OrderHeaders_AspNetUsers_ApplicationUserId",
						column: x => x.ApplicationUserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_OrderHeaders_ApplicationUserId",
				table: "OrderHeaders",
				column: "ApplicationUserId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "OrderHeaders");
		}
	}
}
