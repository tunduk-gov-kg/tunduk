using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Catalog.DataAccessLayer.Migrations
{
    public partial class RemovedDomainLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DomainLogs");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "843f7734-496f-4404-aca3-7a221be65aed");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "da11715f-1d5c-4aeb-ab3d-b07c1a8cf5e7");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "af16314a-eda0-4fd1-b426-bfd33bbcdfaf", "365ec4b0-b0e2-4a8f-b83e-c64ea5dd86d7", "Administrator", null },
                    { "57113634-c0e1-4090-ba63-6c69a6c8ab88", "602a71b9-443d-4d64-bae7-5336a971bdad", "CatalogUser", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "57113634-c0e1-4090-ba63-6c69a6c8ab88");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "af16314a-eda0-4fd1-b426-bfd33bbcdfaf");

            migrationBuilder.CreateTable(
                name: "DomainLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    LogLevel = table.Column<string>(maxLength: 100, nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainLogs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "da11715f-1d5c-4aeb-ab3d-b07c1a8cf5e7", "1d4a256e-58f3-4369-a765-9c54efde5db4", "Administrator", null },
                    { "843f7734-496f-4404-aca3-7a221be65aed", "3e6592da-f1a9-441a-bfb5-d1f414d2b36e", "CatalogUser", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DomainLogs_LogLevel",
                table: "DomainLogs",
                column: "LogLevel");
        }
    }
}
