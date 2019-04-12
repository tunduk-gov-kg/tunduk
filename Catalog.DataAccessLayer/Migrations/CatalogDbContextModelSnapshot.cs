﻿// <auto-generated />
using System;
using Catalog.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Catalog.DataAccessLayer.Migrations
{
    [DbContext(typeof(CatalogDbContext))]
    partial class CatalogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Catalog.Domain.Entity.CatalogUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Catalog.Domain.Entity.Member", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .HasMaxLength(500);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(500);

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Instance")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("MemberClass")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("MemberCode")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("MemberStatus")
                        .HasMaxLength(200);

                    b.Property<string>("MemberType")
                        .HasMaxLength(200);

                    b.Property<DateTime?>("ModifiedAt");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("Site")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.HasIndex("Instance", "MemberClass", "MemberCode")
                        .IsUnique();

                    b.ToTable("Members");
                });

            modelBuilder.Entity("Catalog.Domain.Entity.MemberRoleReference", b =>
                {
                    b.Property<string>("MemberRole")
                        .HasMaxLength(200);

                    b.Property<long>("MemberId");

                    b.HasKey("MemberRole", "MemberId");

                    b.HasIndex("MemberId");

                    b.ToTable("MemberRoleReferences");
                });

            modelBuilder.Entity("Catalog.Domain.Entity.Message", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConsumerInstance")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("ConsumerMemberClass")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("ConsumerMemberCode")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("ConsumerSecurityServerAddress")
                        .HasMaxLength(200);

                    b.Property<string>("ConsumerSecurityServerInternalIpAddress")
                        .HasMaxLength(200);

                    b.Property<string>("ConsumerSubSystemCode")
                        .HasMaxLength(100);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("FaultCode")
                        .HasMaxLength(500);

                    b.Property<string>("FaultString")
                        .HasMaxLength(1000);

                    b.Property<bool>("IsSucceeded");

                    b.Property<string>("MessageDigest")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<string>("MessageId")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<string>("MessageIssue")
                        .HasMaxLength(500);

                    b.Property<string>("MessageProtocolVersion")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<string>("MessageState")
                        .IsRequired();

                    b.Property<string>("MessageUserId")
                        .HasMaxLength(500);

                    b.Property<DateTime?>("ModifiedAt");

                    b.Property<string>("ProducerInstance")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("ProducerMemberClass")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("ProducerMemberCode")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("ProducerSecurityServerAddress")
                        .HasMaxLength(200);

                    b.Property<string>("ProducerSecurityServerInternalIpAddress")
                        .HasMaxLength(200);

                    b.Property<string>("ProducerServiceCode")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("ProducerServiceVersion")
                        .HasMaxLength(20);

                    b.Property<string>("ProducerSubSystemCode")
                        .HasMaxLength(100);

                    b.Property<int?>("RequestAttachmentsCount");

                    b.Property<int?>("RequestSoapSize");

                    b.Property<int?>("ResponseAttachmentsCount");

                    b.Property<int?>("ResponseSoapSize");

                    b.HasKey("Id");

                    b.HasIndex("MessageDigest")
                        .IsUnique();

                    b.HasIndex("MessageId");

                    b.HasIndex("MessageState");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Catalog.Domain.Entity.OperationalDataRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClientMemberClass")
                        .HasMaxLength(50);

                    b.Property<string>("ClientMemberCode")
                        .HasMaxLength(50);

                    b.Property<string>("ClientSecurityServerAddress")
                        .HasMaxLength(200);

                    b.Property<string>("ClientSubsystemCode")
                        .HasMaxLength(100);

                    b.Property<string>("ClientXRoadInstance")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<bool>("IsProcessed");

                    b.Property<string>("MessageId")
                        .HasMaxLength(100);

                    b.Property<string>("MessageIssue")
                        .HasMaxLength(500);

                    b.Property<string>("MessageProtocolVersion")
                        .HasMaxLength(20);

                    b.Property<string>("MessageUserId")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("ModifiedAt");

                    b.Property<long?>("MonitoringDataTs");

                    b.Property<string>("RepresentedPartyClass")
                        .HasMaxLength(50);

                    b.Property<string>("RepresentedPartyCode")
                        .HasMaxLength(50);

                    b.Property<int?>("RequestAttachmentCount");

                    b.Property<long?>("RequestInTs");

                    b.Property<int?>("RequestMimeSize");

                    b.Property<long?>("RequestOutTs");

                    b.Property<int?>("RequestSoapSize");

                    b.Property<int?>("ResponseAttachmentCount");

                    b.Property<long?>("ResponseInTs");

                    b.Property<int?>("ResponseMimeSize");

                    b.Property<long?>("ResponseOutTs");

                    b.Property<int?>("ResponseSoapSize");

                    b.Property<string>("SecurityServerInternalIp");

                    b.Property<string>("SecurityServerType");

                    b.Property<string>("ServiceCode")
                        .HasMaxLength(100);

                    b.Property<string>("ServiceMemberClass")
                        .HasMaxLength(50);

                    b.Property<string>("ServiceMemberCode")
                        .HasMaxLength(50);

                    b.Property<string>("ServiceSecurityServerAddress")
                        .HasMaxLength(200);

                    b.Property<string>("ServiceSubsystemCode")
                        .HasMaxLength(50);

                    b.Property<string>("ServiceVersion")
                        .HasMaxLength(50);

                    b.Property<string>("ServiceXRoadInstance")
                        .HasMaxLength(50);

                    b.Property<string>("SoapFaultCode");

                    b.Property<string>("SoapFaultString");

                    b.Property<bool?>("Succeeded");

                    b.HasKey("Id");

                    b.ToTable("OperationalDataRecords");
                });

            modelBuilder.Entity("Catalog.Domain.Entity.SecurityServer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastRequestedDateTime");

                    b.Property<long>("MemberId");

                    b.Property<DateTime?>("ModifiedAt");

                    b.Property<string>("SecurityServerCode")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("MemberId", "SecurityServerCode")
                        .IsUnique();

                    b.ToTable("SecurityServers");
                });

            modelBuilder.Entity("Catalog.Domain.Entity.Service", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(500);

                    b.Property<string>("Description");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedAt");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(500);

                    b.Property<string>("Name");

                    b.Property<string>("ServiceCode")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("ServiceVersion")
                        .HasMaxLength(20);

                    b.Property<long>("SubSystemId");

                    b.Property<string>("Wsdl")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.HasIndex("SubSystemId", "ServiceCode", "ServiceVersion")
                        .IsUnique();

                    b.ToTable("Services");
                });

            modelBuilder.Entity("Catalog.Domain.Entity.SubSystem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(500);

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted");

                    b.Property<long>("MemberId");

                    b.Property<DateTime?>("ModifiedAt");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .HasMaxLength(200);

                    b.Property<string>("SubSystemCode")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.HasIndex("MemberId", "SubSystemCode")
                        .IsUnique();

                    b.ToTable("SubSystems");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");

                    b.HasData(
                        new
                        {
                            Id = "1a575744-a62f-4c6e-887e-2c3a684514b1",
                            ConcurrencyStamp = "3eacffa1-ee9a-4792-838b-0c526d3151cb",
                            Name = "Administrator"
                        },
                        new
                        {
                            Id = "2c3d0b37-4e7c-4210-8742-dc599ec8aeed",
                            ConcurrencyStamp = "e4100c06-a650-42ce-b918-4c97f6e55748",
                            Name = "CatalogUser"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Catalog.Domain.Entity.MemberRoleReference", b =>
                {
                    b.HasOne("Catalog.Domain.Entity.Member", "Member")
                        .WithMany("MemberRoles")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Catalog.Domain.Entity.Message", b =>
                {
                    b.OwnsOne("Catalog.Domain.Entity.MessageLifecycle", "ConsumerMessageLifecycle", b1 =>
                        {
                            b1.Property<long>("MessageId");

                            b1.Property<DateTime?>("RequestInTs")
                                .HasColumnName("ConsumerRequestInTs");

                            b1.Property<DateTime?>("RequestOutTs")
                                .HasColumnName("ConsumerRequestOutTs");

                            b1.Property<DateTime?>("ResponseInTs")
                                .HasColumnName("ConsumerResponseInTs");

                            b1.Property<DateTime?>("ResponseOutTs")
                                .HasColumnName("ConsumerResponseOutTs`");

                            b1.HasKey("MessageId");

                            b1.ToTable("Messages");

                            b1.HasOne("Catalog.Domain.Entity.Message")
                                .WithOne("ConsumerMessageLifecycle")
                                .HasForeignKey("Catalog.Domain.Entity.MessageLifecycle", "MessageId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("Catalog.Domain.Entity.MessageLifecycle", "ProducerMessageLifecycle", b1 =>
                        {
                            b1.Property<long>("MessageId");

                            b1.Property<DateTime?>("RequestInTs")
                                .HasColumnName("ProducerRequestInTs");

                            b1.Property<DateTime?>("RequestOutTs")
                                .HasColumnName("ProducerRequestOutTs");

                            b1.Property<DateTime?>("ResponseInTs")
                                .HasColumnName("ProducerResponseInTs");

                            b1.Property<DateTime?>("ResponseOutTs")
                                .HasColumnName("ProducerResponseOutTs`");

                            b1.HasKey("MessageId");

                            b1.ToTable("Messages");

                            b1.HasOne("Catalog.Domain.Entity.Message")
                                .WithOne("ProducerMessageLifecycle")
                                .HasForeignKey("Catalog.Domain.Entity.MessageLifecycle", "MessageId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("Catalog.Domain.Entity.SecurityServer", b =>
                {
                    b.HasOne("Catalog.Domain.Entity.Member", "Member")
                        .WithMany("SecurityServers")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Catalog.Domain.Entity.Service", b =>
                {
                    b.HasOne("Catalog.Domain.Entity.SubSystem", "SubSystem")
                        .WithMany("Services")
                        .HasForeignKey("SubSystemId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Catalog.Domain.Entity.SubSystem", b =>
                {
                    b.HasOne("Catalog.Domain.Entity.Member", "Member")
                        .WithMany("SubSystems")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Catalog.Domain.Entity.CatalogUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Catalog.Domain.Entity.CatalogUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Catalog.Domain.Entity.CatalogUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Catalog.Domain.Entity.CatalogUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
