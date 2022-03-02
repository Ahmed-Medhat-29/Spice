using Microsoft.EntityFrameworkCore.Migrations;

namespace Spice.Migrations
{
	public partial class CreateCouponsTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Coupons",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
					Discount = table.Column<short>(type: "smallint", nullable: false),
					MinimumAmount = table.Column<short>(type: "smallint", nullable: false),
					Type = table.Column<byte>(type: "tinyint", nullable: false),
					IsActive = table.Column<bool>(type: "bit", nullable: false),
					Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Coupons", x => x.Id);
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Coupons");
		}
	}
}
