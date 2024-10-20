using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleMaintenance.Migrations
{
    /// <inheritdoc />
    public partial class v22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyModules",
                table: "CompanyModules");

            migrationBuilder.DropColumn(
                name: "ModuleCode",
                table: "CompanyModules");

            migrationBuilder.DropColumn(
                name: "ModuleName",
                table: "CompanyModules");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "CompanyModules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyModuleId",
                table: "CompanyModules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedDate",
                table: "CompanyModules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CompanyModules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyModules",
                table: "CompanyModules",
                column: "CompanyModuleId");

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModuleCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.ModuleId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyModules_ModuleId",
                table: "CompanyModules",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyModules_Modules_ModuleId",
                table: "CompanyModules",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "ModuleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyModules_Modules_ModuleId",
                table: "CompanyModules");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyModules",
                table: "CompanyModules");

            migrationBuilder.DropIndex(
                name: "IX_CompanyModules_ModuleId",
                table: "CompanyModules");

            migrationBuilder.DropColumn(
                name: "CompanyModuleId",
                table: "CompanyModules");

            migrationBuilder.DropColumn(
                name: "AssignedDate",
                table: "CompanyModules");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CompanyModules");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "CompanyModules",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "ModuleCode",
                table: "CompanyModules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ModuleName",
                table: "CompanyModules",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyModules",
                table: "CompanyModules",
                column: "ModuleId");
        }
    }
}
