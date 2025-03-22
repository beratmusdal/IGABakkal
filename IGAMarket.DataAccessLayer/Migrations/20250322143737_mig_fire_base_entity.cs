using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IGAMarket.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_fire_base_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Fires",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Fires",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Fires");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Fires");
        }
    }
}
