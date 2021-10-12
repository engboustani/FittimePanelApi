﻿// <auto-generated />
using System;
using FittimePanelApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FittimePanelApi.Migrations
{
    [DbContext(typeof(AppDb))]
    partial class AppDbModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<int?>("ExerciseStatusId")
                        .HasColumnType("int");

                    b.Property<int?>("ExerciseTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserInstructorId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UserStudentId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseStatusId");

                    b.HasIndex("ExerciseTypeId");

                    b.HasIndex("UserInstructorId");

                    b.HasIndex("UserStudentId");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("FittimePanelApi.Data.ExerciseDownload", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("ExerciseId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Uri")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.ToTable("ExerciseDownloads");
                });

            modelBuilder.Entity("FittimePanelApi.Data.ExerciseMeta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

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

            modelBuilder.Entity("FittimePanelApi.Data.ExerciseStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("ExerciseStatus");
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

                    b.Property<int>("Price")
                        .HasColumnType("int");

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

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("PaymentGetwayId")
                        .HasColumnType("int");

                    b.Property<int?>("PaymentStatusId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("UserId1")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("PaymentGetwayId");

                    b.HasIndex("PaymentStatusId");

                    b.HasIndex("UserId1");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("FittimePanelApi.Data.PaymentGetway", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("PaymentGetways");
                });

            modelBuilder.Entity("FittimePanelApi.Data.PaymentMeta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Key")
                        .HasColumnType("longtext");

                    b.Property<Guid?>("PaymentId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("PaymentId");

                    b.ToTable("PaymentMetas");
                });

            modelBuilder.Entity("FittimePanelApi.Data.PaymentStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("PaymentStatus");
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

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("UserGroupId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.HasIndex("UserGroupId");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("FittimePanelApi.Data.UserGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("UserGroups");
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

            modelBuilder.Entity("FittimePanelApi.Data.UserRule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UserGroupId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserGroupId");

                    b.ToTable("UserRules");
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
                            Id = "ba8145e0-1944-4ae2-8275-e3d23bdca367",
                            ConcurrencyStamp = "f4138331-0573-4d76-8203-3730896c6840",
                            Name = "User",
                            NormalizedName = "USER"
                        },
                        new
                        {
                            Id = "a8277e51-fc3c-49bb-a550-92fe9f5c6308",
                            ConcurrencyStamp = "e1ce438b-99ac-4f91-ad4c-3a2bceea1da3",
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
                    b.HasOne("FittimePanelApi.Data.ExerciseStatus", "ExerciseStatus")
                        .WithMany()
                        .HasForeignKey("ExerciseStatusId");

                    b.HasOne("FittimePanelApi.Data.ExerciseType", "ExerciseType")
                        .WithMany()
                        .HasForeignKey("ExerciseTypeId");

                    b.HasOne("FittimePanelApi.Data.User", "UserInstructor")
                        .WithMany()
                        .HasForeignKey("UserInstructorId");

                    b.HasOne("FittimePanelApi.Data.User", "UserStudent")
                        .WithMany()
                        .HasForeignKey("UserStudentId");

                    b.Navigation("ExerciseStatus");

                    b.Navigation("ExerciseType");

                    b.Navigation("UserInstructor");

                    b.Navigation("UserStudent");
                });

            modelBuilder.Entity("FittimePanelApi.Data.ExerciseDownload", b =>
                {
                    b.HasOne("FittimePanelApi.Data.Exercise", "Exercise")
                        .WithMany("ExerciseDownloads")
                        .HasForeignKey("ExerciseId");

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
                    b.HasOne("FittimePanelApi.Data.PaymentGetway", null)
                        .WithMany("Payments")
                        .HasForeignKey("PaymentGetwayId");

                    b.HasOne("FittimePanelApi.Data.PaymentStatus", null)
                        .WithMany("Payments")
                        .HasForeignKey("PaymentStatusId");

                    b.HasOne("FittimePanelApi.Data.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId1");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FittimePanelApi.Data.PaymentMeta", b =>
                {
                    b.HasOne("FittimePanelApi.Data.Payment", "Payment")
                        .WithMany()
                        .HasForeignKey("PaymentId");

                    b.Navigation("Payment");
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

            modelBuilder.Entity("FittimePanelApi.Data.User", b =>
                {
                    b.HasOne("FittimePanelApi.Data.UserGroup", "UserGroup")
                        .WithMany("Users")
                        .HasForeignKey("UserGroupId");

                    b.Navigation("UserGroup");
                });

            modelBuilder.Entity("FittimePanelApi.Data.UserMeta", b =>
                {
                    b.HasOne("FittimePanelApi.Data.User", "User")
                        .WithMany("UserMetas")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FittimePanelApi.Data.UserRule", b =>
                {
                    b.HasOne("FittimePanelApi.Data.UserGroup", "UserGroup")
                        .WithMany("UserRules")
                        .HasForeignKey("UserGroupId");

                    b.Navigation("UserGroup");
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
                    b.Navigation("ExerciseDownloads");

                    b.Navigation("ExerciseMetas");
                });

            modelBuilder.Entity("FittimePanelApi.Data.PaymentGetway", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("FittimePanelApi.Data.PaymentStatus", b =>
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
                    b.Navigation("UserMetas");
                });

            modelBuilder.Entity("FittimePanelApi.Data.UserGroup", b =>
                {
                    b.Navigation("UserRules");

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
