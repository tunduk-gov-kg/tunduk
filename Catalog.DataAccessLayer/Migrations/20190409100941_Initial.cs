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
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DomainLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    LogLevel = table.Column<string>(maxLength: 100, nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 500, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 500, nullable: true),
                    Instance = table.Column<string>(maxLength: 20, nullable: false),
                    MemberClass = table.Column<string>(maxLength: 20, nullable: false),
                    MemberCode = table.Column<string>(maxLength: 20, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Site = table.Column<string>(maxLength: 500, nullable: true),
                    Address = table.Column<string>(maxLength: 500, nullable: true),
                    MemberStatus = table.Column<string>(maxLength: 200, nullable: true),
                    MemberType = table.Column<string>(maxLength: 200, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    MessageId = table.Column<string>(maxLength: 500, nullable: false),
                    MessageDigest = table.Column<string>(maxLength: 500, nullable: false),
                    MessageProtocolVersion = table.Column<string>(maxLength: 10, nullable: false),
                    MessageIssue = table.Column<string>(maxLength: 500, nullable: true),
                    MessageUserId = table.Column<string>(maxLength: 500, nullable: true),
                    MessageState = table.Column<string>(nullable: false),
                    ConsumerInstance = table.Column<string>(maxLength: 20, nullable: false),
                    ConsumerMemberClass = table.Column<string>(maxLength: 20, nullable: false),
                    ConsumerMemberCode = table.Column<string>(maxLength: 100, nullable: false),
                    ConsumerSubSystemCode = table.Column<string>(maxLength: 100, nullable: false),
                    ProducerInstance = table.Column<string>(maxLength: 20, nullable: false),
                    ProducerMemberClass = table.Column<string>(maxLength: 20, nullable: false),
                    ProducerMemberCode = table.Column<string>(maxLength: 100, nullable: false),
                    ProducerSubSystemCode = table.Column<string>(maxLength: 100, nullable: true),
                    ProducerServiceCode = table.Column<string>(maxLength: 100, nullable: false),
                    ProducerServiceVersion = table.Column<string>(maxLength: 20, nullable: true),
                    ConsumerSecurityServerInternalIpAddress = table.Column<string>(maxLength: 200, nullable: true),
                    ConsumerSecurityServerAddress = table.Column<string>(maxLength: 200, nullable: true),
                    ProducerSecurityServerInternalIpAddress = table.Column<string>(maxLength: 200, nullable: true),
                    ProducerSecurityServerAddress = table.Column<string>(maxLength: 200, nullable: true),
                    ConsumerRequestInTs = table.Column<DateTime>(nullable: true),
                    ConsumerRequestOutTs = table.Column<DateTime>(nullable: true),
                    ConsumerResponseInTs = table.Column<DateTime>(nullable: true),
                    ConsumerResponseOutTs = table.Column<DateTime>(name: "ConsumerResponseOutTs`", nullable: true),
                    ProducerRequestInTs = table.Column<DateTime>(nullable: true),
                    ProducerRequestOutTs = table.Column<DateTime>(nullable: true),
                    ProducerResponseInTs = table.Column<DateTime>(nullable: true),
                    ProducerResponseOutTs = table.Column<DateTime>(name: "ProducerResponseOutTs`", nullable: true),
                    RequestAttachmentsCount = table.Column<int>(nullable: true),
                    RequestSoapSize = table.Column<int>(nullable: true),
                    ResponseAttachmentsCount = table.Column<int>(nullable: true),
                    ResponseSoapSize = table.Column<int>(nullable: true),
                    IsSucceeded = table.Column<bool>(nullable: false),
                    FaultCode = table.Column<string>(maxLength: 500, nullable: true),
                    FaultString = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperationalDataRecords",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    ClientXRoadInstance = table.Column<string>(maxLength: 50, nullable: true),
                    ClientMemberClass = table.Column<string>(maxLength: 50, nullable: true),
                    ClientMemberCode = table.Column<string>(maxLength: 50, nullable: true),
                    ClientSecurityServerAddress = table.Column<string>(maxLength: 200, nullable: true),
                    ClientSubsystemCode = table.Column<string>(maxLength: 100, nullable: true),
                    ServiceXRoadInstance = table.Column<string>(maxLength: 50, nullable: true),
                    ServiceMemberClass = table.Column<string>(maxLength: 50, nullable: true),
                    ServiceMemberCode = table.Column<string>(maxLength: 50, nullable: true),
                    ServiceSubsystemCode = table.Column<string>(maxLength: 50, nullable: true),
                    ServiceCode = table.Column<string>(maxLength: 100, nullable: true),
                    ServiceVersion = table.Column<string>(maxLength: 50, nullable: true),
                    ServiceSecurityServerAddress = table.Column<string>(maxLength: 200, nullable: true),
                    MessageId = table.Column<string>(maxLength: 100, nullable: true),
                    MessageIssue = table.Column<string>(maxLength: 500, nullable: true),
                    MessageProtocolVersion = table.Column<string>(maxLength: 20, nullable: true),
                    MessageUserId = table.Column<string>(maxLength: 100, nullable: true),
                    MonitoringDataTs = table.Column<long>(nullable: true),
                    RepresentedPartyClass = table.Column<string>(maxLength: 50, nullable: true),
                    RepresentedPartyCode = table.Column<string>(maxLength: 50, nullable: true),
                    RequestAttachmentCount = table.Column<int>(nullable: true),
                    RequestInTs = table.Column<long>(nullable: true),
                    RequestOutTs = table.Column<long>(nullable: true),
                    RequestSoapSize = table.Column<int>(nullable: true),
                    RequestMimeSize = table.Column<int>(nullable: true),
                    ResponseAttachmentCount = table.Column<int>(nullable: true),
                    ResponseInTs = table.Column<long>(nullable: true),
                    ResponseOutTs = table.Column<long>(nullable: true),
                    ResponseSoapSize = table.Column<int>(nullable: true),
                    ResponseMimeSize = table.Column<int>(nullable: true),
                    SecurityServerInternalIp = table.Column<string>(nullable: true),
                    SecurityServerType = table.Column<string>(nullable: true),
                    Succeeded = table.Column<bool>(nullable: true),
                    SoapFaultCode = table.Column<string>(nullable: true),
                    SoapFaultString = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationalDataRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberRoleReferences",
                columns: table => new
                {
                    MemberRole = table.Column<string>(maxLength: 200, nullable: false),
                    MemberId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberRoleReferences", x => new { x.MemberRole, x.MemberId });
                    table.ForeignKey(
                        name: "FK_MemberRoleReferences_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecurityServers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    SecurityServerCode = table.Column<string>(maxLength: 200, nullable: false),
                    Address = table.Column<string>(maxLength: 500, nullable: false),
                    MemberId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
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
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 500, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 500, nullable: true),
                    SubSystemCode = table.Column<string>(maxLength: 100, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    MemberId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
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
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 500, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 500, nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ServiceCode = table.Column<string>(maxLength: 200, nullable: false),
                    ServiceVersion = table.Column<string>(maxLength: 20, nullable: true),
                    Wsdl = table.Column<string>(type: "text", nullable: true),
                    SubSystemId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_SubSystems_SubSystemId",
                        column: x => x.SubSystemId,
                        principalTable: "SubSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "942d9f5f-5d07-4922-97e5-528e73c72e4c", "efc76605-9107-4c61-97a7-69749cc9bd8a", "Administrator", null },
                    { "83e39e46-534a-4590-8ec4-183070b0c68c", "2e77da4d-0183-4212-93ae-3e0eb3c6dd68", "CatalogUser", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DomainLogs_LogLevel",
                table: "DomainLogs",
                column: "LogLevel");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRoleReferences_MemberId",
                table: "MemberRoleReferences",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_CreatedBy",
                table: "Members",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Members_ModifiedBy",
                table: "Members",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Members_Instance_MemberClass_MemberCode",
                table: "Members",
                columns: new[] { "Instance", "MemberClass", "MemberCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MessageDigest",
                table: "Messages",
                column: "MessageDigest",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MessageId",
                table: "Messages",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityServers_MemberId_SecurityServerCode",
                table: "SecurityServers",
                columns: new[] { "MemberId", "SecurityServerCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_CreatedBy",
                table: "Services",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ModifiedBy",
                table: "Services",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Services_SubSystemId_ServiceCode_ServiceVersion",
                table: "Services",
                columns: new[] { "SubSystemId", "ServiceCode", "ServiceVersion" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubSystems_CreatedBy",
                table: "SubSystems",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SubSystems_ModifiedBy",
                table: "SubSystems",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SubSystems_MemberId_SubSystemCode",
                table: "SubSystems",
                columns: new[] { "MemberId", "SubSystemCode" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DomainLogs");

            migrationBuilder.DropTable(
                name: "MemberRoleReferences");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "OperationalDataRecords");

            migrationBuilder.DropTable(
                name: "SecurityServers");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SubSystems");

            migrationBuilder.DropTable(
                name: "Members");
        }
    }
}
