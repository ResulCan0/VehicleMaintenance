using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleMaintenance.Migrations
{
    /// <inheritdoc />
    public partial class helpers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "FeaturedNumbers",
                newName: "Values");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "FeaturedNumbers",
                newName: "Keys");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Values",
                table: "FeaturedNumbers",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Keys",
                table: "FeaturedNumbers",
                newName: "Key");
        }
    }
}
