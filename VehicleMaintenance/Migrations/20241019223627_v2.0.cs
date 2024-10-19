using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleMaintenance.Migrations
{
    /// <inheritdoc />
    public partial class v20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyCompanyModule_CompanyModule_CompanyModulesModuleId",
                table: "CompanyCompanyModule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyModule",
                table: "CompanyModule");

            migrationBuilder.RenameTable(
                name: "CompanyModule",
                newName: "CompanyModules");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyModules",
                table: "CompanyModules",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyCompanyModule_CompanyModules_CompanyModulesModuleId",
                table: "CompanyCompanyModule",
                column: "CompanyModulesModuleId",
                principalTable: "CompanyModules",
                principalColumn: "ModuleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyCompanyModule_CompanyModules_CompanyModulesModuleId",
                table: "CompanyCompanyModule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyModules",
                table: "CompanyModules");

            migrationBuilder.RenameTable(
                name: "CompanyModules",
                newName: "CompanyModule");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyModule",
                table: "CompanyModule",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyCompanyModule_CompanyModule_CompanyModulesModuleId",
                table: "CompanyCompanyModule",
                column: "CompanyModulesModuleId",
                principalTable: "CompanyModule",
                principalColumn: "ModuleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
