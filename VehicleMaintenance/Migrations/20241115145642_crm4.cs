using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleMaintenance.Migrations
{
    /// <inheritdoc />
    public partial class crm4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationDate",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Applications",
                newName: "Name");

            migrationBuilder.AddColumn<bool>(
                name: "Ecommerce",
                table: "Applications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "SeoOptimization",
                table: "Applications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "StockTracking",
                table: "Applications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "VehicleTracking",
                table: "Applications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ecommerce",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "SeoOptimization",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "StockTracking",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "VehicleTracking",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Applications",
                newName: "UserName");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApplicationDate",
                table: "Applications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
