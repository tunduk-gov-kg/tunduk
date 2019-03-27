using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Catalog.DataAccessLayer.Migrations {
    public partial class RemovedErrorLog : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                "ErrorLogs");

            migrationBuilder.CreateTable(
                "DomainLogs",
                table => new {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    LogLevel = table.Column<string>(maxLength: 100, nullable: false),
                    Message = table.Column<string>("text", nullable: true),
                    Description = table.Column<string>("text", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_DomainLogs", x => x.Id); });

            migrationBuilder.CreateIndex(
                "IX_DomainLogs_LogLevel",
                "DomainLogs",
                "LogLevel");
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                "DomainLogs");

            migrationBuilder.CreateTable(
                "ErrorLogs",
                table => new {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Code = table.Column<string>(maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Message = table.Column<string>("text", nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    StackTrace = table.Column<string>("text", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_ErrorLogs", x => x.Id); });

            migrationBuilder.CreateIndex(
                "IX_ErrorLogs_Code",
                "ErrorLogs",
                "Code");
        }
    }
}