using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.Api.Authorization.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CLAIMTYPES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    CREATEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    UPDATEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DELETEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLAIMTYPES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "POLICIES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    CREATEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    UPDATEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DELETEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POLICIES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ROLES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    CREATEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    UPDATEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DELETEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMAIL = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    PASSWORD = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    SESSIONID = table.Column<int>(type: "int", nullable: true),
                    CREATEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    UPDATEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DELETEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "POLICYROLES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    POLICYID = table.Column<int>(type: "int", nullable: false),
                    ROLEID = table.Column<int>(type: "int", nullable: false),
                    CREATEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    UPDATEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DELETEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POLICYROLES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_POLICYROLES_POLICIES_POLICYID",
                        column: x => x.POLICYID,
                        principalTable: "POLICIES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_POLICYROLES_ROLES_ROLEID",
                        column: x => x.ROLEID,
                        principalTable: "ROLES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CLAIMS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CLAIMTYPEID = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VALUE = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    CREATEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    UPDATEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DELETEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLAIMS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CLAIMS_CLAIMTYPES_CLAIMTYPEID",
                        column: x => x.CLAIMTYPEID,
                        principalTable: "CLAIMTYPES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CLAIMS_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "USERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SESSIONS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IPADDRESS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ISVALID = table.Column<bool>(type: "bit", nullable: false),
                    REGION = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    TOKEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    USERAGENT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    USERID = table.Column<int>(type: "int", nullable: false),
                    VALIDTO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    UPDATEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DELETEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SESSIONS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SESSIONS_USERS_USERID",
                        column: x => x.USERID,
                        principalTable: "USERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USERROLES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ROLEID = table.Column<int>(type: "int", nullable: false),
                    USERID = table.Column<int>(type: "int", nullable: false),
                    CREATEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    UPDATEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DELETEDATE = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERROLES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_USERROLES_ROLES_ROLEID",
                        column: x => x.ROLEID,
                        principalTable: "ROLES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_USERROLES_USERS_USERID",
                        column: x => x.USERID,
                        principalTable: "USERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CLAIMS_CLAIMTYPEID",
                table: "CLAIMS",
                column: "CLAIMTYPEID");

            migrationBuilder.CreateIndex(
                name: "IX_CLAIMS_UserId",
                table: "CLAIMS",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_POLICYROLES_POLICYID",
                table: "POLICYROLES",
                column: "POLICYID");

            migrationBuilder.CreateIndex(
                name: "IX_POLICYROLES_ROLEID",
                table: "POLICYROLES",
                column: "ROLEID");

            migrationBuilder.CreateIndex(
                name: "IX_SESSIONS_USERID",
                table: "SESSIONS",
                column: "USERID");

            migrationBuilder.CreateIndex(
                name: "IX_USERROLES_ROLEID",
                table: "USERROLES",
                column: "ROLEID");

            migrationBuilder.CreateIndex(
                name: "IX_USERROLES_USERID",
                table: "USERROLES",
                column: "USERID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CLAIMS");

            migrationBuilder.DropTable(
                name: "POLICYROLES");

            migrationBuilder.DropTable(
                name: "SESSIONS");

            migrationBuilder.DropTable(
                name: "USERROLES");

            migrationBuilder.DropTable(
                name: "CLAIMTYPES");

            migrationBuilder.DropTable(
                name: "POLICIES");

            migrationBuilder.DropTable(
                name: "ROLES");

            migrationBuilder.DropTable(
                name: "USERS");
        }
    }
}
