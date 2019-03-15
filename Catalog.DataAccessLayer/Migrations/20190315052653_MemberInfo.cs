using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Catalog.DataAccessLayer.Migrations
{
    public partial class MemberInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MemberRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemberStatuses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true)
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
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemberInfoRecords",
                columns: table => new
                {
                    MemberInfoId = table.Column<long>(nullable: false),
                    ModificationDateTime = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Site = table.Column<string>(nullable: true),
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
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_MemberInfoRoleReferences_MemberInfoId",
                table: "MemberInfoRoleReferences",
                column: "MemberInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberInfoRoleReferences_MemberRoleId",
                table: "MemberInfoRoleReferences",
                column: "MemberRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberInfoRoleReferences");

            migrationBuilder.DropTable(
                name: "MemberInfoRecords");

            migrationBuilder.DropTable(
                name: "MemberRoles");

            migrationBuilder.DropTable(
                name: "MemberStatuses");

            migrationBuilder.DropTable(
                name: "MemberTypes");
        }
    }
}
