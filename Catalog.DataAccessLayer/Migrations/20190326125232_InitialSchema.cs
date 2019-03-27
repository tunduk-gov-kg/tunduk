using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Catalog.DataAccessLayer.Migrations {
    public partial class InitialSchema : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                "AspNetRoles",
                table => new {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_AspNetRoles", x => x.Id); });

            migrationBuilder.CreateTable(
                "AspNetUsers",
                table => new {
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
                constraints: table => { table.PrimaryKey("PK_AspNetUsers", x => x.Id); });

            migrationBuilder.CreateTable(
                "ErrorLogs",
                table => new {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    Code = table.Column<string>(maxLength: 100, nullable: false),
                    Message = table.Column<string>("text", nullable: true),
                    StackTrace = table.Column<string>("text", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_ErrorLogs", x => x.Id); });

            migrationBuilder.CreateTable(
                "Members",
                table => new {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 500, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Instance = table.Column<string>(maxLength: 20, nullable: false),
                    MemberClass = table.Column<string>(maxLength: 20, nullable: false),
                    MemberCode = table.Column<string>(maxLength: 20, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Description = table.Column<string>("text", nullable: true),
                    Site = table.Column<string>(maxLength: 500, nullable: true),
                    Address = table.Column<string>(maxLength: 500, nullable: true),
                    MemberStatus = table.Column<string>(maxLength: 200, nullable: true),
                    MemberType = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Members", x => x.Id); });

            migrationBuilder.CreateTable(
                "AspNetRoleClaims",
                table => new {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        x => x.RoleId,
                        "AspNetRoles",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUserClaims",
                table => new {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        "FK_AspNetUserClaims_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUserLogins",
                table => new {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new {x.LoginProvider, x.ProviderKey});
                    table.ForeignKey(
                        "FK_AspNetUserLogins_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUserRoles",
                table => new {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new {x.UserId, x.RoleId});
                    table.ForeignKey(
                        "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        x => x.RoleId,
                        "AspNetRoles",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_AspNetUserRoles_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUserTokens",
                table => new {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new {x.UserId, x.LoginProvider, x.Name});
                    table.ForeignKey(
                        "FK_AspNetUserTokens_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "MemberRoleReferences",
                table => new {
                    MemberRole = table.Column<string>(maxLength: 200, nullable: false),
                    MemberId = table.Column<long>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_MemberRoleReferences", x => new {x.MemberRole, x.MemberId});
                    table.ForeignKey(
                        "FK_MemberRoleReferences_Members_MemberId",
                        x => x.MemberId,
                        "Members",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "SecurityServers",
                table => new {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SecurityServerCode = table.Column<string>(maxLength: 200, nullable: false),
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
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 500, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SubSystemCode = table.Column<string>(maxLength: 100, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>("text", nullable: true),
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
                "Services",
                table => new {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 500, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 500, nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ServiceCode = table.Column<string>(maxLength: 200, nullable: false),
                    ServiceVersion = table.Column<string>(maxLength: 20, nullable: true),
                    Wsdl = table.Column<string>("text", nullable: true),
                    SubSystemId = table.Column<long>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        "FK_Services_SubSystems_SubSystemId",
                        x => x.SubSystemId,
                        "SubSystems",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_AspNetRoleClaims_RoleId",
                "AspNetRoleClaims",
                "RoleId");

            migrationBuilder.CreateIndex(
                "RoleNameIndex",
                "AspNetRoles",
                "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_AspNetUserClaims_UserId",
                "AspNetUserClaims",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_AspNetUserLogins_UserId",
                "AspNetUserLogins",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_AspNetUserRoles_RoleId",
                "AspNetUserRoles",
                "RoleId");

            migrationBuilder.CreateIndex(
                "EmailIndex",
                "AspNetUsers",
                "NormalizedEmail");

            migrationBuilder.CreateIndex(
                "UserNameIndex",
                "AspNetUsers",
                "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_ErrorLogs_Code",
                "ErrorLogs",
                "Code");

            migrationBuilder.CreateIndex(
                "IX_MemberRoleReferences_MemberId",
                "MemberRoleReferences",
                "MemberId");

            migrationBuilder.CreateIndex(
                "IX_Members_CreatedBy",
                "Members",
                "CreatedBy");

            migrationBuilder.CreateIndex(
                "IX_Members_ModifiedBy",
                "Members",
                "ModifiedBy");

            migrationBuilder.CreateIndex(
                "IX_Members_Instance_MemberClass_MemberCode",
                "Members",
                new[] {"Instance", "MemberClass", "MemberCode"},
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_SecurityServers_MemberId_SecurityServerCode",
                "SecurityServers",
                new[] {"MemberId", "SecurityServerCode"},
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Services_CreatedBy",
                "Services",
                "CreatedBy");

            migrationBuilder.CreateIndex(
                "IX_Services_ModifiedBy",
                "Services",
                "ModifiedBy");

            migrationBuilder.CreateIndex(
                "IX_Services_SubSystemId_ServiceCode_ServiceVersion",
                "Services",
                new[] {"SubSystemId", "ServiceCode", "ServiceVersion"},
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_SubSystems_CreatedBy",
                "SubSystems",
                "CreatedBy");

            migrationBuilder.CreateIndex(
                "IX_SubSystems_ModifiedBy",
                "SubSystems",
                "ModifiedBy");

            migrationBuilder.CreateIndex(
                "IX_SubSystems_MemberId_SubSystemCode",
                "SubSystems",
                new[] {"MemberId", "SubSystemCode"},
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                "AspNetRoleClaims");

            migrationBuilder.DropTable(
                "AspNetUserClaims");

            migrationBuilder.DropTable(
                "AspNetUserLogins");

            migrationBuilder.DropTable(
                "AspNetUserRoles");

            migrationBuilder.DropTable(
                "AspNetUserTokens");

            migrationBuilder.DropTable(
                "ErrorLogs");

            migrationBuilder.DropTable(
                "MemberRoleReferences");

            migrationBuilder.DropTable(
                "SecurityServers");

            migrationBuilder.DropTable(
                "Services");

            migrationBuilder.DropTable(
                "AspNetRoles");

            migrationBuilder.DropTable(
                "AspNetUsers");

            migrationBuilder.DropTable(
                "SubSystems");

            migrationBuilder.DropTable(
                "Members");
        }
    }
}