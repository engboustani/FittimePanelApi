﻿// <auto-generated />
using System;
using FittimePanelApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FittimePanelApi.Migrations
{
    [DbContext(typeof(AppDb))]
    [Migration("20220221095244_paymentDiscount")]
    partial class paymentDiscount
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.10");

            modelBuilder.Entity("FittimePanelApi.Data.Exercise", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ExerciseTypeId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserInstructorId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UserStudentId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseTypeId");

                    b.HasIndex("UserInstructorId");

                    b.HasIndex("UserStudentId");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("FittimePanelApi.Data.ExerciseBlob", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<Guid?>("ExerciseId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Key")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<byte[]>("Value")
                        .HasColumnType("MediumBlob");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("UserId");

                    b.ToTable("ExerciseBlobs");
                });

            modelBuilder.Entity("FittimePanelApi.Data.ExerciseDownload", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<Guid>("ExerciseId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Uri")
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Value")
                        .HasColumnType("MediumBlob");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.ToTable("ExerciseDownloads");
                });

            modelBuilder.Entity("FittimePanelApi.Data.ExerciseMeta", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<Guid?>("ExerciseId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Key")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("UserId");

                    b.ToTable("ExerciseMetas");
                });

            modelBuilder.Entity("FittimePanelApi.Data.ExerciseType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<uint>("Price")
                        .HasColumnType("int unsigned");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("ExerciseTypes");
                });

            modelBuilder.Entity("FittimePanelApi.Data.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<double>("Amount")
                        .HasColumnType("double");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("DiscountId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ExerciseId")
                        .HasColumnType("char(36)");

                    b.Property<int>("PaymentGetwayId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("DiscountId");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("PaymentGetwayId");

                    b.HasIndex("UserId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("FittimePanelApi.Data.PaymentDiscount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Code")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<double>("Discount")
                        .HasColumnType("double");

                    b.Property<int>("DiscountType")
                        .HasColumnType("int");

                    b.Property<bool>("Enabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("ExpireDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Limit")
                        .HasColumnType("int");

                    b.Property<bool>("Limited")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("Percentage")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("PaymentDiscount");
                });

            modelBuilder.Entity("FittimePanelApi.Data.PaymentGetaway", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("Enabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("NameFa")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("PaymentGetways");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Enabled = true,
                            Name = "payir",
                            UpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2,
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Enabled = true,
                            Name = "idpay",
                            UpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 3,
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Enabled = true,
                            Name = "zarinpal",
                            UpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("FittimePanelApi.Data.Ticket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserAnsweredId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UserCreatedId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserAnsweredId");

                    b.HasIndex("UserCreatedId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("FittimePanelApi.Data.TicketMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Text")
                        .HasColumnType("longtext");

                    b.Property<Guid?>("TicketId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("TicketId");

                    b.HasIndex("UserId");

                    b.ToTable("TicketMessages");
                });

            modelBuilder.Entity("FittimePanelApi.Data.TicketStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("longtext");

                    b.Property<Guid?>("TicketId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("TicketId");

                    b.ToTable("TicketStatus");
                });

            modelBuilder.Entity("FittimePanelApi.Data.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastLoginTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("FittimePanelApi.Data.UserBlob", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Key")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<byte[]>("Value")
                        .HasColumnType("MediumBlob");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserBlobs");
                });

            modelBuilder.Entity("FittimePanelApi.Data.UserMeta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Key")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserMetas");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");

                    b.HasData(
                        new
                        {
                            Id = "6e947082-d76d-4b8b-90f3-16e6e60c6616",
                            ConcurrencyStamp = "5f39f79b-84bb-4013-b4bb-0664d084ca6d",
                            Name = "User",
                            NormalizedName = "USER"
                        },
                        new
                        {
                            Id = "c87ee1ab-58bc-4eb4-8948-e35e68a3d16b",
                            ConcurrencyStamp = "fa764b63-d437-4397-80c5-d2a480cd5b15",
                            Name = "Administrator",
                            NormalizedName = "ADMINISTRATOR"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("FittimePanelApi.Data.Exercise", b =>
                {
                    b.HasOne("FittimePanelApi.Data.ExerciseType", "ExerciseType")
                        .WithMany("Exercises")
                        .HasForeignKey("ExerciseTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FittimePanelApi.Data.User", "UserInstructor")
                        .WithMany()
                        .HasForeignKey("UserInstructorId");

                    b.HasOne("FittimePanelApi.Data.User", "UserStudent")
                        .WithMany()
                        .HasForeignKey("UserStudentId");

                    b.Navigation("ExerciseType");

                    b.Navigation("UserInstructor");

                    b.Navigation("UserStudent");
                });

            modelBuilder.Entity("FittimePanelApi.Data.ExerciseBlob", b =>
                {
                    b.HasOne("FittimePanelApi.Data.Exercise", "Exercise")
                        .WithMany("ExerciseBlobs")
                        .HasForeignKey("ExerciseId");

                    b.HasOne("FittimePanelApi.Data.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Exercise");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FittimePanelApi.Data.ExerciseDownload", b =>
                {
                    b.HasOne("FittimePanelApi.Data.Exercise", "Exercise")
                        .WithMany("ExerciseDownloads")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");
                });

            modelBuilder.Entity("FittimePanelApi.Data.ExerciseMeta", b =>
                {
                    b.HasOne("FittimePanelApi.Data.Exercise", "Exercise")
                        .WithMany("ExerciseMetas")
                        .HasForeignKey("ExerciseId");

                    b.HasOne("FittimePanelApi.Data.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Exercise");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FittimePanelApi.Data.Payment", b =>
                {
                    b.HasOne("FittimePanelApi.Data.PaymentDiscount", "Discount")
                        .WithMany("Payments")
                        .HasForeignKey("DiscountId");

                    b.HasOne("FittimePanelApi.Data.Exercise", "Exercise")
                        .WithMany("Payments")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FittimePanelApi.Data.PaymentGetaway", "PaymentGetway")
                        .WithMany("Payments")
                        .HasForeignKey("PaymentGetwayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FittimePanelApi.Data.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Discount");

                    b.Navigation("Exercise");

                    b.Navigation("PaymentGetway");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FittimePanelApi.Data.Ticket", b =>
                {
                    b.HasOne("FittimePanelApi.Data.User", "UserAnswered")
                        .WithMany()
                        .HasForeignKey("UserAnsweredId");

                    b.HasOne("FittimePanelApi.Data.User", "UserCreated")
                        .WithMany()
                        .HasForeignKey("UserCreatedId");

                    b.Navigation("UserAnswered");

                    b.Navigation("UserCreated");
                });

            modelBuilder.Entity("FittimePanelApi.Data.TicketMessage", b =>
                {
                    b.HasOne("FittimePanelApi.Data.Ticket", "Ticket")
                        .WithMany("TicketMessages")
                        .HasForeignKey("TicketId");

                    b.HasOne("FittimePanelApi.Data.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Ticket");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FittimePanelApi.Data.TicketStatus", b =>
                {
                    b.HasOne("FittimePanelApi.Data.Ticket", "Ticket")
                        .WithMany("TicketStatuses")
                        .HasForeignKey("TicketId");

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("FittimePanelApi.Data.UserBlob", b =>
                {
                    b.HasOne("FittimePanelApi.Data.User", "User")
                        .WithMany("UserBlobs")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FittimePanelApi.Data.UserMeta", b =>
                {
                    b.HasOne("FittimePanelApi.Data.User", "User")
                        .WithMany("UserMetas")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("FittimePanelApi.Data.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("FittimePanelApi.Data.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FittimePanelApi.Data.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("FittimePanelApi.Data.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FittimePanelApi.Data.Exercise", b =>
                {
                    b.Navigation("ExerciseBlobs");

                    b.Navigation("ExerciseDownloads");

                    b.Navigation("ExerciseMetas");

                    b.Navigation("Payments");
                });

            modelBuilder.Entity("FittimePanelApi.Data.ExerciseType", b =>
                {
                    b.Navigation("Exercises");
                });

            modelBuilder.Entity("FittimePanelApi.Data.PaymentDiscount", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("FittimePanelApi.Data.PaymentGetaway", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("FittimePanelApi.Data.Ticket", b =>
                {
                    b.Navigation("TicketMessages");

                    b.Navigation("TicketStatuses");
                });

            modelBuilder.Entity("FittimePanelApi.Data.User", b =>
                {
                    b.Navigation("UserBlobs");

                    b.Navigation("UserMetas");
                });
#pragma warning restore 612, 618
        }
    }
}
