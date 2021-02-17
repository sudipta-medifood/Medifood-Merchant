using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Repositories.Migrations
{
    public partial class MerchantProfileCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PharmacyMerchantProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    MerchantPaymentType = table.Column<string>(nullable: true),
                    PaymentPeriod = table.Column<string>(nullable: true),
                    PharmacyName = table.Column<string>(nullable: true),
                    DrugLicenseNumber = table.Column<string>(nullable: true),
                    TradeLicenseNumber = table.Column<string>(nullable: true),
                    NidNumber = table.Column<string>(nullable: true),
                    PharmacyRatting = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ApprovedDate = table.Column<DateTime>(nullable: false),
                    BlockDate = table.Column<DateTime>(nullable: false),
                    SuspendedDate = table.Column<DateTime>(nullable: false),
                    LoginStatus = table.Column<int>(nullable: false),
                    PharmacyMerchantId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdatedAt = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyMerchantProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PharmacyMerchantProfiles_MerchantPharmacys_PharmacyMerchantId",
                        column: x => x.PharmacyMerchantId,
                        principalTable: "MerchantPharmacys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantMerchantProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    MerchantPaymentType = table.Column<string>(nullable: true),
                    PaymentPeriod = table.Column<string>(nullable: true),
                    RestaurantName = table.Column<string>(nullable: true),
                    TradeLicenseNumber = table.Column<string>(nullable: true),
                    NidNumber = table.Column<string>(nullable: true),
                    RestaurantRatting = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ApprovedDate = table.Column<DateTime>(nullable: false),
                    BlockDate = table.Column<DateTime>(nullable: false),
                    SuspendedDate = table.Column<DateTime>(nullable: false),
                    LoginStatus = table.Column<int>(nullable: false),
                    RestaurantMerchantId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdatedAt = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantMerchantProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestaurantMerchantProfiles_MerchantRestaurants_RestaurantMer~",
                        column: x => x.RestaurantMerchantId,
                        principalTable: "MerchantRestaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyMerchantProfiles_PharmacyMerchantId",
                table: "PharmacyMerchantProfiles",
                column: "PharmacyMerchantId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantMerchantProfiles_RestaurantMerchantId",
                table: "RestaurantMerchantProfiles",
                column: "RestaurantMerchantId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PharmacyMerchantProfiles");

            migrationBuilder.DropTable(
                name: "RestaurantMerchantProfiles");
        }
    }
}
