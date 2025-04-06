using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IGAMarket.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_dailyclosur_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalSalePrice",
                table: "DailyClosures",
                newName: "TotalRefund");

            migrationBuilder.RenameColumn(
                name: "TotalDonationPrice",
                table: "DailyClosures",
                newName: "TotalAmount");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Sales",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DailyEndStockQuantity",
                table: "DailyClosures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DailyClosures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PersonelName",
                table: "DailyClosures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "DailyClosures",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "TotalFireQuantity",
                table: "DailyClosures",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DailyEndStockQuantity",
                table: "DailyClosures");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "DailyClosures");

            migrationBuilder.DropColumn(
                name: "PersonelName",
                table: "DailyClosures");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "DailyClosures");

            migrationBuilder.DropColumn(
                name: "TotalFireQuantity",
                table: "DailyClosures");

            migrationBuilder.RenameColumn(
                name: "TotalRefund",
                table: "DailyClosures",
                newName: "TotalSalePrice");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "DailyClosures",
                newName: "TotalDonationPrice");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
