using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.Api.Infrastructure.Authorization.Migrations
{
    public partial class update5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BeforeSessionId",
                table: "SESSIONS",
                newName: "BEFORESESSIONID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BEFORESESSIONID",
                table: "SESSIONS",
                newName: "BeforeSessionId");
        }
    }
}
