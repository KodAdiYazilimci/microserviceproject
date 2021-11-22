using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.Api.Business.Departments.Production.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PRODUCTION_PRODUCT_DEPENDENCIES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PRODUCT_ID = table.Column<int>(type: "int", nullable: false),
                    DEPENDED_PRODUCT_ID = table.Column<int>(type: "int", nullable: false),
                    AMOUNT = table.Column<int>(type: "int", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCTION_PRODUCT_DEPENDENCIES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRODUCTION_PRODUCT_DEPENDENCIES_PRODUCTION_PRODUCTS_DEPENDED_PRODUCT_ID",
                        column: x => x.DEPENDED_PRODUCT_ID,
                        principalTable: "PRODUCTION_PRODUCTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PRODUCTION_PRODUCT_DEPENDENCIES_PRODUCTION_PRODUCTS_PRODUCT_ID",
                        column: x => x.PRODUCT_ID,
                        principalTable: "PRODUCTION_PRODUCTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTION_PRODUCT_DEPENDENCIES_DEPENDED_PRODUCT_ID",
                table: "PRODUCTION_PRODUCT_DEPENDENCIES",
                column: "DEPENDED_PRODUCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTION_PRODUCT_DEPENDENCIES_PRODUCT_ID",
                table: "PRODUCTION_PRODUCT_DEPENDENCIES",
                column: "PRODUCT_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PRODUCTION_PRODUCT_DEPENDENCIES");
        }
    }
}
