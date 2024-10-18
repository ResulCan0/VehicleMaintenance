using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleMaintenance.Migrations
{
    /// <inheritdoc />
    public partial class v15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "CompanyUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Roles = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUsers_RoleId",
                table: "CompanyUsers",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyUsers_Role_RoleId",
                table: "CompanyUsers",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyUsers_Role_RoleId",
                table: "CompanyUsers");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropIndex(
                name: "IX_CompanyUsers_RoleId",
                table: "CompanyUsers");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "CompanyUsers");
        }
    }
}
