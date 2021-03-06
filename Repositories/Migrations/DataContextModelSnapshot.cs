﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repositories;

namespace Repositories.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

<<<<<<< HEAD
            modelBuilder.Entity("Models.Riders.RefreshToken", b =>
=======
            modelBuilder.Entity("Models.Merchants.MerchantPharmacy", b =>
>>>>>>> 69142915af0cedae9b642d72a42af8d86afd3ec1
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

<<<<<<< HEAD
                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime");
=======
                    b.Property<int>("AccountStatus")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("text");
>>>>>>> 69142915af0cedae9b642d72a42af8d86afd3ec1

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

<<<<<<< HEAD
                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime");
=======
                    b.Property<string>("DrugLicenceNumber")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");
>>>>>>> 69142915af0cedae9b642d72a42af8d86afd3ec1

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("datetime");

<<<<<<< HEAD
                    b.Property<string>("ReplacedByToken")
                        .HasColumnType("text");

                    b.Property<DateTime?>("Revoked")
                        .HasColumnType("datetime");

                    b.Property<int?>("RiderId")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RiderId");

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("Models.Riders.Rider", b =>
=======
                    b.Property<int>("LoginStatus")
                        .HasColumnType("int");

                    b.Property<string>("NidNumber")
                        .HasColumnType("text");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(4000)");

                    b.Property<DateTime?>("PasswordReset")
                        .HasColumnType("datetime");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(4000)");

                    b.Property<string>("PharmacyName")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<string>("ResetToken")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ResetTokenExpires")
                        .HasColumnType("datetime");

                    b.Property<string>("Role")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("MerchantPharmacys");
                });

            modelBuilder.Entity("Models.Merchants.MerchantRestaurant", b =>
>>>>>>> 69142915af0cedae9b642d72a42af8d86afd3ec1
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountStatus")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("text");

<<<<<<< HEAD
                    b.Property<string>("BicycleModelNumber")
                        .HasColumnType("text");

                    b.Property<string>("BikeRegistrationNumber")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("DrivingLicenceNumber")
                        .HasColumnType("text");

=======
                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

>>>>>>> 69142915af0cedae9b642d72a42af8d86afd3ec1
                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("datetime");

                    b.Property<int>("LoginStatus")
                        .HasColumnType("int");

                    b.Property<string>("NidNumber")
                        .HasColumnType("text");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(4000)");

                    b.Property<DateTime?>("PasswordReset")
                        .HasColumnType("datetime");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(4000)");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<string>("ResetToken")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ResetTokenExpires")
                        .HasColumnType("datetime");

<<<<<<< HEAD
                    b.Property<string>("Role")
                        .HasColumnType("text");

                    b.Property<string>("VehicleType")
                        .HasColumnType("text");

                    b.Property<string>("Zone")
=======
                    b.Property<string>("RestaurantName")
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("MerchantRestaurants");
                });

            modelBuilder.Entity("Models.Merchants.PharmacyMerchantProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<DateTime>("ApprovedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("BlockDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime");

                    b.Property<string>("DrugLicenseNumber")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("datetime");

                    b.Property<int>("LoginStatus")
                        .HasColumnType("int");

                    b.Property<string>("MerchantPaymentType")
                        .HasColumnType("text");

                    b.Property<string>("NidNumber")
                        .HasColumnType("text");

                    b.Property<string>("PaymentPeriod")
                        .HasColumnType("text");

                    b.Property<int>("PharmacyMerchantId")
                        .HasColumnType("int");

                    b.Property<string>("PharmacyName")
                        .HasColumnType("text");

                    b.Property<string>("PharmacyRatting")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<DateTime?>("SuspendedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("TradeLicenseNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PharmacyMerchantId")
                        .IsUnique();

                    b.ToTable("PharmacyMerchantProfiles");
                });

            modelBuilder.Entity("Models.Merchants.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("ReplacedByToken")
                        .HasColumnType("text");

                    b.Property<DateTime?>("Revoked")
                        .HasColumnType("datetime");

                    b.Property<string>("Token")
>>>>>>> 69142915af0cedae9b642d72a42af8d86afd3ec1
                        .HasColumnType("text");

                    b.HasKey("Id");

<<<<<<< HEAD
                    b.ToTable("Riders");
                });

            modelBuilder.Entity("Models.Riders.RefreshToken", b =>
                {
                    b.HasOne("Models.Riders.Rider", null)
                        .WithMany("RefreshTokens")
                        .HasForeignKey("RiderId");
=======
                    b.ToTable("RefreshTokens");

                    b.HasDiscriminator<string>("Discriminator").HasValue("RefreshToken");
                });

            modelBuilder.Entity("Models.Merchants.RestaurantMerchantProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<DateTime>("ApprovedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("BlockDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("datetime");

                    b.Property<int>("LoginStatus")
                        .HasColumnType("int");

                    b.Property<string>("MerchantPaymentType")
                        .HasColumnType("text");

                    b.Property<string>("NidNumber")
                        .HasColumnType("text");

                    b.Property<string>("PaymentPeriod")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<int>("RestaurantMerchantId")
                        .HasColumnType("int");

                    b.Property<string>("RestaurantName")
                        .HasColumnType("text");

                    b.Property<string>("RestaurantRatting")
                        .HasColumnType("text");

                    b.Property<DateTime?>("SuspendedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("TradeLicenseNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantMerchantId")
                        .IsUnique();

                    b.ToTable("RestaurantMerchantProfiles");
                });

            modelBuilder.Entity("Models.Merchants.RefTokenPharmacyMerchant", b =>
                {
                    b.HasBaseType("Models.Merchants.RefreshToken");

                    b.Property<int?>("MerchantPharmacyId")
                        .HasColumnType("int");

                    b.HasIndex("MerchantPharmacyId");

                    b.HasDiscriminator().HasValue("RefTokenPharmacyMerchant");
                });

            modelBuilder.Entity("Models.Merchants.RefTokenRestaurantMerchant", b =>
                {
                    b.HasBaseType("Models.Merchants.RefreshToken");

                    b.Property<int?>("MerchantRestaurantId")
                        .HasColumnType("int");

                    b.HasIndex("MerchantRestaurantId");

                    b.HasDiscriminator().HasValue("RefTokenRestaurantMerchant");
                });

            modelBuilder.Entity("Models.Merchants.PharmacyMerchantProfile", b =>
                {
                    b.HasOne("Models.Merchants.MerchantPharmacy", "MerchantPharmacy")
                        .WithOne("PharmacyMerchantProfiles")
                        .HasForeignKey("Models.Merchants.PharmacyMerchantProfile", "PharmacyMerchantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models.Merchants.RestaurantMerchantProfile", b =>
                {
                    b.HasOne("Models.Merchants.MerchantRestaurant", "MerchantRestaurant")
                        .WithOne("RestaurantMerchantProfiles")
                        .HasForeignKey("Models.Merchants.RestaurantMerchantProfile", "RestaurantMerchantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models.Merchants.RefTokenPharmacyMerchant", b =>
                {
                    b.HasOne("Models.Merchants.MerchantPharmacy", null)
                        .WithMany("RefTokenPharmacyMerchants")
                        .HasForeignKey("MerchantPharmacyId");
                });

            modelBuilder.Entity("Models.Merchants.RefTokenRestaurantMerchant", b =>
                {
                    b.HasOne("Models.Merchants.MerchantRestaurant", null)
                        .WithMany("RefTokenRestaurantMerchants")
                        .HasForeignKey("MerchantRestaurantId");
>>>>>>> 69142915af0cedae9b642d72a42af8d86afd3ec1
                });
#pragma warning restore 612, 618
        }
    }
}
