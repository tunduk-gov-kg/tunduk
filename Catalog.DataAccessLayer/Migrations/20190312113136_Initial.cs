using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Catalog.DataAccessLayer.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    CreationDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    Instance = table.Column<string>(maxLength: 20, nullable: false),
                    MemberClass = table.Column<string>(maxLength: 20, nullable: false),
                    MemberCode = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemberServices",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreationDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    ModificationDateTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    ServiceCode = table.Column<string>(maxLength: 100, nullable: false),
                    ServiceVersion = table.Column<string>(maxLength: 100, nullable: true),
                    Wsdl = table.Column<string>(type: "text", nullable: true),
                    MemberId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberServices_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SecurityServers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreationDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    ModificationDateTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    SecurityServerCode = table.Column<string>(maxLength: 100, nullable: false),
                    Address = table.Column<string>(maxLength: 500, nullable: false),
                    MemberId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityServers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecurityServers_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubSystems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreationDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    SubSystemCode = table.Column<string>(maxLength: 100, nullable: false),
                    MemberId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubSystems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubSystems_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubSystemServices",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreationDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    ModificationDateTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    ServiceCode = table.Column<string>(maxLength: 100, nullable: false),
                    ServiceVersion = table.Column<string>(maxLength: 100, nullable: true),
                    Wsdl = table.Column<string>(type: "text", nullable: true),
                    SubSystemId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubSystemServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubSystemServices_SubSystems_SubSystemId",
                        column: x => x.SubSystemId,
                        principalTable: "SubSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_Instance_MemberClass_MemberCode",
                table: "Members",
                columns: new[] { "Instance", "MemberClass", "MemberCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberServices_MemberId_ServiceCode_ServiceVersion",
                table: "MemberServices",
                columns: new[] { "MemberId", "ServiceCode", "ServiceVersion" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SecurityServers_MemberId_SecurityServerCode",
                table: "SecurityServers",
                columns: new[] { "MemberId", "SecurityServerCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubSystems_MemberId_SubSystemCode",
                table: "SubSystems",
                columns: new[] { "MemberId", "SubSystemCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubSystemServices_SubSystemId_ServiceCode_ServiceVersion",
                table: "SubSystemServices",
                columns: new[] { "SubSystemId", "ServiceCode", "ServiceVersion" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberServices");

            migrationBuilder.DropTable(
                name: "SecurityServers");

            migrationBuilder.DropTable(
                name: "SubSystemServices");

            migrationBuilder.DropTable(
                name: "SubSystems");

            migrationBuilder.DropTable(
                name: "Members");
        }
    }
}
