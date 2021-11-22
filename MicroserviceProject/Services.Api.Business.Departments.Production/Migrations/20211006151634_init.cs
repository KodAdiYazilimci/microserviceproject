using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.Api.Business.Departments.Production.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PRODUCTION_PRODUCTS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PRODUCT_ID = table.Column<int>(type: "int", nullable: false),
                    PRODUCT_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCTION_PRODUCTS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCTION_TRANSACTIONS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TRANSACTION_IDENTITY = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    TRANSACTION_TYPE = table.Column<int>(type: "int", nullable: false),
                    TRANSACTION_DATE = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    ISROLLEDBACK = table.Column<bool>(type: "bit", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCTION_TRANSACTIONS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCTION_TRANSACTION_ITEMS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TRANSACTION_IDENTITY = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    ROLLBACK_TYPE = table.Column<string>(type: "NVARCHAR(250)", nullable: false),
                    DATA_SET = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    IDENTITY_ = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    OLD_VALUE = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    NEW_VALUE = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    ISROLLEDBACK = table.Column<bool>(type: "bit", nullable: false),
                    RollbackEntityId = table.Column<int>(type: "int", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCTION_TRANSACTION_ITEMS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PRODUCTION_TRANSACTION_ITEMS_PRODUCTION_TRANSACTIONS_RollbackEntityId",
                        column: x => x.RollbackEntityId,
                        principalTable: "PRODUCTION_TRANSACTIONS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTION_TRANSACTION_ITEMS_RollbackEntityId",
                table: "PRODUCTION_TRANSACTION_ITEMS",
                column: "RollbackEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PRODUCTION_PRODUCTS");

            migrationBuilder.DropTable(
                name: "PRODUCTION_TRANSACTION_ITEMS");

            migrationBuilder.DropTable(
                name: "PRODUCTION_TRANSACTIONS");
        }
    }
}
