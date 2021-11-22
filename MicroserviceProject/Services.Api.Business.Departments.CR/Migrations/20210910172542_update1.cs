using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.Api.Business.Departments.CR.Migrations
{
    public partial class update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CR_TRANSACTIONS",
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
                    table.PrimaryKey("PK_CR_TRANSACTIONS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CR_TRANSACTION_ITEMS",
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
                    table.PrimaryKey("PK_CR_TRANSACTION_ITEMS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CR_TRANSACTION_ITEMS_CR_TRANSACTIONS_RollbackEntityId",
                        column: x => x.RollbackEntityId,
                        principalTable: "CR_TRANSACTIONS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CR_TRANSACTION_ITEMS_RollbackEntityId",
                table: "CR_TRANSACTION_ITEMS",
                column: "RollbackEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CR_TRANSACTION_ITEMS");

            migrationBuilder.DropTable(
                name: "CR_TRANSACTIONS");
        }
    }
}
