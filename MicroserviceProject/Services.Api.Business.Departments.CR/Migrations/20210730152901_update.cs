using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.Api.Business.Departments.CR.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsLegal",
                table: "CR_CUSTOMERS",
                newName: "ISLEGAL");

            migrationBuilder.RenameColumn(
                name: "MİDDLENAME",
                table: "CR_CUSTOMERS",
                newName: "MIDDLENAME");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ISLEGAL",
                table: "CR_CUSTOMERS",
                newName: "IsLegal");

            migrationBuilder.RenameColumn(
                name: "MIDDLENAME",
                table: "CR_CUSTOMERS",
                newName: "MİDDLENAME");
        }
    }
}
