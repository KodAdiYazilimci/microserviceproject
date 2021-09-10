using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.Business.Departments.CR.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CR_CUSTOMERS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsLegal = table.Column<bool>(type: "bit", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    MİDDLENAME = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    SURNAME = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CR_CUSTOMERS", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CR_CUSTOMERS");
        }
    }
}
