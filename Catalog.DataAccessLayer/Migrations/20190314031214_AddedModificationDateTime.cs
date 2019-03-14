using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalog.DataAccessLayer.Migrations
{
    public partial class AddedModificationDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDateTime",
                table: "SubSystems",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDateTime",
                table: "Members",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModificationDateTime",
                table: "SubSystems");

            migrationBuilder.DropColumn(
                name: "ModificationDateTime",
                table: "Members");
        }
    }
}
