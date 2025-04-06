using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IGAMarket.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_primary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Barcode",
                table: "Sepetler",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sepetler",
                table: "Sepetler",
                column: "Barcode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Sepetler",
                table: "Sepetler");

            migrationBuilder.AlterColumn<string>(
                name: "Barcode",
                table: "Sepetler",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
