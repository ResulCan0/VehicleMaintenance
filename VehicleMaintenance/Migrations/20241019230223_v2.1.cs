using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleMaintenance.Migrations
{
    /// <inheritdoc />
    public partial class v21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyCompanyModule");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyModules_CompanyId",
                table: "CompanyModules",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyModules_Companies_CompanyId",
                table: "CompanyModules",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyModules_Companies_CompanyId",
                table: "CompanyModules");

            migrationBuilder.DropIndex(
                name: "IX_CompanyModules_CompanyId",
                table: "CompanyModules");

            migrationBuilder.CreateTable(
                name: "CompanyCompanyModule",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyModulesModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyCompanyModule", x => new { x.CompanyId, x.CompanyModulesModuleId });
                    table.ForeignKey(
                        name: "FK_CompanyCompanyModule_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyCompanyModule_CompanyModules_CompanyModulesModuleId",
                        column: x => x.CompanyModulesModuleId,
                        principalTable: "CompanyModules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCompanyModule_CompanyModulesModuleId",
                table: "CompanyCompanyModule",
                column: "CompanyModulesModuleId");
        }
    }
}
