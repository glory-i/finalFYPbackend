using Microsoft.EntityFrameworkCore.Migrations;

namespace finalFYPbackend.Migrations
{
    public partial class removeImageFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoodImageData",
                table: "Meals");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FoodImageData",
                table: "Meals",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
