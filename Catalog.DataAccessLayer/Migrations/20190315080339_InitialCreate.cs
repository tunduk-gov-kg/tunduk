using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Catalog.DataAccessLayer.Migrations {
    public partial class InitialCreate : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                "MemberRoles",
                table => new {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_MemberRoles", x => x.Id); });

            migrationBuilder.CreateTable(
                "Members",
                table => new {
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
                constraints: table => { table.PrimaryKey("PK_Members", x => x.Id); });

            migrationBuilder.CreateTable(
                "MemberStatuses",
                table => new {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_MemberStatuses", x => x.Id); });

            migrationBuilder.CreateTable(
                "MemberTypes",
                table => new {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_MemberTypes", x => x.Id); });

            migrationBuilder.CreateTable(
                "MemberServices",
                table => new {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreationDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    ModificationDateTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    ServiceCode = table.Column<string>(maxLength: 100, nullable: false),
                    ServiceVersion = table.Column<string>(maxLength: 100, nullable: true),
                    Wsdl = table.Column<string>("text", nullable: true),
                    MemberId = table.Column<long>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_MemberServices", x => x.Id);
                    table.ForeignKey(
                        "FK_MemberServices_Members_MemberId",
                        x => x.MemberId,
                        "Members",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "SecurityServers",
                table => new {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreationDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    ModificationDateTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    SecurityServerCode = table.Column<string>(maxLength: 100, nullable: false),
                    Address = table.Column<string>(maxLength: 500, nullable: false),
                    MemberId = table.Column<long>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_SecurityServers", x => x.Id);
                    table.ForeignKey(
                        "FK_SecurityServers_Members_MemberId",
                        x => x.MemberId,
                        "Members",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "SubSystems",
                table => new {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreationDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    ModificationDateTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    SubSystemCode = table.Column<string>(maxLength: 100, nullable: false),
                    MemberId = table.Column<long>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_SubSystems", x => x.Id);
                    table.ForeignKey(
                        "FK_SubSystems_Members_MemberId",
                        x => x.MemberId,
                        "Members",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "MemberInfoRecords",
                table => new {
                    MemberInfoId = table.Column<long>(nullable: false),
                    ModificationDateTime = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>("text", nullable: true),
                    Site = table.Column<string>(maxLength: 100, nullable: true),
                    MemberStatusId = table.Column<long>(nullable: true),
                    MemberTypeId = table.Column<long>(nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_MemberInfoRecords", x => x.MemberInfoId);
                    table.ForeignKey(
                        "FK_MemberInfoRecords_Members_MemberInfoId",
                        x => x.MemberInfoId,
                        "Members",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_MemberInfoRecords_MemberStatuses_MemberStatusId",
                        x => x.MemberStatusId,
                        "MemberStatuses",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_MemberInfoRecords_MemberTypes_MemberTypeId",
                        x => x.MemberTypeId,
                        "MemberTypes",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "SubSystemServices",
                table => new {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreationDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    ModificationDateTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    ServiceCode = table.Column<string>(maxLength: 100, nullable: false),
                    ServiceVersion = table.Column<string>(maxLength: 100, nullable: true),
                    Wsdl = table.Column<string>("text", nullable: true),
                    SubSystemId = table.Column<long>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_SubSystemServices", x => x.Id);
                    table.ForeignKey(
                        "FK_SubSystemServices_SubSystems_SubSystemId",
                        x => x.SubSystemId,
                        "SubSystems",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "MemberInfoRoleReferences",
                table => new {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    MemberInfoId = table.Column<long>(nullable: false),
                    MemberRoleId = table.Column<long>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_MemberInfoRoleReferences", x => x.Id);
                    table.ForeignKey(
                        "FK_MemberInfoRoleReferences_MemberInfoRecords_MemberInfoId",
                        x => x.MemberInfoId,
                        "MemberInfoRecords",
                        "MemberInfoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_MemberInfoRoleReferences_MemberRoles_MemberRoleId",
                        x => x.MemberRoleId,
                        "MemberRoles",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_MemberInfoRecords_MemberStatusId",
                "MemberInfoRecords",
                "MemberStatusId");

            migrationBuilder.CreateIndex(
                "IX_MemberInfoRecords_MemberTypeId",
                "MemberInfoRecords",
                "MemberTypeId");

            migrationBuilder.CreateIndex(
                "IX_MemberInfoRoleReferences_MemberRoleId",
                "MemberInfoRoleReferences",
                "MemberRoleId");

            migrationBuilder.CreateIndex(
                "IX_MemberInfoRoleReferences_MemberInfoId_MemberRoleId",
                "MemberInfoRoleReferences",
                new[] {"MemberInfoId", "MemberRoleId"},
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_MemberRoles_Name",
                "MemberRoles",
                "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Members_Instance_MemberClass_MemberCode",
                "Members",
                new[] {"Instance", "MemberClass", "MemberCode"},
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_MemberServices_MemberId_ServiceCode_ServiceVersion",
                "MemberServices",
                new[] {"MemberId", "ServiceCode", "ServiceVersion"},
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_MemberStatuses_Name",
                "MemberStatuses",
                "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_MemberTypes_Name",
                "MemberTypes",
                "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_SecurityServers_MemberId_SecurityServerCode",
                "SecurityServers",
                new[] {"MemberId", "SecurityServerCode"},
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_SubSystems_MemberId_SubSystemCode",
                "SubSystems",
                new[] {"MemberId", "SubSystemCode"},
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_SubSystemServices_SubSystemId_ServiceCode_ServiceVersion",
                "SubSystemServices",
                new[] {"SubSystemId", "ServiceCode", "ServiceVersion"},
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                "MemberInfoRoleReferences");

            migrationBuilder.DropTable(
                "MemberServices");

            migrationBuilder.DropTable(
                "SecurityServers");

            migrationBuilder.DropTable(
                "SubSystemServices");

            migrationBuilder.DropTable(
                "MemberInfoRecords");

            migrationBuilder.DropTable(
                "MemberRoles");

            migrationBuilder.DropTable(
                "SubSystems");

            migrationBuilder.DropTable(
                "MemberStatuses");

            migrationBuilder.DropTable(
                "MemberTypes");

            migrationBuilder.DropTable(
                "Members");
        }
    }
}