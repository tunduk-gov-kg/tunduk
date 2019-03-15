using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Catalog.DataAccessLayer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MemberRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    CreationDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    ModificationDateTime = table.Column<DateTime>(nullable: true),
                    Instance = table.Column<string>(maxLength: 20, nullable: false),
                    MemberClass = table.Column<string>(maxLength: 20, nullable: false),
                    MemberCode = table.Column<string>(maxLength: 20, nullable: false),
                    Name = table.Column<string>(maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemberStatuses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemberTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberTypes", x => x.Id);
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
                    ModificationDateTime = table.Column<DateTime>(nullable: true),
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
                name: "MemberInfoRecords",
                columns: table => new
                {
                    MemberInfoId = table.Column<long>(nullable: false),
                    ModificationDateTime = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Site = table.Column<string>(maxLength: 100, nullable: true),
                    MemberStatusId = table.Column<long>(nullable: true),
                    MemberTypeId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberInfoRecords", x => x.MemberInfoId);
                    table.ForeignKey(
                        name: "FK_MemberInfoRecords_Members_MemberInfoId",
                        column: x => x.MemberInfoId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberInfoRecords_MemberStatuses_MemberStatusId",
                        column: x => x.MemberStatusId,
                        principalTable: "MemberStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberInfoRecords_MemberTypes_MemberTypeId",
                        column: x => x.MemberTypeId,
                        principalTable: "MemberTypes",
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

            migrationBuilder.CreateTable(
                name: "MemberInfoRoleReferences",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    MemberInfoId = table.Column<long>(nullable: false),
                    MemberRoleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberInfoRoleReferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberInfoRoleReferences_MemberInfoRecords_MemberInfoId",
                        column: x => x.MemberInfoId,
                        principalTable: "MemberInfoRecords",
                        principalColumn: "MemberInfoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberInfoRoleReferences_MemberRoles_MemberRoleId",
                        column: x => x.MemberRoleId,
                        principalTable: "MemberRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberInfoRecords_MemberStatusId",
                table: "MemberInfoRecords",
                column: "MemberStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberInfoRecords_MemberTypeId",
                table: "MemberInfoRecords",
                column: "MemberTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberInfoRoleReferences_MemberRoleId",
                table: "MemberInfoRoleReferences",
                column: "MemberRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberInfoRoleReferences_MemberInfoId_MemberRoleId",
                table: "MemberInfoRoleReferences",
                columns: new[] { "MemberInfoId", "MemberRoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberRoles_Name",
                table: "MemberRoles",
                column: "Name",
                unique: true);

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
                name: "IX_MemberStatuses_Name",
                table: "MemberStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberTypes_Name",
                table: "MemberTypes",
                column: "Name",
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
                name: "MemberInfoRoleReferences");

            migrationBuilder.DropTable(
                name: "MemberServices");

            migrationBuilder.DropTable(
                name: "SecurityServers");

            migrationBuilder.DropTable(
                name: "SubSystemServices");

            migrationBuilder.DropTable(
                name: "MemberInfoRecords");

            migrationBuilder.DropTable(
                name: "MemberRoles");

            migrationBuilder.DropTable(
                name: "SubSystems");

            migrationBuilder.DropTable(
                name: "MemberStatuses");

            migrationBuilder.DropTable(
                name: "MemberTypes");

            migrationBuilder.DropTable(
                name: "Members");
        }
    }
}
