using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalog.DataAccessLayer.Migrations
{
    public partial class MessageIdLength300 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                "AspNetRoles",
                "Id",
                "1a575744-a62f-4c6e-887e-2c3a684514b1");

            migrationBuilder.DeleteData(
                "AspNetRoles",
                "Id",
                "2c3d0b37-4e7c-4210-8742-dc599ec8aeed");

            migrationBuilder.AlterColumn<string>(
                "MessageId",
                "OperationalDataRecords",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.InsertData(
                "AspNetRoles",
                new[] {"Id", "ConcurrencyStamp", "Name", "NormalizedName"},
                new object[,]
                {
                    {
                        "5c1de318-a255-4470-b1e6-b760dbb14455", "53a7d482-b9c1-4713-a19a-c66fb32a8d15", "Administrator",
                        null
                    },
                    {
                        "cd03078a-4aa4-4aa4-a532-c9d50fbf8520", "4c93620d-657e-40b3-8ea4-2d443f735ee0", "CatalogUser",
                        null
                    }
                });

            migrationBuilder.CreateIndex(
                "IX_OperationalDataRecords_MessageId",
                "OperationalDataRecords",
                "MessageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                "IX_OperationalDataRecords_MessageId",
                "OperationalDataRecords");

            migrationBuilder.DeleteData(
                "AspNetRoles",
                "Id",
                "5c1de318-a255-4470-b1e6-b760dbb14455");

            migrationBuilder.DeleteData(
                "AspNetRoles",
                "Id",
                "cd03078a-4aa4-4aa4-a532-c9d50fbf8520");

            migrationBuilder.AlterColumn<string>(
                "MessageId",
                "OperationalDataRecords",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.InsertData(
                "AspNetRoles",
                new[] {"Id", "ConcurrencyStamp", "Name", "NormalizedName"},
                new object[,]
                {
                    {
                        "1a575744-a62f-4c6e-887e-2c3a684514b1", "3eacffa1-ee9a-4792-838b-0c526d3151cb", "Administrator",
                        null
                    },
                    {
                        "2c3d0b37-4e7c-4210-8742-dc599ec8aeed", "e4100c06-a650-42ce-b918-4c97f6e55748", "CatalogUser",
                        null
                    }
                });
        }
    }
}