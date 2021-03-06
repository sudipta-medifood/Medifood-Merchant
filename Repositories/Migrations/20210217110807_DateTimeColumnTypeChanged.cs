using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repositories.Migrations
{
    public partial class DateTimeColumnTypeChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SuspendedDate",
                table: "RestaurantMerchantProfiles",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "RestaurantMerchantProfiles",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "RestaurantMerchantProfiles",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BlockDate",
                table: "RestaurantMerchantProfiles",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "RefreshTokens",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "RefreshTokens",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SuspendedDate",
                table: "PharmacyMerchantProfiles",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "PharmacyMerchantProfiles",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "PharmacyMerchantProfiles",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BlockDate",
                table: "PharmacyMerchantProfiles",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "MerchantRestaurants",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "MerchantRestaurants",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "MerchantPharmacys",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "MerchantPharmacys",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SuspendedDate",
                table: "RestaurantMerchantProfiles",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastUpdatedAt",
                table: "RestaurantMerchantProfiles",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "RestaurantMerchantProfiles",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BlockDate",
                table: "RestaurantMerchantProfiles",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastUpdatedAt",
                table: "RefreshTokens",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "RefreshTokens",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "SuspendedDate",
                table: "PharmacyMerchantProfiles",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastUpdatedAt",
                table: "PharmacyMerchantProfiles",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "PharmacyMerchantProfiles",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BlockDate",
                table: "PharmacyMerchantProfiles",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastUpdatedAt",
                table: "MerchantRestaurants",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "MerchantRestaurants",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastUpdatedAt",
                table: "MerchantPharmacys",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "MerchantPharmacys",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}
