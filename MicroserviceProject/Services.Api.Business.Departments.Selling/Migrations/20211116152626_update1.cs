using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.Api.Business.Departments.Selling.Migrations
{
    public partial class update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReferenceNumber",
                table: "SELLING_SOLDS",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SellStatusId",
                table: "SELLING_SOLDS",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "SELLING_SOLDS");

            migrationBuilder.DropColumn(
                name: "SellStatusId",
                table: "SELLING_SOLDS");
        }
    }
}
