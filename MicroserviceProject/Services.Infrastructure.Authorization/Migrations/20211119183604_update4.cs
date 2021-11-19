using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.Infrastructure.Authorization.Migrations
{
    public partial class update4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BeforeSessionId",
                table: "SESSIONS",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GRANTTYPE",
                table: "SESSIONS",
                type: "NVARCHAR(50)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "REFRESHINDEX",
                table: "SESSIONS",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "REFRESHTOKEN",
                table: "SESSIONS",
                type: "NVARCHAR(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SCOPE",
                table: "SESSIONS",
                type: "NVARCHAR(50)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeforeSessionId",
                table: "SESSIONS");

            migrationBuilder.DropColumn(
                name: "GRANTTYPE",
                table: "SESSIONS");

            migrationBuilder.DropColumn(
                name: "REFRESHINDEX",
                table: "SESSIONS");

            migrationBuilder.DropColumn(
                name: "REFRESHTOKEN",
                table: "SESSIONS");

            migrationBuilder.DropColumn(
                name: "SCOPE",
                table: "SESSIONS");
        }
    }
}
