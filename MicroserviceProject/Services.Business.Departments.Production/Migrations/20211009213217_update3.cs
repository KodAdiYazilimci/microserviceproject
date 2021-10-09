using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.Business.Departments.Production.Migrations
{
    public partial class update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "REQUESTED_AMOUNT",
                table: "PRODUCTION_PRODUCTIONS",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PRODUCTION_PRODUCTIONS_ITEMS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PRODUCTION_ID = table.Column<int>(type: "int", nullable: false),
                    PRODUCT_DEPENDEDPRODUCTID = table.Column<int>(type: "int", nullable: false),
                    REQUIRED_AMOUNT = table.Column<int>(type: "int", nullable: false),
                    STATUS_ID = table.Column<int>(type: "int", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCTION_PRODUCTIONS_ITEMS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRODUCTION_PRODUCTIONS_ITEMS_PRODUCTION_PRODUCTIONS_PRODUCTION_ID",
                        column: x => x.PRODUCTION_ID,
                        principalTable: "PRODUCTION_PRODUCTIONS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PRODUCTION_PRODUCTIONS_ITEMS_PRODUCTION_PRODUCTS_PRODUCT_DEPENDEDPRODUCTID",
                        column: x => x.PRODUCT_DEPENDEDPRODUCTID,
                        principalTable: "PRODUCTION_PRODUCTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTION_PRODUCTIONS_ITEMS_PRODUCT_DEPENDEDPRODUCTID",
                table: "PRODUCTION_PRODUCTIONS_ITEMS",
                column: "PRODUCT_DEPENDEDPRODUCTID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTION_PRODUCTIONS_ITEMS_PRODUCTION_ID",
                table: "PRODUCTION_PRODUCTIONS_ITEMS",
                column: "PRODUCTION_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PRODUCTION_PRODUCTIONS_ITEMS");

            migrationBuilder.DropColumn(
                name: "REQUESTED_AMOUNT",
                table: "PRODUCTION_PRODUCTIONS");
        }
    }
}
