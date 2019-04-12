using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalog.DataAccessLayer.Migrations
{
    public partial class IsProcessedColumnAddedToOpRecords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "57113634-c0e1-4090-ba63-6c69a6c8ab88");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "af16314a-eda0-4fd1-b426-bfd33bbcdfaf");

            migrationBuilder.AddColumn<bool>(
                name: "IsProcessed",
                table: "OperationalDataRecords",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ConsumerSubSystemCode",
                table: "Messages",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a575744-a62f-4c6e-887e-2c3a684514b1", "3eacffa1-ee9a-4792-838b-0c526d3151cb", "Administrator", null },
                    { "2c3d0b37-4e7c-4210-8742-dc599ec8aeed", "e4100c06-a650-42ce-b918-4c97f6e55748", "CatalogUser", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a575744-a62f-4c6e-887e-2c3a684514b1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c3d0b37-4e7c-4210-8742-dc599ec8aeed");

            migrationBuilder.DropColumn(
                name: "IsProcessed",
                table: "OperationalDataRecords");

            migrationBuilder.AlterColumn<string>(
                name: "ConsumerSubSystemCode",
                table: "Messages",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "af16314a-eda0-4fd1-b426-bfd33bbcdfaf", "365ec4b0-b0e2-4a8f-b83e-c64ea5dd86d7", "Administrator", null },
                    { "57113634-c0e1-4090-ba63-6c69a6c8ab88", "602a71b9-443d-4d64-bae7-5336a971bdad", "CatalogUser", null }
                });
        }
    }
}
