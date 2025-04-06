using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IGAMarket.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class daily_closing_entity_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DailyEndStockQuantity",
                table: "DailyClosures");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "DailyClosures");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DailyEndStockQuantity",
                table: "DailyClosures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "DailyClosures",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
