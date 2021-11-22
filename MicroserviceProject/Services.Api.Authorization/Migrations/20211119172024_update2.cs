using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.Api.Infrastructure.Authorization.Migrations
{
    public partial class update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SESSIONID",
                table: "USERS");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SESSIONID",
                table: "USERS",
                type: "int",
                nullable: true);
        }
    }
}
