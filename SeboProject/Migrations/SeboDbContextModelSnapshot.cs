﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SeboProject.Data;

namespace SeboProject.Migrations
{
    [DbContext(typeof(SeboDbContext))]
    partial class SeboDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
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
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

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

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("SeboProject.Models.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Blocked");

                    b.Property<int>("BookConditionId");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Description")
                        .HasMaxLength(1000);

                    b.Property<int>("Edition");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasMaxLength(13);

                    b.Property<bool>("IsWaitList");

                    b.Property<byte[]>("PhotoFileName");

                    b.Property<double>("Price");

                    b.Property<string>("Publisher")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.Property<int>("Quantity");

                    b.Property<int>("QuantitySold");

                    b.Property<int>("StudyAreaId");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("UserId");

                    b.Property<int>("Visualizations");

                    b.HasKey("BookId");

                    b.HasIndex("BookConditionId");

                    b.HasIndex("StudyAreaId");

                    b.HasIndex("UserId");

                    b.ToTable("Book");
                });

            modelBuilder.Entity("SeboProject.Models.BookCondition", b =>
                {
                    b.Property<int>("BookConditionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Condition")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("BookConditionId");

                    b.ToTable("BookCondition");
                });

            modelBuilder.Entity("SeboProject.Models.Claim", b =>
                {
                    b.Property<int>("ClaimId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int>("OrderId");

                    b.Property<string>("Status")
                        .IsRequired();

                    b.Property<int>("UserId");

                    b.HasKey("ClaimId");

                    b.HasIndex("OrderId");

                    b.HasIndex("UserId");

                    b.ToTable("Claim");
                });

            modelBuilder.Entity("SeboProject.Models.ClaimMediation", b =>
                {
                    b.Property<int>("ClaimMediationId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Action")
                        .IsRequired();

                    b.Property<int>("ClaimId");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.HasKey("ClaimMediationId");

                    b.HasIndex("ClaimId");

                    b.ToTable("ClaimMediation");
                });

            modelBuilder.Entity("SeboProject.Models.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("InstitutionId");

                    b.Property<int>("StudyAreaId");

                    b.HasKey("CourseId");

                    b.HasIndex("InstitutionId");

                    b.HasIndex("StudyAreaId");

                    b.ToTable("Course");
                });

            modelBuilder.Entity("SeboProject.Models.CustomerService", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CodeNumber")
                        .IsRequired();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FullName")
                        .IsRequired();

                    b.Property<string>("Message");

                    b.Property<string>("PostCode")
                        .IsRequired();

                    b.HasKey("CustomerId");

                    b.ToTable("CustomerService");
                });

            modelBuilder.Entity("SeboProject.Models.Institution", b =>
                {
                    b.Property<int>("InstitutionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("InstitutionName")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.HasKey("InstitutionId");

                    b.ToTable("Institution");
                });

            modelBuilder.Entity("SeboProject.Models.InstitutionBranch", b =>
                {
                    b.Property<int>("InstitutionBranchId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("InstitutionBranchName")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.Property<int>("InstitutionId");

                    b.HasKey("InstitutionBranchId");

                    b.HasIndex("InstitutionId");

                    b.ToTable("InstitutionBranch");
                });

            modelBuilder.Entity("SeboProject.Models.Localization", b =>
                {
                    b.Property<int>("LocalizationId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PlaceName")
                        .IsRequired()
                        .HasMaxLength(170);

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(3);

                    b.Property<string>("Province")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("LocalizationId");

                    b.ToTable("Localization");
                });

            modelBuilder.Entity("SeboProject.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BookId");

                    b.Property<DateTime>("CancelationDate");

                    b.Property<DateTime>("CanfirmationDate");

                    b.Property<DateTime>("CreationDate");

                    b.Property<bool>("IsConfirmedByBuyer");

                    b.Property<DateTime>("PaymentDate");

                    b.Property<string>("PaymentForm");

                    b.Property<int>("Price");

                    b.Property<int>("Quantity");

                    b.Property<DateTime>("ReleaseDate");

                    b.Property<int>("SellerId");

                    b.Property<string>("Status");

                    b.Property<int>("UserId");

                    b.HasKey("OrderId");

                    b.HasIndex("BookId");

                    b.HasIndex("UserId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("SeboProject.Models.StudyArea", b =>
                {
                    b.Property<int>("StudyAreaId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("StudyAreaName")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("StudyAreaId");

                    b.ToTable("StudyArea");
                });

            modelBuilder.Entity("SeboProject.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasMaxLength(90);

                    b.Property<string>("AddressComplement");

                    b.Property<int>("Age");

                    b.Property<int?>("CourseId");

                    b.Property<double>("Creditcard");

                    b.Property<string>("CreditcardName")
                        .IsRequired()
                        .HasMaxLength(22);

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<int>("InstitutionBranchId");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<int>("LocalizationId");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(30);

                    b.Property<string>("Number");

                    b.Property<string>("Phone")
                        .IsRequired();

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("UserType");

                    b.Property<bool>("isBlocked");

                    b.HasKey("UserId");

                    b.HasIndex("CourseId");

                    b.HasIndex("InstitutionBranchId");

                    b.HasIndex("LocalizationId");

                    b.ToTable("User");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("SeboProject.Models.Seller", b =>
                {
                    b.HasBaseType("SeboProject.Models.User");


                    b.ToTable("Seller");

                    b.HasDiscriminator().HasValue("Seller");
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
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
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

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SeboProject.Models.Book", b =>
                {
                    b.HasOne("SeboProject.Models.BookCondition", "BookCondition")
                        .WithMany("Books")
                        .HasForeignKey("BookConditionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SeboProject.Models.StudyArea", "StudyArea")
                        .WithMany("Books")
                        .HasForeignKey("StudyAreaId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SeboProject.Models.User", "User")
                        .WithMany("Books")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("SeboProject.Models.Claim", b =>
                {
                    b.HasOne("SeboProject.Models.Order", "Order")
                        .WithMany("Claims")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SeboProject.Models.User", "User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("SeboProject.Models.ClaimMediation", b =>
                {
                    b.HasOne("SeboProject.Models.Claim", "Claim")
                        .WithMany("ClaimMediations")
                        .HasForeignKey("ClaimId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("SeboProject.Models.Course", b =>
                {
                    b.HasOne("SeboProject.Models.Institution", "Institution")
                        .WithMany("Courses")
                        .HasForeignKey("InstitutionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SeboProject.Models.StudyArea", "StudyArea")
                        .WithMany("Courses")
                        .HasForeignKey("StudyAreaId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("SeboProject.Models.InstitutionBranch", b =>
                {
                    b.HasOne("SeboProject.Models.Institution", "Institution")
                        .WithMany("InstitutionBranches")
                        .HasForeignKey("InstitutionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("SeboProject.Models.Order", b =>
                {
                    b.HasOne("SeboProject.Models.Book", "Book")
                        .WithMany("Orders")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SeboProject.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("SeboProject.Models.User", b =>
                {
                    b.HasOne("SeboProject.Models.Course")
                        .WithMany("Users")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SeboProject.Models.InstitutionBranch", "InstitutionBranch")
                        .WithMany("Users")
                        .HasForeignKey("InstitutionBranchId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SeboProject.Models.Localization", "Localization")
                        .WithMany("Users")
                        .HasForeignKey("LocalizationId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
