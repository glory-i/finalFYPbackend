using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace finalFYPbackend.Migrations
{
    public partial class isOtpExpiredproperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "dateExpired",
                table: "OTPs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dateExpired",
                table: "OTPs");
        }
    }
}
