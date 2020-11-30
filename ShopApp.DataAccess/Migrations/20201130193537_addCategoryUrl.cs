using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopApp.DataAccess.Migrations
{
    public partial class addCategoryUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Categories");
        }
    }
}
